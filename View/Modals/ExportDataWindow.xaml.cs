using DispelTools.DataEditor;
using DispelTools.DataEditor.Export;
using System;
using System.IO;
using System.IO.Abstractions;
using System.Windows;
using View.ViewModels;

namespace View.Modals
{
    /// <summary>
    /// Interaction logic for ExportDataWindow.xaml
    /// </summary>
    public partial class ExportDataWindow : Window
    {
        private readonly SimpleDataContainer container;
        private readonly int selectedProperty;
        private readonly FileSystem fs = new();
        Exporter? exporter = null;

        private readonly FileDialogs.SaveFileDialog destinationDialog;
        private string savePath = string.Empty;
        private readonly AsyncWorkHandler workHandler;

        public ExportDataWindow(SimpleDataContainer container, int selectedProperty)
        {
            InitializeComponent();
#if DEBUG
            ProjDocsExport.Visibility = Visibility.Visible;
            ProjDocsExport.IsEnabled = true;
            this.container = container;
            this.selectedProperty = selectedProperty;
#else
            ProjDocsExport.Visibility = Visibility.Collapse;
            ProjDocsExport.IsEnabled = false;
#endif

            destinationDialog = new(fs, this, new FileDialogs.SaveFileDialog.Configuration());

            workHandler = new()
            {
                ProgressBar = ProgressBar
            };
            workHandler.DisableWhileWorking(
                ExportButton,
                ProjDocsExport,
                DestinationTextBox,
                DestinationButton,
                ExportAllRadio,
                ExportSelectedRadio,
                ExportDataRadio,
                ExportSchemeRadio
                );
            workHandler.AddStage(Export);
            ExportTypeClick(this, new RoutedEventArgs());
        }

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

        private void ExportAmountClick(object sender, RoutedEventArgs e)
        {
            workHandler.Resetprogress();
        }

        private void ExportTypeClick(object sender, RoutedEventArgs e)
        {
            workHandler.Resetprogress();
            if (ExportSchemeRadio.IsChecked ?? false)
            {
                ExportAllRadio.IsEnabled = false;
                ExportSelectedRadio.IsEnabled = false;
                destinationDialog.Filter = "Text File (*.txt)|*.txt";
            }
            else
            {
                ExportAllRadio.IsEnabled = true;
                ExportSelectedRadio.IsEnabled = true;
                destinationDialog.Filter = "Json file (*.json)|*.json";
            }
            DestinationTextBox.Text =string.Empty;
        }
        private void SelectFileClick(object sender, RoutedEventArgs e)
        {
            destinationDialog.FileName = GetDefaultFilename().Replace('.', '_');
            destinationDialog.ShowDialog(() =>
            {
                DestinationTextBox.Text = destinationDialog.FileName;
            });
            workHandler.Resetprogress();
        }

        private string GetDefaultFilename()
        {
            if (ExportSchemeRadio.IsChecked ?? false)
            {
               return ExportAsScheme.GetDefaultFilename(container);
            }
            if (ExportDataRadio.IsChecked ?? false)
            {
                if (ExportAllRadio.IsChecked ?? false)
                {
                    return container.Filename;
                }
                else
                {
                    return container.Filename + selectedProperty;
                }
            }
            return string.Empty;
        }
        private void ExportClick(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(Path.GetDirectoryName(DestinationTextBox.Text))) return;
            if (ExportSchemeRadio.IsChecked ?? false)
            {
                exporter = new ExportAsScheme(fs, workHandler.WorkReporter, container);
            }
            if (ExportDataRadio.IsChecked ?? false)
            {
                exporter = new ExportAsData(fs, workHandler.WorkReporter, container)
                {
                    DataItemIndex = selectedProperty,
                    AllProperties = ExportAllRadio.IsChecked ?? false
                };
            }
            if (exporter is not null)
            {
                savePath = DestinationTextBox.Text;
                workHandler.Start();
            }
            workHandler.Resetprogress();
        }

        private void Export()
        {
            exporter!.Export(savePath);
        }
    }
}
