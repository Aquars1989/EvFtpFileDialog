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
    public partial class NavigationBar : Control
    {
        private Regex _RegexStartUp = new Regex("[^/]+.*[^/]");
        private List<string> _Directorys = new List<string>() { "" };

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

        [DefaultValue("")]
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

        public string Path
        {
            get { return "/" + string.Join("/", _Directorys) + "/"; }
        }



        public NavigationBar()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.DrawRectangle(Pens.Gray, 0, 0, Width - 1, Height - 1);
            base.OnPaint(pe);
        }

        public void ClearDirectory()
        {
            _Directorys.RemoveRange(1, _Directorys.Count - 1);
            RebuildItems();
            OnValidating();
        }

        public void LeaveDirectory()
        {
            if (_Directorys.Count > 1)
            {
                string backUp = _Directorys.Last();
                _Directorys.RemoveAt(_Directorys.Count - 1);
                if (OnValidating())
                {
                    _Directorys.Add(backUp);
                }
                else
                {
                    RebuildItems();
                }
            }
        }

        public void EnterDirectory(string directory)
        {
            _Directorys.Add(directory);
            if (OnValidating())
            {
                _Directorys.RemoveAt(_Directorys.Count - 1);
            }
            else
            {
                RebuildItems();
            }
        }

        public void EnterDirectory(IEnumerable<string> directory)
        {
            _Directorys.AddRange(directory);
            if (OnValidating())
            {
                for (int i = 0; i < directory.Count(); i++)
                {
                    _Directorys.RemoveAt(_Directorys.Count - 1);
                }
            }
            else
            {
                RebuildItems();
            }
        }

        private void RebuildItems()
        {
            Controls.Clear();
            int height = 0;
            for (int i = 0; i < _Directorys.Count; i++)
            {
                Button node = new Button()
                {
                    Text = _Directorys[i],
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

        private void DirectoryClicked(object sender, EventArgs e)
        {
            int length = (int)((sender as Control).Tag);
            if (_Directorys.Count <= length) return;

            Stack<string> backUp = new Stack<string>();
            while (_Directorys.Count > length)
            {
                backUp.Push(_Directorys.Last());
                _Directorys.RemoveAt(_Directorys.Count - 1);
            }
            if (OnValidating())
            {
                _Directorys.AddRange(backUp);
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
    }
}
