using DispelTools.ImageProcessing;
using System;
using System.Windows.Forms;

namespace DispelTools.ImageAnalyzer
{
    internal partial class ImageAlignControls : UserControl
    {

        private readonly Options options = new Options();
        public Options ImageOptions { get { LoadOptions(); return options; } }

        public ImageAlignControls()
        {
            InitializeComponent();
            cbColorMode.DataSource = Enum.GetValues(typeof(ColorManagement.ColorMode));
            cbTransparency.DataSource = Enum.GetValues(typeof(Options.Transparency));


            inOffset.Value= 5000700;
            inWidth.Value =499;
            inHeight.Value=499;
            inLineLen.Value=499;
            inImageNumber.Value=1;
            inImageOffset.Value=1;

            cbColorMode.SelectedItem = ColorManagement.ColorMode.DATA32;
            cbTransparency.SelectedItem = Options.Transparency.NONE;
        }

        internal class Options
        {
            public int offset { get; internal set; }
            public int width { get; internal set; }
            public int height { get; internal set; }
            public int lineLen { get; internal set; }
            public int imageNumber { get; internal set; }
            public int imageOffset { get; internal set; }
            public ColorManagement.ColorMode colorMode { get; internal set; }
            public Transparency transparency { get; internal set; }

            public enum Transparency { NONE, COLOR_KEY_BLACK, ALPHA };
        }

        private void LoadOptions()
        {
            options.offset = decimal.ToInt32(inOffset.Value);
            options.width = decimal.ToInt32(inWidth.Value);
            options.height = decimal.ToInt32(inHeight.Value);
            options.lineLen = decimal.ToInt32(inLineLen.Value);
            options.imageNumber = decimal.ToInt32(inImageNumber.Value);
            options.imageOffset = decimal.ToInt32(inImageOffset.Value);

            options.colorMode = (ColorManagement.ColorMode)Enum.Parse(typeof(ColorManagement.ColorMode), cbColorMode.SelectedValue.ToString());
            options.transparency = (Options.Transparency)Enum.Parse(typeof(Options.Transparency), cbTransparency.SelectedValue.ToString());
        }

        internal event EventHandler<Options> OptionsChangedEvent;

        private void SendOptionsChangedEvent()
        {
            if(autoAdjust.Checked)
            {
                LoadOptions();
                OptionsChangedEvent?.Invoke(this, options);
            }
        }

        private void inWidth_ValueChanged(object sender, EventArgs e)
        {
            if (inWidth.Value > inLineLen.Value)
            {
                inLineLen.Value = inWidth.Value;
            }
            SendOptionsChangedEvent();
        }

        private void inLineLen_ValueChanged(object sender, EventArgs e)
        {
            if (inLineLen.Value < inWidth.Value)
            {
                inWidth.Value = inLineLen.Value;
            }
            SendOptionsChangedEvent();
        }

        private void inOffset_ValueChanged(object sender, EventArgs e)
        {
            SendOptionsChangedEvent();
        }

        private void inHeight_ValueChanged(object sender, EventArgs e)
        {
            SendOptionsChangedEvent();
        }

        private void inImageOffset_ValueChanged(object sender, EventArgs e)
        {
            SendOptionsChangedEvent();
        }

        private void inImageNumber_ValueChanged(object sender, EventArgs e)
        {
            SendOptionsChangedEvent();
        }


        private void skipBackButton_Click(object sender, EventArgs e)
        {
            int imageByteSize = options.height * options.lineLen * options.imageNumber * ColorManagement.From(options.colorMode).BytesConsumed;
            if (inOffset.Value - imageByteSize > 0)
            {
                inOffset.Value -= imageByteSize;
            }
            else
            {
                inOffset.Value = 0;
            }
            SendOptionsChangedEvent();
        }

        private void skipForwButton_Click(object sender, EventArgs e)
        {
            int imageByteSize = options.height * options.lineLen * options.imageNumber * ColorManagement.From(options.colorMode).BytesConsumed;
            inOffset.Value += imageByteSize;
            SendOptionsChangedEvent();
        }

        private void cbColorMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            options.colorMode = (ColorManagement.ColorMode)Enum.Parse(typeof(ColorManagement.ColorMode), cbColorMode.SelectedValue.ToString());
            SendOptionsChangedEvent();
        }

        private void cbTransparency_SelectedIndexChanged(object sender, EventArgs e)
        {
            options.transparency = (Options.Transparency)Enum.Parse(typeof(Options.Transparency), cbTransparency.SelectedValue.ToString());
            SendOptionsChangedEvent();
        }
    }
}
