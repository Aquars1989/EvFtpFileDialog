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
    [ToolboxBitmap(typeof(OpenFileDialog))]
    [Description("Display a dialog box that select a file on ftp")]
    public class FtpOpenDialog : Component
    {
        /// <summary>
        /// Address of ftp server
        /// </summary>
        [Description("The address of ftp server"), DefaultValue("")]
        public string FtpAddress { get; set; }

        /// <summary>
        /// Start path of ftp server
        /// </summary>
        [Description("The start path of ftp server"), DefaultValue("")]
        public string FtpStartupPath { get; set; }

        /// <summary>
        /// User name of account
        /// </summary>
        [Description("The user name of account"), DefaultValue("")]
        public string FtpUserName { get; set; }

        /// <summary>
        /// Password of account
        /// </summary>
        [Description("The password of account"), DefaultValue("")]
        public string FtpPassword { get; set; }

        /// <summary>
        /// Result return from dialog box
        /// </summary>
        [Description("The result return from dialog box"), DefaultValue("")]
        public string FileName { get; set; }

        /// <summary>
        /// Initializes a new instance of FtpOpenDialog
        /// </summary>
        public FtpOpenDialog()
        {
        }

        /// <summary>
        /// Initializes a new instance of FtpOpenDialog with ftp setting
        /// </summary>
        public FtpOpenDialog(string address, string startupPath, string userName, string password) : base()
        {
            FtpAddress = address;
            FtpStartupPath = startupPath;
            FtpUserName = userName;
            FtpPassword = password;
        }

        /// <summary>
        /// Show a dialog box to choice a file on ftp, return OK if success, otherwise return Cancel 
        /// </summary>
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
