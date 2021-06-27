using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DispelTools.DataExtractor.ExtractionParams;
using static DispelTools.ImageProcessing.ColorManagement;

namespace DispelTools.DataExtractor
{
    public partial class ExtractorOptionsForm : Form
    {
        private ExtractionParams extractionParams;
        public ExtractorOptionsForm(OptionNames acceptedOptions, ExtractionParams extractionParams)
        {
            InitializeComponent();

            colorModeComboBox.Enabled = acceptedOptions.HasFlag(OptionNames.ColorMode);
            animatedGifCheckBox.Enabled = acceptedOptions.HasFlag(OptionNames.AnimatedGifs);

            this.extractionParams = extractionParams;
            colorModeComboBox.SelectedIndex = colorModeComboBox.Items.IndexOf(Enum.GetName(typeof(ColorMode), extractionParams.ColorMode));
            animatedGifCheckBox.Checked = extractionParams.CreateAnimatedGifs;
        }

        public ExtractionParams Options { get => extractionParams; }

        private void okButton_Click(object sender, EventArgs e)
        {
            extractionParams = new ExtractionParams()
            {
                ColorMode = (string)colorModeComboBox.SelectedItem == "RGB16_555" ? ColorMode.RGB16_555 : ColorMode.RGB16_565,
                CreateAnimatedGifs = animatedGifCheckBox.Checked,
            };
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
