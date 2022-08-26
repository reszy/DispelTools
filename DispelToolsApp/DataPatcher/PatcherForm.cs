using DispelTools.Common;
using DispelTools.Common.DataProcessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DispelTools.DataPatcher
{
    public partial class PatcherForm : Form
    {
        private readonly PatcherManager patcherManager;
        private readonly IPatcherFactory patcherFactory;
        private PatcherParams patcherParams = new PatcherParams();

        public PatcherForm(IPatcherFactory patcherFactory)
        {
            InitializeComponent();
            Text = patcherFactory.PatcherName;
            this.patcherManager = new PatcherManager(patcherFactory, backgroundWorker);
            optionsButton.Enabled = patcherFactory.AcceptedOptions != PatcherParams.NoOptions;
            this.patcherFactory = patcherFactory;
        }

        private void selectPatchesButton_Click(object sender, EventArgs e)
        {
            selectPatchesDialog.Multiselect = true;
            selectPatchesDialog.Filter = patcherFactory.PatchFileFilter;
            selectPatchesDialog.InitialDirectory = Settings.OutRootDir;
            selectPatchesDialog.ShowDialog(() =>
            {
                patcherParams.PatchesFilenames = selectPatchesDialog.FileNames.ToList();
                patcherManager.SetParams(patcherParams);
                selectionTextBox.Text = patcherManager.GetPatchMaping();
            });
            SetIfReady();
        }

        private void patchButton_Click(object sender, EventArgs e)
        {
            progressBar.Maximum = 1;
            progressBar.Value = 0;
            progressBar.Text = null;
            details.ClearDetails();
            patchButton.Enabled = false;
            outputSelectButton.Enabled = false;
            inputSelectButton.Enabled = false;
            optionsButton.Enabled = false;
            backgroundWorker.RunWorkerAsync();
        }

        private void ApplyPatchesToFile(object sender, DoWorkEventArgs e)
        {
            if (!e.Cancel)
            {
                progressBar.Maximum = patcherManager.FilesToPatchCount * 1000;
                patcherManager.Start();
            }
        }

        private void PatchingProgressChanged(object sender, ProgressChangedEventArgs e)
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

        private void PatchingCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            patchButton.Enabled = true;
            outputSelectButton.Enabled = true;
            inputSelectButton.Enabled = true;
            optionsButton.Enabled = true;
            progressBar.Text = "Completed";
        }

        private void selectPatchedFileButton_Click(object sender, EventArgs e)
        {
            selectDestinationFileDialog.Multiselect = false;
            selectDestinationFileDialog.Filter = patcherFactory.OutputFileFilter;
            selectPatchesDialog.InitialDirectory = Settings.GameRootDir;
            selectDestinationFileDialog.ShowDialog(() => {
                patcherParams.TargetFileName = selectPatchesDialog.FileName;
                selectionTextBox.Text = patcherManager.GetPatchMaping();
            });
            SetIfReady();
        }

        private void SetIfReady()
        {
            if (patcherParams.HaveFilledRequiredParams)
            {
                patchButton.Enabled = true;
            }
        }

        private void optionsButton_Click(object sender, EventArgs e)
        {
            var optionsForm = new PatcherOptionsForm(patcherFactory.AcceptedOptions, patcherParams);
            optionsForm.ShowDialog();
            optionsForm.Dispose();
        }
    }
}
