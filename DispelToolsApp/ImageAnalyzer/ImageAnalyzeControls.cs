using DispelTools.ImageProcessing.Filters;
using System;
using System.Windows.Forms;
using static DispelTools.ImageProcessing.Filters.ValueFilter;

namespace DispelTools.ImageAnalyzer
{
    public partial class ImageAnalyzeControls : UserControl
    {
        public enum Analyzer { NONE, COLOR_LEVELS, VALUE_HIGHLIGHT, CHANNEL_VIEW };
        private Analyzer checkedAnalyzer;
        public Analyzer CurrentAnalyzer => checkedAnalyzer;

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

        public IPerPixelFilter ActiveFilter
        {
            get
            {
                switch (checkedAnalyzer)
                {
                    case Analyzer.COLOR_LEVELS:
                        return new ColorLevelFilter((byte)inColorLevelLow.Value, (byte)inColorLevelHigh.Value);
                    case Analyzer.VALUE_HIGHLIGHT:
                        return new ValueFilter((byte)inValueHighlightValue.Value, (ColorChannel)Enum.Parse(typeof(ColorChannel), valueHighlightChannelComboBox.SelectedItem.ToString()));
                    case Analyzer.CHANNEL_VIEW:
                        return new ChannelFilter(ch1CheckBox.Checked && ch1CheckBox.Enabled, ch2CheckBox.Checked && ch2CheckBox.Enabled, ch3CheckBox.Checked && ch3CheckBox.Enabled, ch4CheckBox.Checked && ch4CheckBox.Enabled); ;
                    default:
                        return null;
                }
            }
        }

        private void Updated() => AnalyzerChangedEvent?.Invoke(this, EventArgs.Empty);

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
            if (checkedAnalyzer == Analyzer.COLOR_LEVELS)
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
                if (checkedAnalyzer == Analyzer.CHANNEL_VIEW)
                {
                    Updated();
                }
            }
        }

        private void valueFilterChanged(object sender, EventArgs e)
        {
            Updated();
        }
    }
}
