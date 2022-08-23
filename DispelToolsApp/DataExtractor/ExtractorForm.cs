using DispelTools.Common;
using DispelTools.Common.DataProcessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace DispelTools.DataExtractor
{
    public partial class ExtractorForm : Form
    {
        private readonly IExtractorFactory extractorFactory;
        private List<string> filenames;
        private string outputDirectory;
        private ExtractionParams extractionParams = new ExtractionParams();

        public ExtractorForm(IExtractorFactory extractorFactory)
        {
            InitializeComponent();
            Text = extractorFactory.ExtractorName;
            this.extractorFactory = extractorFactory;
            optionsButton.Enabled = extractorFactory.AcceptedOptions != ExtractionParams.NoOptions;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            string outDirectory = null;
            if (extractorFactory.Type == ExtractionManager.ExtractorType.MULTI_FILE)
            {
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = extractorFactory.FileFilter;
                openFileDialog.ShowDialog(() =>
                {
                    filenames = new List<string>(openFileDialog.FileNames);
                    string openedDirectory = Path.GetDirectoryName(filenames[0]);
                    outDirectory = openedDirectory;
                    openFileDialog.InitialDirectory = openedDirectory;
                    selectedLabel.Text = filenames.Count == 1 ? $" {filenames.Count}   {filenames[0]}" : $" {filenames.Count}";
                });
            }
            else if (extractorFactory.Type == ExtractionManager.ExtractorType.DIRECTORY)
            {
                folderBrowserDialog.ShowNewFolderButton = false;
                folderBrowserDialog.ShowDialog(() =>
                {
                    filenames = new List<string>
                    {
                        folderBrowserDialog.SelectedPath
                    };
                    outDirectory = filenames[0];
                    selectedLabel.Text = $" {folderBrowserDialog.SelectedPath}";
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

        private void extractButton_Click(object sender, EventArgs e)
        {
            progressBar.Maximum = 1;
            progressBar.Value = 0;
            progressBar.Text = null;
            details.ClearDetails();
            extractButton.Enabled = false;
            directorySelectButton.Enabled = false;
            openButton.Enabled = false;
            openOutputDirectoryButton.Enabled = false;
            optionsButton.Enabled = false;
            backgroundWorker.RunWorkerAsync(filenames);
        }

        private void ExtractAllFiles(object sender, DoWorkEventArgs e)
        {
            if (!e.Cancel)
            {
                var worker = sender as BackgroundWorker;
                extractionParams.Filename = filenames;
                extractionParams.OutputDirectory = outputDirectory;
                var extractor = new ExtractionManager(extractorFactory.CreateInstance(), extractionParams, worker);
                progressBar.Maximum = extractor.Prepare() * 1000;
                extractor.Start();
            }
        }

        private void ExtractionProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            if (e.UserState is string text)
            {
                progressBar.Text = text;
            }
            if (e.UserState is SimpleDetail)
            {
                var detailsToAdd = e.UserState as SimpleDetail;
                details.AddDetails(detailsToAdd.Details);
            }
        }

        private void ExtractionCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            extractButton.Enabled = true;
            directorySelectButton.Enabled = true;
            openButton.Enabled = true;
            openOutputDirectoryButton.Enabled = true;
            optionsButton.Enabled = true;
            progressBar.Text = "Completed";
        }

        private void outputDirectoryButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowNewFolderButton = true;
            if (outputDirectory != null)
            {
                folderBrowserDialog.SelectedPath = outputDirectory;
            }
            folderBrowserDialog.ShowDialog();
            SetOutputDirectory(folderBrowserDialog.SelectedPath);
            SetIfReady();
        }

        private void SetIfReady()
        {
            if (outputDirectory != null)
            {
                openOutputDirectoryButton.Enabled = true;
                if (filenames != null && filenames.Count > 0)
                {
                    extractButton.Enabled = true;
                }
            }
        }

        private void SetOutputDirectory(string dir)
        {
            outputDirectory = dir;
            outputDirectoryInfo.Text = dir;
        }

        private void openOutputDirectoryButton_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = outputDirectory,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        private void optionsButton_Click(object sender, EventArgs e)
        {
            var optionsForm = new ExtractorOptionsForm(extractorFactory.AcceptedOptions, extractionParams);
            optionsForm.ShowDialog();
            extractionParams = optionsForm.Options;
        }
    }
}
