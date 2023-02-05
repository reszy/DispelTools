using DispelTools.Common.DataProcessing;
using DispelTools.DataPatcher;
using FileDialogs;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using View.Components;

namespace View.Views
{
    /// <summary>
    /// Interaction logic for PatcherView.xaml
    /// </summary>
    public partial class PatcherView : UserControl, INestedView
    {
        private readonly FileSystem fs;
        private readonly IPatcherFactory patcherFactory;
        private readonly PatcherManager patcherManager;
        private readonly OpenFileDialog selectDestinationFileDialog;
        private readonly OpenFileDialog selectPatchesDialog;
        private readonly BackgroundWorker backgroundWorker;

        private PatcherParams patcherParams = new();

        public PatcherView(IPatcherFactory patcherFactory)
        {
            InitializeComponent();
            this.patcherFactory = patcherFactory;
            fs = new FileSystem();
            backgroundWorker = new();
            patcherManager = new(patcherFactory, backgroundWorker);

            selectDestinationFileDialog = new OpenFileDialog(fs, Window.GetWindow(this), new OpenFileDialog.Configuration()
            {
                Multiselect = false,
                Filter = patcherFactory.OutputFileFilter
            });
            selectPatchesDialog = new OpenFileDialog(fs, Window.GetWindow(this), new OpenFileDialog.Configuration()
            {
                Multiselect= true,
                Filter = patcherFactory.PatchFileFilter
            });

            OptionsButton.IsEnabled = patcherFactory.AcceptedOptions != PatcherParams.NoOptions;

            backgroundWorker.DoWork += new DoWorkEventHandler(ApplyPatchesToFile);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(PatchingProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(PatchingCompleted);
        }

        public string ViewName => patcherFactory.PatcherName;

        private void SelectPatchesClick(object sender, RoutedEventArgs e)
        {
            selectPatchesDialog.ShowDialog(() =>
            {
                patcherParams.PatchesFilenames = selectPatchesDialog.FileNames.ToList();
                patcherParams.TargetFileName = string.Empty;
                patcherManager.SetParams(patcherParams);
                SelectionTextBox.Text = patcherManager.GetPatchMaping();
            });
            SetIfReady();
        }

        private void PatchClick(object sender, RoutedEventArgs e)
        {
            ProgressBar.Maximum = 1;
            ProgressBar.Value = 0;
            ProgressBar.Text = string.Empty;
            Details.ClearDetails();
            PatchButton.IsEnabled = false;
            OutputSelectButton.IsEnabled = false;
            InputSelectButton.IsEnabled = false;
            OptionsButton.IsEnabled = false;
            backgroundWorker.RunWorkerAsync();
        }

        private void SelectPatchedFileClick(object sender, RoutedEventArgs e)
        {
            selectDestinationFileDialog.ShowDialog(() => {
                patcherParams.TargetFileName = selectDestinationFileDialog.FileName;
                patcherManager.SetParams(patcherParams);
                SelectionTextBox.Text = patcherManager.GetPatchMaping();
            });
            SetIfReady();
        }
        private void OptionsClick(object sender, RoutedEventArgs e)
        {
            var optionsForm = new Modals.PatcherOptions(patcherFactory.AcceptedOptions, patcherParams);
            optionsForm.ShowDialog();
        }

        private void ApplyPatchesToFile(object? sender, DoWorkEventArgs e)
        {
            if (!e.Cancel)
            {
                ProgressBar.Maximum = patcherManager.FilesToPatchCount * 1000;
                patcherManager.Start();
            }
        }

        private void PatchingProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar.Value = e.ProgressPercentage;
            if (e.UserState is string text)
            {
                ProgressBar.Text = text;
            }
            if (e.UserState is SimpleDetail)
            {
                var detailsToAdd = e.UserState as SimpleDetail;
                Details.AddDetails(detailsToAdd.Details);
            }
        }

        private void PatchingCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PatchButton.IsEnabled = true;
            OutputSelectButton.IsEnabled = true;
            InputSelectButton.IsEnabled = true;
            OptionsButton.IsEnabled = true;
            ProgressBar.Text = "Completed";
        }

        private void SetIfReady()
        {
            if (patcherParams.HaveFilledRequiredParams)
            {
                PatchButton.IsEnabled = true;
            }
        }

    }
}
