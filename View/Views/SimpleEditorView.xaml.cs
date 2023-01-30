using DispelTools.DataEditor;
using DispelTools.DataEditor.Data;
using System;
using System.IO.Abstractions;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using View.Components;
using View.Dialog;
using static DispelTools.ImageAnalyzer.DataAnalyzedBitmap;

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
        public SimpleEditorView()
        {
            InitializeComponent();

            openFileDialog = new(new FileSystem(), Window.GetWindow(this), new OpenFileDialog.Configuration());
            PropertyItem.Sample()
                .Select(x => new DataRow(x))
                .ToList()
                .ForEach(x => DataPanel.Children.Add(x));
            InElementNumber.ValueChanged += ElementNumberValueChanged;
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
                    Read();
                }
                else
                {
                    MessageBox.Show(editor.ValidationMessage, "File unsupported", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void ElementNumberValueChanged(object sender, RoutedEventArgs e) => Read();

        private void SaveButtonClick(object sender, RoutedEventArgs e) => editor?.Save(SelectedItem!, (int)InElementNumber.Value);

        private void HideUnnamedIsCheckedChanged(object sender, EventArgs e)
        {
            var hide  = HideUnnamedCheckBox.IsChecked ?? false;
            foreach (var child in DataPanel.Children) if (child is DataRow row) row.HideIfUnnamedField(hide);
        }
        private void SetMaxElementsLabel(decimal number) => MaxElementsLabel.Content = "/ " + number;

        private void Read()
        {
            if (editor != null)
            {
                SelectedItem = editor.ReadValue((int)InElementNumber.Value);

                DataPanel.Children.Clear();
                SelectedItem.Select(x => new DataRow(x))
                .ToList()
                .ForEach(x => DataPanel.Children.Add(x));
            }
        }

    }
}
