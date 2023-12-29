using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EvFtpFileDialog.Controls
{
    [ToolboxBitmap(typeof(Label))]
    [Description("Provide a shortcut to back to above directory")]
    public partial class NavigationBar : Control
    {
        private Regex _RegexStartUp = new Regex("[^/]+.*[^/]");
        private List<string> _Directorys = new List<string>() { "" };
        private List<string> _PreviousDirectorys = new List<string>() { "" };

        /// <summary>
        /// Occurs when path is validating
        /// </summary>
        [Description("Occurs when path is validating")]
        public new event CancelEventHandler Validating;
        protected bool OnValidating()
        {
            if (Validating != null)
            {
                CancelEventArgs args = new CancelEventArgs();
                Validating.Invoke(this, args);
                return args.Cancel;
            }
            return false;
        }

        private bool _ShowStartupPath = false;
        /// <summary>
        /// Show startup path or show [root] instead
        /// </summary>
        [Description("Show startup path or show [root] instead"), DefaultValue(false)]
        public bool ShowStartupPath
        {
            get { return _ShowStartupPath; }
            set
            {
                if (_ShowStartupPath == value) return;
                _ShowStartupPath = value;
                RebuildItems();
            }
        }

        /// <summary>
        /// Original path, can't go above from it
        /// </summary>
        [DefaultValue("Original path, can't go above from it")]
        public string StartupPath
        {
            get { return _Directorys[0]; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    value = _RegexStartUp.Match(value).Value;
                if (_Directorys[0] == value) return;
                _Directorys[0] = value;
                RebuildItems();
            }
        }

        /// <summary>
        /// Full path
        /// </summary>
        [DefaultValue("Full path")]
        public string Path
        {
            get { return (string.IsNullOrWhiteSpace(StartupPath) ? "" : "/") + string.Join("/", _Directorys) + "/"; }
        }

        /// <summary>
        /// Initializes a new instance of NavigationBar
        /// </summary>
        public NavigationBar()
        {
            InitializeComponent();
        }

        private void DirectoryClicked(object sender, EventArgs e)
        {
            int length = (int)((sender as Control).Tag);
            if (_Directorys.Count <= length) return;

            _PreviousDirectorys = new List<string>(_Directorys);
            while (_Directorys.Count > length)
            {
                _Directorys.RemoveAt(_Directorys.Count - 1);
            }
            if (OnValidating())
            {
                _Directorys = _PreviousDirectorys;
                _PreviousDirectorys = null;
            }
            else
            {
                RebuildItems();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Relayout();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.DrawRectangle(Pens.Gray, 0, 0, Width - 1, Height - 1);
            base.OnPaint(pe);
        }

        /// <summary>
        /// Reinitialized path
        /// </summary>
        public void ClearDirectory()
        {
            _Directorys.RemoveRange(1, _Directorys.Count - 1);
            RebuildItems();
            OnValidating();
        }

        /// <summary>
        /// Go to above directory
        /// </summary>
        public void LeaveDirectory()
        {
            if (_Directorys.Count > 1)
            {
                _PreviousDirectorys = new List<string>(_Directorys);
                _Directorys.RemoveAt(_Directorys.Count - 1);
                if (OnValidating())
                {
                    _Directorys = _PreviousDirectorys;
                    _PreviousDirectorys = null;
                }
                else
                {
                    RebuildItems();
                }
            }
        }

        /// <summary>
        /// Enter to directory
        /// </summary>
        /// <param name="directory">Name of directory</param>
        public void EnterDirectory(string directory)
        {
            _PreviousDirectorys = new List<string>(_Directorys);
            _Directorys.Add(directory);
            if (OnValidating())
            {
                _Directorys = _PreviousDirectorys;
                _PreviousDirectorys = null;
            }
            else
            {
                RebuildItems();
            }
        }

        /// <summary>
        /// Enter to directory[0]/directory[1]...
        /// </summary>
        /// <param name="directory">Names of directorys</param>
        public void EnterDirectory(IEnumerable<string> directory)
        {
            _PreviousDirectorys = new List<string>(_Directorys);
            _Directorys.AddRange(directory);
            if (OnValidating())
            {
                _Directorys = _PreviousDirectorys;
                _PreviousDirectorys = null;
            }
            else
            {
                RebuildItems();
            }
        }

        /// <summary>
        /// Resotre last change
        /// </summary>
        public void Rockback()
        {
            if (_PreviousDirectorys == null) return;
            _Directorys = _PreviousDirectorys;
            _PreviousDirectorys = null;
            RebuildItems();
        }


        /// <summary>
        /// Rebuild labels and buttons
        /// </summary>
        private void RebuildItems()
        {
            Controls.Clear();
            int height = 0;
            for (int i = 0; i < _Directorys.Count; i++)
            {
                string text = i == 0 && (ShowStartupPath || string.IsNullOrWhiteSpace(StartupPath)) ? "root" : _Directorys[i];
                Button node = new Button()
                {
                    Text = text,
                    Tag = i + 1,
                    Size = new Size(0, 0),
                    AutoSize = true,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = BackColor,
                    TabStop = false
                };

                node.FlatAppearance.BorderSize = 0;
                node.Click += DirectoryClicked;
                Controls.Add(node);
                height = node.Height;

                Label nodeLable = new Label()
                {
                    Text = "/",
                    Size = new Size(0, 0),
                    MinimumSize = new Size(0, height),
                    TextAlign = ContentAlignment.MiddleCenter,
                    ForeColor = Color.LightSlateGray,
                    AutoSize = true
                };
                Controls.Add(nodeLable);
            }
            Relayout();
            Refresh();
        }

        /// <summary>
        /// Rearrange all controls to fit the size
        /// </summary>
        private void Relayout()
        {
            if (Controls.Count == 0) return;
            int left = 10;
            int height = Controls[0].Height;
            int top = (Height - height) / 2;
            foreach (Control control in Controls)
            {
                control.Left = left;
                control.Top = top;
                left += control.Width;
            }
            Refresh();
        }
    }
}
