using DispelTools.DataEditor;
using DispelTools.DataEditor.Export;
using System;
using System.IO.Abstractions;
using System.Windows;

namespace View.Modals
{
    /// <summary>
    /// Interaction logic for ExportDataWindow.xaml
    /// </summary>
    public partial class ExportDataWindow : Window
    {
        public ExportDataWindow()
        {
            InitializeComponent();
#if DEBUG
            ProjDocsExport.Visibility = Visibility.Visible;
            ProjDocsExport.IsEnabled = true;
#else
            ProjDocsExport.Visibility = Visibility.Collapse;
            ProjDocsExport.IsEnabled = false;
#endif
        }

        public SimpleEditor? Editor { get; init; }

        private void CloseClick(object sender, RoutedEventArgs e) => Close();
        private void ExportDocsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var exporter = new ExportAsDocScheme(new FileSystem());
                exporter.PrepareDirectoryAndMappers();
                exporter.Export();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while exporting\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
