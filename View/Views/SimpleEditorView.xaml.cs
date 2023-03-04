using DispelTools.Common.DataProcessing;
using DispelTools.DataEditor.Data;
using FileDialogs;
using System;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using View.Components;
using View.Exceptions;
using View.Modals;
using View.ViewModels;

namespace View.Views
{
    /// <summary>
    /// Interaction logic for SimpleEditorView.xaml
    /// </summary>
    public partial class SimpleEditorView : UserControl, INestedView
    {
        private SimpleEditor? editor;
        private DataItem? SelectedItem;

        private readonly OpenFileDialog openFileDialog;
        private readonly BackgroundWorker loadFileWorker;

        public string ViewName => "Simple data editor";

        public SimpleEditorView()
        {
            InitializeComponent();

            openFileDialog = new(new FileSystem(), Window.GetWindow(this), new OpenFileDialog.Configuration());
            DataItem.Sample()
                .Select(x => new DataRow(x))
                .ToList()
                .ForEach(x => DataPanel.Children.Add(x));
            InElementNumber.ValueChanged += ElementNumberValueChanged;
            loadFileWorker = new();
            loadFileWorker.DoWork += Read;
            loadFileWorker.RunWorkerCompleted += ReadingFileCompleted;
        }

        private void OpenButtonClick(object sender, EventArgs e)
        {

            openFileDialog.ShowDialog(() =>
            {
                editor = new SimpleEditor(new FileSystem());
                loadFileWorker.RunWorkerAsync();
            });
        }

        private void ElementNumberValueChanged(object sender, RoutedEventArgs e)
        {
            if (editor is not null)
            {
                try
                {
                    SelectedItem = editor.GetValue((int)InElementNumber.Value);
                }
                catch (MessageException ex)
                {
                    MessageBox.Show(ex.Message, ex.Header, MessageBoxButton.OK, ex.GetMessageBoxIcon());
                }
            }
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                editor?.Save(SelectedItem!, (int)InElementNumber.Value);
            }
            catch (MessageException ex)
            {
                MessageBox.Show(ex.Message, ex.Header, MessageBoxButton.OK, ex.GetMessageBoxIcon());
            }
        }

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
                var workReporter = new WorkReporter(loadFileWorker)
                {
                    DecideOnWarning = WorkPaused
                };
                try
                {
                    editor.Load(openFileDialog.FileName, workReporter);
                }
                catch (MessageException e)
                {
                    MessageBox.Show(e.Message, e.Header, MessageBoxButton.OK, e.GetMessageBoxIcon());
                }
            }
        }

        private static bool WorkPaused(WorkReporter.WorkerWarning warning)
        {
            var result = MessageBox.Show(warning.Message + "\n Ignore and continue?", "Reading warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
            return result == MessageBoxResult.Yes;
        }

        private void ReadingFileCompleted(object? sender, RunWorkerCompletedEventArgs args)
        {
            InElementNumber.Value = 0;
            OpenedFileLabel.Content = editor.GetLoadedContainer().Path;
            SelectedItem = editor.GetValue(0);
            if (SelectedItem is not null)
            {
                DataPanel.Children.Clear();
                SelectedItem.Select(x => new DataRow(x))
                .ToList()
                .ForEach(x => DataPanel.Children.Add(x));
            }
            SetMaxElementsLabel(InElementNumber.Maximum = (editor?.GetElementCount() ?? 1) - 1);
        }

        private void ExportClick(object? sender, RoutedEventArgs args)
        {
            if (editor is not null)
            {
                var exportWindow = new ExportDataWindow(editor.GetLoadedContainer(), (int)InElementNumber.Value)
                {
                    Owner = Application.Current.MainWindow
                };
                exportWindow.ShowDialog();
            }
        }
    }
}
