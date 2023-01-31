using DispelTools.DataExtractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static DispelTools.DataExtractor.ExtractionParams;
using static DispelTools.ImageProcessing.ColorManagement;

namespace View.Modals
{
    /// <summary>
    /// Interaction logic for ExtractorOptions.xaml
    /// </summary>
    public partial class ExtractorOptions : Window
    {
        private ExtractionParams extractionParams;
        public ExtractorOptions(OptionNames acceptedOptions, ExtractionParams extractionParams)
        {
            InitializeComponent();
            //Enable options
            ColorModeComboBox.IsEnabled = acceptedOptions.HasFlag(OptionNames.ColorMode);
            AnimatedGifCheckBox.IsEnabled = acceptedOptions.HasFlag(OptionNames.AnimatedGifs);
            BlackAsTransparentCheckBox.IsEnabled = acceptedOptions.HasFlag(OptionNames.BlackAsTransparent);

            //Set values
            this.extractionParams = extractionParams;
            ColorModeComboBox.SelectedIndex = ColorModeComboBox.Items.IndexOf(Enum.GetName(typeof(ColorMode), extractionParams.ColorMode));
            AnimatedGifCheckBox.IsChecked = extractionParams.CreateAnimatedGifs;
            BlackAsTransparentCheckBox.IsChecked = extractionParams.BlackAsTransparent;
        }
        public ExtractionParams Options { get => extractionParams; }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            extractionParams = new ExtractionParams()
            {
                ColorMode = (string)ColorModeComboBox.SelectedItem == "RGB16_555" ? ColorMode.RGB16_555 : ColorMode.RGB16_565,
                CreateAnimatedGifs = AnimatedGifCheckBox.IsChecked ?? false,
                BlackAsTransparent = BlackAsTransparentCheckBox.IsChecked ?? false
            };
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
