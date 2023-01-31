using DispelTools.Common;
using DispelTools.Common.DataProcessing;
using DispelTools.DataExtractor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using View.Components;
using View.Dialog;
using static System.Net.Mime.MediaTypeNames;

namespace View.Views
{
    /// <summary>
    /// Interaction logic for ExtractorView.xaml
    /// </summary>
    public partial class ExtractorView : UserControl, INestedView
    {
        private readonly BackgroundWorker backgroundWorker;

        private readonly IExtractorFactory extractorFactory;
        private List<string>? filenames;
        private string? outputDirectory;
        private ExtractionParams extractionParams = new();

        private OpenFileDialog openFileDialog;
        private FolderBrowseDialog folderBrowserDialog;
        private FolderBrowseDialog outputFolderBrowserDialog;

        private FileSystem fs = new FileSystem();
        public string ViewName { get; }

        public ExtractorView(IExtractorFactory extractorFactory)
        {
            InitializeComponent();
            ViewName = extractorFactory.ExtractorName;
            this.extractorFactory = extractorFactory;
            OptionsButton.IsEnabled = extractorFactory.AcceptedOptions != ExtractionParams.NoOptions;
            folderBrowserDialog = new(fs, Window.GetWindow(this), new FolderBrowseDialog.Configuration()
            {
                ShowNewFolderButton = false
            });

            openFileDialog = new(fs, Window.GetWindow(this), new OpenFileDialog.Configuration()
            {
                Multiselect = extractorFactory.Type == ExtractionManager.ExtractorType.MULTI_FILE,
                Filter = extractorFactory.FileFilter,
            });

            outputFolderBrowserDialog = new(fs, Window.GetWindow(this), new FolderBrowseDialog.Configuration()
            {
                ShowNewFolderButton = true
            });

            backgroundWorker = new();
            backgroundWorker.DoWork += new DoWorkEventHandler(ExtractAllFiles);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(ExtractionProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ExtractionCompleted);
        }
        private void OpenClick(object sender, RoutedEventArgs e)
        {
            string? outDirectory = null;
            if (extractorFactory.Type == ExtractionManager.ExtractorType.MULTI_FILE)
            {
                openFileDialog.ShowDialog(() =>
                {
                    filenames = new List<string>(openFileDialog.FileNames);
                    string openedDirectory = fs.Path.GetDirectoryName(filenames[0]) ?? string.Empty;
                    outDirectory = openedDirectory;
                    SelectedInfo.Text = filenames.Count == 1 ? $" {filenames.Count}   {filenames[0]}" : $" {filenames.Count}";
                });
            }
            else if (extractorFactory.Type == ExtractionManager.ExtractorType.DIRECTORY)
            {
                folderBrowserDialog.ShowDialog(() =>
                {
                    filenames = new List<string>
                    {
                        folderBrowserDialog.DirectoryPath
                    };
                    outDirectory = filenames[0];
                    SelectedInfo.Text = $" {folderBrowserDialog.DirectoryPath}";
                });
            }
            if (outDirectory != null)
            {
                if (Settings.RootsValid)
                {
                    SetOutputDirectory(Settings.TranslateToOutDir(outDirectory));
                }
                else
                {
                    SetOutputDirectory(outDirectory);
                }
            }
            SetIfReady();
        }
        private void ExtractClick(object sender, RoutedEventArgs e)
        {
            ProgressBar.Maximum = 1;
            ProgressBar.Value = 0;
            ProgressBar.Text = string.Empty;
            Details.ClearDetails();
            ExtractButton.IsEnabled = false;
            SelectDirectoryButton.IsEnabled = false;
            OpenButton.IsEnabled = false;
            OpenSlectedDirectoryButton.IsEnabled = false;
            OptionsButton.IsEnabled = false;
            backgroundWorker.RunWorkerAsync(filenames);
        }

        private void DirectorySelectClick(object sender, RoutedEventArgs e)
        {
            if (outputDirectory != null)
            {
                outputFolderBrowserDialog.DirectoryPath = outputDirectory;
            }
            folderBrowserDialog.ShowDialog(() =>
            {
                SetOutputDirectory(folderBrowserDialog.DirectoryPath);
                SetIfReady();
            });
        }

        private void OpenSelectedDirectoryClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = outputDirectory,
                UseShellExecute = true,
                Verb = "open"
            });
        }
        private void OptionsClick(object sender, RoutedEventArgs e)
        {
            var optionsForm = new Modals.ExtractorOptions(extractorFactory.AcceptedOptions, extractionParams);
            optionsForm.ShowDialog();
            extractionParams = optionsForm.Options;
        }
        private void ExtractAllFiles(object? sender, DoWorkEventArgs e)
        {
            if (!e.Cancel && filenames is not null && outputDirectory is not null && sender is BackgroundWorker worker)
            {
                extractionParams.Filename = filenames;
                extractionParams.OutputDirectory = outputDirectory;
                var extractor = new ExtractionManager(extractorFactory.CreateInstance(), extractionParams, worker);
                int fileCount = extractor.Prepare();
                if (fileCount > 0)
                {
                    ProgressBar.Maximum = fileCount * 1000;
                    extractor.Start();
                }
            }
        }

        private void ExtractionProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
            if (e.UserState is string text)
            {
                ProgressBar.Text = text;
            }
            if (e.UserState is SimpleDetail detailsToAdd)
            {
                Details.AddDetails(detailsToAdd.Details);
            }
        }

        private void ExtractionCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            ExtractButton.IsEnabled = true;
            SelectDirectoryButton.IsEnabled = true;
            OpenButton.IsEnabled = true;
            OpenSlectedDirectoryButton.IsEnabled = true;
            OptionsButton.IsEnabled = true;
            ProgressBar.Text = "Completed";
        }
        private void SetIfReady()
        {
            if (outputDirectory != null)
            {
                OpenSlectedDirectoryButton.IsEnabled = true;
                if (filenames != null && filenames.Count > 0)
                {
                    ExtractButton.IsEnabled = true;
                }
            }
        }

        private void SetOutputDirectory(string dir)
        {
            outputDirectory = dir;
            OutputDirectoryInfo.Text = dir;
        }
    }
}
