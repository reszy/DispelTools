using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DispelTools.Components
{
    public partial class ProgressBarWithText : UserControl
    {
        private int _value = 0;
        private string text = null;
        private int lastProgressWidth = 0;

        [DefaultValue(typeof(Color), "#FF00C000")]
        public Color BarColor { get; set; } = Color.FromArgb(255, 0, 192, 0);
        [DefaultValue(null)]
        public new string Text { get => text; set { if (text != value) { text = value; Invalidate(); } } }
        [DefaultValue(1)]
        public int Maximum { get; set; } = 1;
        [DefaultValue(0)]
        public int Value
        {
            get => _value; set
            {
                if (_value != value)
                {
                    _value = value;
                    if (CalculateProgressWidth(value) != lastProgressWidth)
                    {
                        Invalidate();
                    }
                }
            }
        }
        public ProgressBarWithText()
        {
            InitializeComponent();
        }
        private int CalculateProgressWidth(int value) => (int)((double)value / Maximum * Size.Width);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int progressWidth = CalculateProgressWidth(Value);
            var barBrush = new SolidBrush(BarColor);
            e.Graphics.FillRectangle(barBrush, new Rectangle(0, 0, progressWidth, Size.Height));
            barBrush.Dispose();
            lastProgressWidth = progressWidth;

            if (text != null)
            {
                int textX = 5;
                int textY = (Size.Height / 2) - (Font.Height / 2);
                var brush = new SolidBrush(ForeColor);
                e.Graphics.DrawString(Text, Font, brush, textX, textY);
                brush.Dispose();
            }
        }
    }
}
