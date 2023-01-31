using DispelTools.DataPatcher;
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
using static DispelTools.DataPatcher.PatcherParams;

namespace View.Modals
{
    /// <summary>
    /// Interaction logic for PatcherOptions.xaml
    /// </summary>
    public partial class PatcherOptions : Window
    {
        private readonly PatcherParams patcherParams;
        public PatcherOptions(OptionNames acceptedOptions, PatcherParams patcherParams)
        {
            InitializeComponent();
            //Enable options
            KeepBackupsCheckBox.IsEnabled = acceptedOptions.HasFlag(OptionNames.KeepBackupFiles);
            KeepImageSizeCheckBox.IsEnabled = acceptedOptions.HasFlag(OptionNames.KeepImageSize);

            //Set values
            this.patcherParams = patcherParams;
            KeepBackupsCheckBox.IsChecked = patcherParams.options.KeepBackupFiles;
            KeepImageSizeCheckBox.IsChecked = patcherParams.options.KeepImageSize;
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            patcherParams.options.KeepBackupFiles = KeepBackupsCheckBox.IsChecked ?? false;
            patcherParams.options.KeepImageSize = KeepImageSizeCheckBox.IsChecked ?? false;
            Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
