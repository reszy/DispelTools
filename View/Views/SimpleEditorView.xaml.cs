using DispelTools.Common.DataProcessing;
using DispelTools.DataEditor;
using DispelTools.DataEditor.Data;
using FileDialogs;
using System;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using View.Components;
using View.Modals;

namespace View.Views
{
    /// <summary>
    /// Interaction logic for SimpleEditorView.xaml
    /// </summary>
    public partial class SimpleEditorView : UserControl, INestedView
    {
        private SimpleEditor? editor;
        private PropertyItem? SelectedItem;

        private readonly OpenFileDialog openFileDialog;
        private readonly BackgroundWorker backgroundWorker;

        public string ViewName => "Simple data editor";

        public SimpleEditorView()
        {
            InitializeComponent();

            openFileDialog = new(new FileSystem(), Window.GetWindow(this), new OpenFileDialog.Configuration());
            PropertyItem.Sample()
                .Select(x => new DataRow(x))
                .ToList()
                .ForEach(x => DataPanel.Children.Add(x));
            InElementNumber.ValueChanged += ElementNumberValueChanged;
            backgroundWorker = new();
            backgroundWorker.DoWork += Read;
            backgroundWorker.RunWorkerCompleted += ReadingFileCompleted;
        }

        private void OpenButtonClick(object sender, EventArgs e)
        {

            openFileDialog.ShowDialog(() =>
            {
                editor = new SimpleEditor(openFileDialog.FileName);
                if (editor.CanOpen())
                {
                    InElementNumber.Value = 0;

                    SetMaxElementsLabel(InElementNumber.Maximum = editor.GetElementCount());
                    OpenedFileLabel.Content = openFileDialog.FileName;
                    backgroundWorker.RunWorkerAsync();
                }
                else
                {
                    MessageBox.Show(editor.ValidationMessage, "File unsupported", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void ElementNumberValueChanged(object sender, RoutedEventArgs e) => backgroundWorker.RunWorkerAsync();

        private void SaveButtonClick(object sender, RoutedEventArgs e) => editor?.Save(SelectedItem!, (int)InElementNumber.Value);

        private void HideUnnamedIsCheckedChanged(object sender, EventArgs e)
        {
            var hide  = HideUnnamedCheckBox.IsChecked ?? false;
            foreach (var child in DataPanel.Children) if (child is DataRow row) row.HideIfUnnamedField(hide);
        }
        private void SetMaxElementsLabel(decimal number) => MaxElementsLabel.Content = "/ " + number;

        private void Read(object? sender, DoWorkEventArgs args)
        {
            if (editor != null)
            {
                var workReporter = new WorkReporter(backgroundWorker);
                workReporter.DecideOnWarning = WorkPaused;
                SelectedItem = editor.ReadValue((int)InElementNumber.Value, workReporter);
            }
        }

        private static bool WorkPaused(WorkReporter.WorkerWarning warning)
        {
            var result = MessageBox.Show(warning.Message + "\n Ignore and continue?", "Reading warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            return result == MessageBoxResult.Yes;
        }

        private void ReadingFileCompleted(object? sender, RunWorkerCompletedEventArgs args)
        {
            if (SelectedItem is not null)
            {
                DataPanel.Children.Clear();
                SelectedItem.Select(x => new DataRow(x))
                .ToList()
                .ForEach(x => DataPanel.Children.Add(x));
            }
        }

        private void ExportClick(object? sender, RoutedEventArgs args)
        {
            var exportWindow = new ExportDataWindow()
            {
                Editor = editor
            };
            exportWindow.ShowDialog();
        }
    }
}
