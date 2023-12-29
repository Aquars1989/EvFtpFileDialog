using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EvFtpFileDialog.Controls
{
    public partial class LoadingPanel : Control
    {
        private static StringFormat _FormatMiddleCenter = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        private Timer _Timer = new Timer() { Interval = 10 };
        private int _AnimationTick = 0;

        private string _Text2 = "";
        public string Text2
        {
            get { return _Text2; }
            set
            {
                _Text2 = value;
                Invalidate();
            }
        }

        public LoadingPanel()
        {
            InitializeComponent();
            DoubleBuffered = true;
            _Timer.Tick += (x, e) =>
            {
                if (Visible)
                {
                    _AnimationTick = (_AnimationTick + 3) % 100;
                    Invalidate();
                }
            };
            _Timer.Enabled = true;
        }

        private static SolidBrush _BrushAnimation = new SolidBrush(Color.FromArgb(120, 140, 180));
        private static SolidBrush _BrushAlertCover = new SolidBrush(Color.FromArgb(20, 255, 80, 80));
        private static SolidBrush _BrushText1 = new SolidBrush(Color.Black);
        private static SolidBrush _BrushText2 = new SolidBrush(Color.Firebrick);
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            int space = 15;
            int left = (int)(Width / 2 - (space * 4 + 4)/2 );
            int top = Height / 2 + 40;
            for (int i = 0; i < 5; i++)
            {
                int offset = (int)((25 - Math.Abs(50 - _AnimationTick)) * 0.3F);
                if (i % 2 == 0)
                    offset = -offset;
                pe.Graphics.FillRectangle(_BrushAnimation, left - 4, top - 4 - offset / 2, 8, 16 + offset);
                left += space;
            }

            if (!string.IsNullOrWhiteSpace(Text))
            {
                if (string.IsNullOrWhiteSpace(Text2))
                {
                    pe.Graphics.DrawString(Text, Font, _BrushText1, Width / 2, Height / 2 - 20, _FormatMiddleCenter);
                }
                else
                {
                    pe.Graphics.DrawString(Text, Font, _BrushText1, Width / 2, Height / 2 - 40, _FormatMiddleCenter);
                    pe.Graphics.DrawString($"[ {Text2} ]", Font, _BrushText2, Width / 2, Height / 2 - 10, _FormatMiddleCenter);
                }
            }
        }
    }
}
