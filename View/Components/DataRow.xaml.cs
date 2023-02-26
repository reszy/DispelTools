using DispelTools.DataEditor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace View.Components
{
    /// <summary>
    /// Interaction logic for DataRow.xaml
    /// </summary>
    public partial class DataRow : UserControl
    {
        private readonly IField field;
        private readonly NotifyTextBox? textControl;
        private readonly NumericUpDown? numericControl;
        public DataRow(IField field)
        {
            this.field = field;
            InitializeComponent();

            Label.Text = field.Name;
            Label.ToolTip = field.Description is null ? field.Name : field.Name + "\n" + field.Description;

            if (field.ReadOnly)
            {
                ResetButton.Visibility = Visibility.Collapsed;
            }

            if (field.IsText)
            {
                textControl = new()
                {
                    Text = field.GetDecodedText(),
                    IsEnabled = !field.ReadOnly
                };
                textControl.ValueChanged += TextValueChanged;
                ValueBox.Content = textControl;
            }
            else if(field is IPrimitiveField pField)
            {
                numericControl = new()
                {
                    Maximum = pField.MaxValue,
                    Minimum = pField.MinValue,
                    Value = (int)field.Value,
                    IsEnabled = !field.ReadOnly,
                    Hexadecimal = field.DisplayType == Field.DisplayType.HEX
                };
                numericControl.ValueChanged += NumberValueChanged;
                ValueBox.Content = numericControl;
            }
        }

        public void HideIfUnnamedField(bool hideIf) => Visibility = (hideIf && field.Name.StartsWith("?")) ? Visibility.Collapsed : Visibility.Visible;
        private string GetResetButtonText() => field.HasChanged ? "●" : "○";

        private void TextValueChanged(object sender, RoutedEventArgs e)
        {
            field.Value = textControl.Text;
            textControl.FontWeight = field.HasChanged ? FontWeights.Bold : FontWeights.Normal;
            ResetButton.Content = GetResetButtonText();
        }

        private void NumberValueChanged(object sender, RoutedEventArgs e)
        {
            field.Value = numericControl.Value;
            numericControl.FontWeight = field.HasChanged ? FontWeights.Bold : FontWeights.Normal;
            ResetButton.Content = GetResetButtonText();
        }

        private void ResetValue(object sender, RoutedEventArgs e)
        {
            if (!field.ReadOnly)
            {
                field.RevertValue();
                if (numericControl != null)
                {
                    numericControl.Value = (int)field.Value;
                    numericControl.FontWeight = field.HasChanged ? FontWeights.Bold : FontWeights.Normal;
                }
                if (textControl != null)
                {
                    textControl.Text = field.GetDecodedText();
                    textControl.FontWeight = field.HasChanged ? FontWeights.Bold : FontWeights.Normal;
                }
                ResetButton.Content = GetResetButtonText();
            }
        }
    }
}
