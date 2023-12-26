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

namespace EvFtpFileDialog.Forms
{
    public partial class FtpOpenDialogForm : Form
    {
        public string Address { get; set; }
        public string StartupPath { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public FtpWebRequest FtpConnection { get; private set; }

        public FtpOpenDialogForm(string address, string startupPath, string userName, string password)
        {
            InitializeComponent();

            Address = address;
            StartupPath = startupPath;
            UserName = userName;
            Password = password;

            CreateConnection();
            ListDirectoryDetails();
        }

        private void CreateConnection()
        {
            FtpConnection = (FtpWebRequest)FtpWebRequest.Create($"ftp://{Address}{StartupPath}");
            FtpConnection.Credentials = new NetworkCredential(UserName, Password);
        }

        public List<string> ListDirectoryDetails()
        {
            FtpConnection.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            WebResponse response = FtpConnection.GetResponse();
            List<string> result = new List<string>();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                    result.Add(reader.ReadLine());
            }
            return result;
        }
    }
}
