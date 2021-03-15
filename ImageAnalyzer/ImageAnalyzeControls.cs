using DispelTools.ImageProcessing;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DispelTools.ImageAnalyzer
{
    public partial class ImageAnalyzeControls : UserControl
    {
        public enum ColorChannel { Any, R, G, B, A, CH1 = R, CH2 = G, CH3 = B, CH4 = A };
        public enum Analyzer { NONE, COLOR_LEVELS, VALUE_HIGHLIGHT, CHANNEL_VIEW };
        private Analyzer checkedAnalyzer;
        private bool oneChannelSelected;


        internal event EventHandler AnalyzerChangedEvent;

        public ImageAnalyzeControls()
        {
            InitializeComponent();
            valueHighlightChannelComboBox.Items.AddRange(new object[] {
                "Any",
                "CH1",
                "CH2",
                "CH3",
                "CH4"
            });
            valueHighlightChannelComboBox.SelectedIndex = 0;
        }

        private void Updated()
        {
            AnalyzerChangedEvent?.Invoke(this, EventArgs.Empty);
        }

        private void inColorLevelLow_ValueChanged(object sender, EventArgs e)
        {
            if (inColorLevelHigh.Value >= inColorLevelLow.Value)
            {
                inColorLevelHigh.Value = inColorLevelLow.Value + 1;
            }
            if (checkedAnalyzer == Analyzer.COLOR_LEVELS)
            {
                Updated();
            }
        }

        private void inColorLevelHigh_ValueChanged(object sender, EventArgs e)
        {
            if (inColorLevelLow.Value >= inColorLevelHigh.Value)
            {
                inColorLevelLow.Value = inColorLevelHigh.Value - 1;
            }
            if (checkedAnalyzer == Analyzer.COLOR_LEVELS)
            {
                Updated();
            }
        }

        private void colorLevelResetButton_Click(object sender, EventArgs e)
        {
            inColorLevelLow.Value = 0;
            inColorLevelHigh.Value = 255;
            if(checkedAnalyzer == Analyzer.COLOR_LEVELS)
            {
                Updated();
            }
        }

        private void checkedAnalyzerRadioButton(object sender, EventArgs e)
        {
            var prev = checkedAnalyzer;
            if (sender == valueHighlightRadioButton)
            {
                checkedAnalyzer = Analyzer.VALUE_HIGHLIGHT;
            }
            else if (sender == colorLevelRadioButton)
            {
                checkedAnalyzer = Analyzer.COLOR_LEVELS;
            }
            else if (sender == channelViewRadioButton)
            {
                checkedAnalyzer = Analyzer.CHANNEL_VIEW;
            }
            else
            {
                checkedAnalyzer = Analyzer.NONE;
            }
            if (prev != checkedAnalyzer)
            {
                Updated();
            }
        }

        public Color AnalyzePixel(Color pixel)
        {
            switch (checkedAnalyzer)
            {
                case Analyzer.COLOR_LEVELS:
                    return new ColorLevelFilter((byte)inColorLevelLow.Value, (byte)inColorLevelHigh.Value).Apply(pixel);
                case Analyzer.VALUE_HIGHLIGHT:
                    return IsExpectedColor((byte)inValueHighlightValue.Value, pixel, (ColorChannel)Enum.Parse(typeof(ColorChannel), valueHighlightChannelComboBox.SelectedItem.ToString()))
                        ? Color.White : Color.Black;
                case Analyzer.CHANNEL_VIEW:
                    return FilterByChannels(pixel);
                default:
                    return pixel;
            }
        }

        private static bool IsExpectedColor(byte expectedColor, Color givenColor, ColorChannel channel)
        {
            switch (channel)
            {
                case ColorChannel.Any:
                    return IsExpectedColor(expectedColor, givenColor, ColorChannel.R) ||
                           IsExpectedColor(expectedColor, givenColor, ColorChannel.G) ||
                           IsExpectedColor(expectedColor, givenColor, ColorChannel.B) ||
                           IsExpectedColor(expectedColor, givenColor, ColorChannel.A);
                case ColorChannel.R:
                    return givenColor.R == expectedColor;
                case ColorChannel.G:
                    return givenColor.G == expectedColor;
                case ColorChannel.B:
                    return givenColor.B == expectedColor;
                case ColorChannel.A:
                    return givenColor.A == expectedColor;
                default:
                    throw new ArgumentException($"Unexpected channel value {channel}");
            }
        }

        private void channelsCheckBox_Click(object sender, EventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox.Enabled)
            {
                if (ModifierKeys.HasFlag(Keys.Shift))
                {
                    foreach (object control in channelsGroupBox.Controls)
                    {
                        ((CheckBox)control).Checked = false;
                    }
                    checkBox.Checked = true;
                }
                int selectionCounter = 0;
                foreach (object control in channelsGroupBox.Controls)
                {
                    if (((CheckBox)control).Checked) { selectionCounter++; }
                }
                oneChannelSelected = selectionCounter == 1;
                if (checkedAnalyzer == Analyzer.CHANNEL_VIEW)
                {
                    Updated();
                }
            }
        }

        private Color FilterByChannels(Color pixel)
        {
            if (oneChannelSelected)
            {
                if (ch1CheckBox.Checked)
                {
                    return Color.FromArgb(pixel.R, pixel.R, pixel.R);
                }
                else if (ch2CheckBox.Checked)
                {
                    return Color.FromArgb(pixel.G, pixel.G, pixel.G);
                }
                else if (ch3CheckBox.Checked)
                {
                    return Color.FromArgb(pixel.B, pixel.B, pixel.B);
                }
                else
                {
                    return Color.FromArgb(pixel.A, pixel.A, pixel.A);
                }
            }
            else
            {
                uint rMask = 0x00ff0000;
                uint gMask = 0x0000ff00;
                uint bMask = 0x000000ff;
                uint aMask = 0xff000000;

                uint pixelValue = (uint)pixel.ToArgb();
                uint result = 0;
                if (ch1CheckBox.Checked && ch1CheckBox.Enabled)
                {
                    result += pixelValue & rMask;
                }
                if (ch2CheckBox.Checked && ch2CheckBox.Enabled)
                {
                    result += pixelValue & gMask;
                }
                if (ch3CheckBox.Checked && ch3CheckBox.Enabled)
                {
                    result += pixelValue & bMask;
                }
                if (ch4CheckBox.Enabled)
                {
                    if (ch4CheckBox.Checked)
                    {
                        result += pixelValue & aMask;
                    }
                }
                else
                {
                    result += aMask;
                }
                return Color.FromArgb((int)result);
            }
        }
    }
}
