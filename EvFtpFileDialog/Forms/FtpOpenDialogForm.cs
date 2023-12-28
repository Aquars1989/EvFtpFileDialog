using FluentFTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace EvFtpFileDialog.Forms
{
    public partial class FtpOpenDialogForm : Form
    {
        private Color _DirectorysForeColor = Color.DarkOliveGreen;
        private Color _FilesForeColor = Color.DarkSlateGray;

        public string Address { get; set; }
        public string StartupPath { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FileName { get; set; }

        public FtpClient FtpClient { get; private set; }

        public FtpOpenDialogForm(string address, string startupPath, string userName, string password, string fileName)
        {
            InitializeComponent();

            Address = address;
            StartupPath = startupPath;
            UserName = userName;
            Password = password;
            FileName = fileName;
            navigationBar.StartupPath = startupPath;

            CreateConnection();
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                string[] startupParts = startupPath.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                string[] fileNameParts = fileName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (fileNameParts.Length == 0)
                {
                    LoadDirectory();
                    return;
                }

                List<string> directorys = new List<string>();
                for (int i = 0; i < fileNameParts.Length - 1; i++)
                {
                    if (i < startupParts.Length)
                    {
                        if (!startupParts[i].Equals(fileNameParts[i], StringComparison.OrdinalIgnoreCase))
                        {
                            LoadDirectory();
                            return;
                        }
                    }
                    else
                    {
                        directorys.Add(fileNameParts[i]);
                    }
                }
                navigationBar.EnterDirectory(directorys);
                string file = fileNameParts[fileNameParts.Length - 1];
                foreach (ListViewItem item in ftpLists.Items)
                {
                    if (item.Group == ftpLists.Groups[1] && item.SubItems[1].Text.Equals(file, StringComparison.OrdinalIgnoreCase))
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
            else
            {
                LoadDirectory();
            }
        }

        private void CreateConnection()
        {
            FtpClient = new FtpClient($"ftp://{Address}", UserName, Password);
        }

        public bool LoadDirectory()
        {
            FtpListItem[] items;
            try
            {
                items = FtpClient.GetListing($"{navigationBar.Path}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

            btnSelect.Enabled = false;
            ftpLists.Items.Clear();
            if (navigationBar.Path != StartupPath)
            {
                ListViewItem listItemAbove = new ListViewItem("📁") { ForeColor = _DirectorysForeColor };
                listItemAbove.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = ".." });
                listItemAbove.Group = ftpLists.Groups[0];
                ftpLists.Items.Add(listItemAbove);
            }

            foreach (FtpListItem item in items)
            {
                bool isDirectory = item.Type == FtpObjectType.Directory;
                ListViewItem listItem = new ListViewItem(isDirectory ? "📁" : "") { ForeColor = isDirectory ? _DirectorysForeColor : _FilesForeColor };
                listItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = item.Name });
                listItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = isDirectory ? "<DIR>" : item.Size.ToString() });
                listItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = item.Modified.ToString("yyyy/MM/dd HH:mm:ss") });
                listItem.Group = isDirectory ? ftpLists.Groups[0] : ftpLists.Groups[1];
                ftpLists.Items.Add(listItem);
            }
            return true;
        }

        private void ftpLists_DoubleClick(object sender, EventArgs e)
        {
            btnSelect_Click(null, null);
        }

        private void ftpLists_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Back:
                    navigationBar.LeaveDirectory();
                    break;
                case Keys.Enter:
                    ftpLists_DoubleClick(null, null);
                    break;
            }
        }

        private void ftpLists_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if ((e.KeyChar >= 'a' && e.KeyChar <= 'z') || e.KeyChar >= 'A' && e.KeyChar <= 'Z')
            //{
            string key = e.KeyChar.ToString();
            ListViewItem matchItem = null;
            if (ftpLists.SelectedItems.Count == 0)
            {
                matchItem = FindPrefix(key, ftpLists.Groups[0].Items);
                if (matchItem == null)
                    matchItem = FindPrefix(key, ftpLists.Groups[1].Items);
            }
            else
            {
                if (ftpLists.SelectedItems[0].Group == ftpLists.Groups[0])
                {
                    matchItem = FindPrefix(key, ftpLists.Groups[0].Items, ftpLists.SelectedItems[0], true);
                    if (matchItem == null)
                        matchItem = FindPrefix(key, ftpLists.Groups[1].Items);
                    if (matchItem == null)
                        matchItem = FindPrefix(key, ftpLists.Groups[0].Items, ftpLists.SelectedItems[0], false);
                }
                else
                {
                    matchItem = FindPrefix(key, ftpLists.Groups[1].Items, ftpLists.SelectedItems[0], true);
                    if (matchItem == null)
                        matchItem = FindPrefix(key, ftpLists.Groups[0].Items);
                    if (matchItem == null)
                        matchItem = FindPrefix(key, ftpLists.Groups[1].Items, ftpLists.SelectedItems[0], false);
                }
            }
            if (matchItem != null)
            {
                matchItem.Selected = true;
                matchItem.EnsureVisible();
            }
            e.Handled = true;
            //}
        }

        private void navigationBar_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !LoadDirectory();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (ftpLists.SelectedItems.Count == 0) return;

            ListViewItem item = ftpLists.SelectedItems[0];
            if (item.Group == ftpLists.Groups[0])
            {
                if (item.SubItems[1].Text == "..")
                {
                    navigationBar.LeaveDirectory();
                }
                else
                {
                    navigationBar.EnterDirectory(item.SubItems[1].Text);
                }
            }
            else
            {
                FileName = navigationBar.Path + item.SubItems[1].Text;
                DialogResult = DialogResult.OK;
            }
        }

        private void ftpLists_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            btnSelect.Enabled = e.IsSelected;
        }

        /// <summary>
        /// search listView with prefix, return the first match item
        /// </summary>
        /// <param name="prefix">prefix for search listView</param>
        /// <param name="collection">ListViewItem collection</param>
        /// <returns>match item</returns>
        private ListViewItem FindPrefix(string prefix, ListViewItemCollection collection, ListViewItem current = null, bool findAfter = true)
        {
            if (current == null)
            {
                foreach (ListViewItem item in collection)
                {
                    if (item.SubItems[1].Text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                        return item;
                }
            }
            else
            {
                if (findAfter)
                {
                    bool check = false;
                    foreach (ListViewItem item in collection)
                    {
                        if (check && item.SubItems[1].Text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                            return item;
                        if (item == current)
                            check = true;
                    }
                }
                else
                {
                    foreach (ListViewItem item in collection)
                    {
                        if (item == current)
                            return null;
                        if (item.SubItems[1].Text.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                            return item;
                    }
                }
            }
            return null;
        }
    }
}
