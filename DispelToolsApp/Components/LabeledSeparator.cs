using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispelTools.Components
{
    public partial class LabeledSeparator : UserControl
    {
        public string Label { get => label.Text; set { label.Text = value; } }
        public LabeledSeparator()
        {
            InitializeComponent();
            UpdateSeparator();
            UpdateLabelLocation();

            label.SizeChanged += LabelSizeChanged;
            SizeChanged += LabeledSeparatorSizeChanged;
        }



        private void LabeledSeparatorSizeChanged(object sender, EventArgs e)
        {
            UpdateSeparator();
            UpdateLabelLocation();
        }

        private void LabelSizeChanged(object sender, EventArgs e)
        {
            UpdateLabelLocation();
        }

        private void UpdateLabelLocation() => label.Location = new Point((Size.Width - label.Size.Width) / 2, label.Location.Y);

        private void UpdateSeparator()
        {
            separator.Location = new Point(5, Size.Height / 2);
            separator.Size = new Size(Size.Width - 10, 2);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if ((specified & BoundsSpecified.Height) == 0 || height == label.Size.Height)
            {
                base.SetBoundsCore(x, y, width, label.Size.Height, specified);
            }
            else
            {
                return; // RETURN WITHOUT DOING ANY RESIZING
            }
        }
    }
}
