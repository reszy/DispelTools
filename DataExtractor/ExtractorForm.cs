using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
            if (extractorFactory.type == ExtractionManager.ExtractorType.MULTI_FILE)
            {
                var dialog = new OpenFileDialog()
                {
                    Multiselect = true,
                    Filter = extractorFactory.FileFilter
                };
                dialog.ShowDialog();
                filenames = new List<string>(dialog.FileNames);
                if (filenames.Count != 0)
                {
                    setOutputDirectory(Path.GetDirectoryName(filenames[0]));
                    selectedLabel.Text = $" {filenames.Count}";
                }
            }
            else if (extractorFactory.type == ExtractionManager.ExtractorType.DIRECTORY)
            {
                var dialog = new FolderBrowserDialog()
                {
                    ShowNewFolderButton = true
                };
                dialog.ShowDialog();
                filenames = new List<string>
                {
                    dialog.SelectedPath
                };
                selectedLabel.Text = $" {dialog.SelectedPath}";
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

        private void ExtractionCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            extractButton.Enabled = true;
        }

        private void outputDirectoryButton_Click(object sender, EventArgs e)
        {
            var openDialog = new FolderBrowserDialog();
            if (outputDirectory != null)
            {
                openDialog.SelectedPath = outputDirectory;
            }
            openDialog.ShowDialog();
            setOutputDirectory(openDialog.SelectedPath);
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
