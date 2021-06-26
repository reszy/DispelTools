using DispelTools.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace DispelTools.DataExtractor
{
    public partial class ExtractorForm : Form
    {
        private IExtractorFactory extractorFactory;
        private List<string> filenames;
        private string outputDirectory;
        private BackgroundWorker backgroundWorker;

        public ExtractorForm(IExtractorFactory extractorFactory)
        {
            InitializeComponent();
            Text = extractorFactory.ExtractorName;
            this.extractorFactory = extractorFactory;
            backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += ExtractAllFiles;
            backgroundWorker.RunWorkerCompleted += ExtractionCompleted;
            backgroundWorker.ProgressChanged += ExtractionProgressChanged;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            string outDirectory = null;
            if (extractorFactory.type == ExtractionManager.ExtractorType.MULTI_FILE)
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
            else if (extractorFactory.type == ExtractionManager.ExtractorType.DIRECTORY)
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
                    setOutputDirectory(Settings.TranslateToOutDir(outDirectory));
                }
                else
                {
                    setOutputDirectory(outDirectory);
                }
            }
            setIfReady();
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            progressBar.Maximum = 1;
            progressBar.Value = 0;
            progressBar.Text = null;
            details.ClearDetails();
            extractButton.Enabled = false;
            backgroundWorker.RunWorkerAsync(filenames);
        }

        private void ExtractAllFiles(object sender, DoWorkEventArgs e)
        {
            if (!e.Cancel)
            {
                var worker = sender as BackgroundWorker;
                var extractor = extractorFactory.CreateExtractorInstance(filenames, outputDirectory, worker);
                extractor.Start();
            }
        }

        private void ExtractionProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is ExtractionStatus.ProgressChanged)
            {
                var extractionStatus = e.UserState as ExtractionStatus.ProgressChanged;
                if (progressBar.Maximum != extractionStatus.ProgressTotal)
                {
                    progressBar.Maximum = extractionStatus.ProgressTotal;
                }
                if (extractionStatus.Action != null)
                {
                    details.AddDetails(extractionStatus.Action);
                }
                progressBar.Value = extractionStatus.CurrentProgress;
            }
            if (e.UserState is ExtractionStatus.StatusNameChanged)
            {
                var newStatus = e.UserState as ExtractionStatus.StatusNameChanged;
                if (progressBar.Text != newStatus.ExtraStatusName)
                {
                    progressBar.Text = newStatus.ExtraStatusName;
                }
            }
            if (e.UserState is ExtractionStatus.SimpleDetail)
            {
                var detailsToAdd = e.UserState as ExtractionStatus.SimpleDetail;
                details.AddDetails(detailsToAdd.Details);
            }
        }

        private void ExtractionCompleted(object sender, RunWorkerCompletedEventArgs e) => extractButton.Enabled = true;

        private void outputDirectoryButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.ShowNewFolderButton = true;
            if (outputDirectory != null)
            {
                folderBrowserDialog.SelectedPath = outputDirectory;
            }
            folderBrowserDialog.ShowDialog();
            setOutputDirectory(folderBrowserDialog.SelectedPath);
            setIfReady();
        }

        private void setIfReady()
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

        private void setOutputDirectory(string dir)
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
    }
}
