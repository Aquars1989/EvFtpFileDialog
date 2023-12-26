using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EvFtpFileDialog.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ftpOpenDialog1.FtpAddress = TestInfo.Address;
            ftpOpenDialog1.FtpStartupPath = TestInfo.StartupPath;
            ftpOpenDialog1.FtpUserName = TestInfo.UserName;
            ftpOpenDialog1.FtpPassword = TestInfo.Password;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ftpOpenDialog1.ShowDialog();
        }
    }
}
