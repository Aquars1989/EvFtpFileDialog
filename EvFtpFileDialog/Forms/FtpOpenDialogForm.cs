using EvFtpFileDialog.Classes;
using FluentFTP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace EvFtpFileDialog.Forms
{
    public partial class FtpOpenDialogForm : Form
    {
        ListViewColumnSorter _FtpListSorter = new ListViewColumnSorter();
        private Color _DirectorysForeColor = Color.DarkOliveGreen;
        private Color _FilesForeColor = Color.DarkSlateGray;

        /// <summary>
        /// Address of ftp server
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Startup path of ftp server
        /// </summary>
        public string StartupPath { get; set; }

        /// <summary>
        /// User name of account
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password of account
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Result full path
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Ftp Client
        /// </summary>
        public FtpClient FtpClient { get; private set; }

        /// <summary>
        /// Initializes a new instance of FtpOpenDialogForm with ftp setting and previous value
        /// </summary>
        /// <param name="address">Address of ftp server</param>
        /// <param name="startupPath">Startup path of ftp server</param>
        /// <param name="userName">User name of account</param>
        /// <param name="password">Password of account</param>
        /// <param name="fileName">Previous value</param>
        public FtpOpenDialogForm(string address, string startupPath, string userName, string password, string fileName)
        {
            InitializeComponent();

            Address = address;
            StartupPath = startupPath;
            UserName = userName;
            Password = password;
            FileName = fileName;
            navigationBar.StartupPath = startupPath;
            ftpLists.ListViewItemSorter = _FtpListSorter;

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
            //e.Cancel = !LoadDirectory();
            LoadDirectory();
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

        private void ftpLists_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == _FtpListSorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (_FtpListSorter.Order == SortOrder.Ascending)
                {
                    _FtpListSorter.Order = SortOrder.Descending;
                }
                else
                {
                    _FtpListSorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                _FtpListSorter.SortColumn = e.Column;
                _FtpListSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.ftpLists.Sort();

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

        private void CreateConnection()
        {
            FtpClient = new FtpClient($"ftp://{Address}", UserName, Password);
        }

        private void LoadDirectory()
        {
            LockScreen();
            Thread threadLoad = new Thread(() =>
            {
                try
                {
                    FtpListItem[] items = FtpClient.GetListing($"{navigationBar.Path}");
                    RefreshFtpView(items);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    NavigationRockback();
                }
                finally
                {
                    UnlockScreen();
                }
            });
            threadLoad.Start();
        }

        private delegate void deleFtpListItem(FtpListItem[] items);
        private void RefreshFtpView(FtpListItem[] items)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new deleFtpListItem(RefreshFtpView), new object[] { items });
            }
            else
            {
                ftpLists.Items.Clear();
                if (navigationBar.Path != StartupPath)
                {
                    ListViewItem listItemAbove = new ListViewItem("📁") { ForeColor = _DirectorysForeColor };
                    listItemAbove.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = ".." });
                    listItemAbove.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = "" });
                    listItemAbove.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = "" });
                    listItemAbove.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = "" });
                    listItemAbove.Group = ftpLists.Groups[0];
                    ftpLists.Items.Add(listItemAbove);
                }

                foreach (FtpListItem item in items)
                {
                    bool isDirectory = item.Type == FtpObjectType.Directory;
                    ListViewItem listItem = new ListViewItem(isDirectory ? "📁" : "") { ForeColor = isDirectory ? _DirectorysForeColor : _FilesForeColor };
                    listItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = item.Name });
                    listItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = isDirectory ? "<DIR>" : Path.GetExtension(item.Name) });
                    listItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = isDirectory ? "<DIR>" : item.Size.ToString() });
                    listItem.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = item.Modified.ToString("yyyy/MM/dd HH:mm:ss") });
                    listItem.Group = isDirectory ? ftpLists.Groups[0] : ftpLists.Groups[1];
                    ftpLists.Items.Add(listItem);
                }
            }
        }

        private void LockScreen()
        {
            if (!loadingPanel1.Visible)
            {
                loadingPanel1.Left = ftpLists.Left + 1;
                loadingPanel1.Top = ftpLists.Top + 1;
                loadingPanel1.Width = ftpLists.Width - 2;
                loadingPanel1.Height = ftpLists.Height - 2;
                loadingPanel1.BackgroundImage?.Dispose();
                loadingPanel1.BackgroundImage = null;
                Bitmap temp = new Bitmap(ftpLists.Width, ftpLists.Height);
                ftpLists.DrawToBitmap(temp, new Rectangle(0, 0, ftpLists.Width, ftpLists.Height));
                Bitmap image = ImageFunc.Blur(temp, 1, Color.FromArgb(150, ftpLists.BackColor));
                temp.Dispose();
                loadingPanel1.BackgroundImage = image;
                loadingPanel1.Visible = true;
            }
            loadingPanel1.Text = "Loading";
            loadingPanel1.Invalidate();
            navigationBar.Enabled = false;
            ftpLists.Enabled = false;
            btnSelect.Enabled = false;
        }

        private void UnlockScreen()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UnlockScreen));
            }
            else
            {
                navigationBar.Enabled = true;
                ftpLists.Enabled = true;
                loadingPanel1.Visible = false;
            }
        }
        private void NavigationRockback()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(NavigationRockback));
            }
            else
            {
                navigationBar.Rockback();
            }
        }
    }
}
