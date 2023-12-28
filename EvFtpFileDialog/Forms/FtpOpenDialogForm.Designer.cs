
namespace EvFtpFileDialog.Forms
{
    partial class FtpOpenDialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Directorys", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Files", System.Windows.Forms.HorizontalAlignment.Left);
            this.ftpLists = new System.Windows.Forms.ListView();
            this.columnDIrectory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnModifiedTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.navigationBar = new EvFtpFileDialog.Controls.NavigationBar();
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // ftpLists
            // 
            this.ftpLists.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ftpLists.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnDIrectory,
            this.columnFileName,
            this.columnType,
            this.columnSize,
            this.columnModifiedTime});
            this.ftpLists.FullRowSelect = true;
            listViewGroup1.Header = "Directorys";
            listViewGroup1.Name = "groupDirectory";
            listViewGroup2.Header = "Files";
            listViewGroup2.Name = "groupFiles";
            this.ftpLists.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.ftpLists.HideSelection = false;
            this.ftpLists.Location = new System.Drawing.Point(13, 55);
            this.ftpLists.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ftpLists.MultiSelect = false;
            this.ftpLists.Name = "ftpLists";
            this.ftpLists.Size = new System.Drawing.Size(714, 440);
            this.ftpLists.TabIndex = 0;
            this.ftpLists.UseCompatibleStateImageBehavior = false;
            this.ftpLists.View = System.Windows.Forms.View.Details;
            this.ftpLists.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.ftpLists_ColumnClick);
            this.ftpLists.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.ftpLists_ItemSelectionChanged);
            this.ftpLists.DoubleClick += new System.EventHandler(this.ftpLists_DoubleClick);
            this.ftpLists.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ftpLists_KeyDown);
            this.ftpLists.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ftpLists_KeyPress);
            // 
            // columnDIrectory
            // 
            this.columnDIrectory.Text = "";
            this.columnDIrectory.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnDIrectory.Width = 34;
            // 
            // columnFileName
            // 
            this.columnFileName.Text = "FileName";
            this.columnFileName.Width = 345;
            // 
            // columnSize
            // 
            this.columnSize.Text = "Size";
            this.columnSize.Width = 104;
            // 
            // columnModifiedTime
            // 
            this.columnModifiedTime.Text = "ModifiedTime";
            this.columnModifiedTime.Width = 167;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Enabled = false;
            this.btnSelect.Location = new System.Drawing.Point(463, 503);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(124, 29);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "Select (&S)";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(603, 503);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(124, 29);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // navigationBar
            // 
            this.navigationBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.navigationBar.BackColor = System.Drawing.Color.White;
            this.navigationBar.Location = new System.Drawing.Point(13, 13);
            this.navigationBar.Margin = new System.Windows.Forms.Padding(2);
            this.navigationBar.Name = "navigationBar";
            this.navigationBar.ShowStartupPath = true;
            this.navigationBar.Size = new System.Drawing.Size(714, 36);
            this.navigationBar.TabIndex = 3;
            this.navigationBar.Text = "navigationBar1";
            this.navigationBar.Validating += new System.ComponentModel.CancelEventHandler(this.navigationBar_Validating);
            // 
            // columnType
            // 
            this.columnType.Text = "Type";
            // 
            // FtpOpenDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(739, 545);
            this.Controls.Add(this.ftpLists);
            this.Controls.Add(this.navigationBar);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Font = new System.Drawing.Font("Consolas", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FtpOpenDialogForm";
            this.Text = "Select Ftp file";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView ftpLists;
        private System.Windows.Forms.ColumnHeader columnFileName;
        private System.Windows.Forms.ColumnHeader columnSize;
        private System.Windows.Forms.ColumnHeader columnModifiedTime;
        private System.Windows.Forms.ColumnHeader columnDIrectory;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private Controls.NavigationBar navigationBar;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ColumnHeader columnType;
    }
}