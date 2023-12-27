using EvFtpFileDialog.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EvFtpFileDialog
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(FtpOpenDialog))]
    public class FtpOpenDialog : Component
    {
        [Description("The address of ftp server"), DefaultValue("")]
        public string FtpAddress { get; set; }

        [Description("The start path of ftp server"), DefaultValue("")]
        public string FtpStartupPath { get; set; }

        [Description("The user name of account"), DefaultValue("")]
        public string FtpUserName { get; set; }

        [Description("The password of account"), DefaultValue("")]
        public string FtpPassword { get; set; }

        [Description("The result return from dialog box"), DefaultValue("")]
        public string FileName { get; set; }

        public FtpOpenDialog()
        {
        }

        public FtpOpenDialog(string address, string startupPath, string userName, string password) : base()
        {
            FtpAddress = address;
            FtpStartupPath = startupPath;
            FtpUserName = userName;
            FtpPassword = password;
        }

        public DialogResult ShowDialog()
        {
            using (FtpOpenDialogForm form = new FtpOpenDialogForm(FtpAddress, FtpStartupPath, FtpUserName, FtpPassword, FileName))
            {
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    FileName = form.FileName;
                }
                return result;
            }
        }
    }
}
