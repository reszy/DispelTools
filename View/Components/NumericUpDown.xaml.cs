using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace View.Components
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        private decimal maximum = int.MaxValue;
        private decimal minimum = int.MinValue;
        private decimal value = 0;
        private bool hexadecimal = false;
        private string strValue = string.Empty;

        public string StrValue
        {
            get => strValue;
            set
            {
                strValue = CorrectValue(value);
                ValueChanged?.Invoke(this, new RoutedEventArgs());
            }
        }

        public decimal Maximum
        {
            get => maximum;
            set
            {
                if (value < minimum) throw new ArgumentException("Maximum cannot be less than minimum");
                maximum = value;
                if (this.value > maximum)
                {
                    this.value = maximum;
                    UpdateShownValue();
                }
            }
        }
        public decimal Minimum
        {
            get => minimum;
            set
            {
                if (value > maximum) throw new ArgumentException("Minimum cannot be greater than maximum");
                minimum = value;
                if (this.value < minimum)
                {
                    this.value = minimum;
                    UpdateShownValue();
                }
            }
        }
        public decimal Value
        {
            get => value;
            set
            {
                if (this.value != value)
                {
                    this.value = Math.Clamp(value, minimum, maximum);
                    UpdateShownValue();
                }
            }
        }
        public bool Hexadecimal
        {
            get => hexadecimal;
            set
            {
                if (hexadecimal == value) return;
                hexadecimal = value;
                if (hexadecimal)
                {
                    HexLabel.Visibility = Visibility.Visible;
                    TextBlock.BorderThickness = new(0, 1, 1, 1);
                    TextBlock.Padding = new(-2, 0, 0, 0);
                }
                else
                {
                    HexLabel.Visibility = Visibility.Collapsed;
                    TextBlock.BorderThickness = new(1, 1, 1, 1);
                    TextBlock.Padding = new(0, 0, 0, 0);
                }
                UpdateShownValue();
            }
        }
        public Action<object, RoutedEventArgs>? ValueChanged { get; set; }

        public NumericUpDown()
        {
            InitializeComponent();
            UpdateShownValue();
        }

        private void UpdateShownValue()
        {
            TextBlock.Text = ValueToString();
        }

        private void ClickedUp(object sender, RoutedEventArgs e)
        {
            var newValue = Value + 1;
            Value = newValue > Maximum ? Value : newValue;
            UpdateShownValue();
            ValueChanged?.Invoke(this, new RoutedEventArgs());
        }
        private void ClickedDown(object sender, RoutedEventArgs e)
        {
            var newValue = Value - 1;
            Value = newValue < Minimum ? Value : newValue;
            UpdateShownValue();
            ValueChanged?.Invoke(this, new RoutedEventArgs());
        }
        private void TextPreviewInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = ContainsIncorrectCharacter(e.Text);
        }

        private void TextPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.IsDown)
            {
                if (e.Key == Key.Down)
                {
                    ClickedDown(sender, e);
                    e.Handled = true;
                }
                else if (e.Key == Key.Up)
                {
                    ClickedUp(sender, e);
                    e.Handled = true;
                }
            }
        }

        private void TextPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if(IsMouseOver && TextBlock.IsKeyboardFocusWithin)
            {
                if(e.Delta>100)
                {
                    ClickedUp(sender, e);
                }
                else if(e.Delta < 100)
                {
                    ClickedDown(sender, e);
                }
                e.Handled = true;
            }
        }

        private bool ContainsIncorrectCharacter(string str)
        {
            if (hexadecimal)
            {
                return !HexadecimalRegex().IsMatch(str);
            }
            else
            {
                return str != "-" && !int.TryParse(str, out _);
            }
        }

        private string ValueToString() => Hexadecimal ? ((int)Value).ToString("X") : Value.ToString("N0");

        private string CorrectValue(string str)
        {
            if (string.IsNullOrEmpty(str)) Value = 0;
            if (hexadecimal)
            {
                try
                {
                    var newValue = Convert.ToInt32(str, 16);
                    value = newValue;
                }
                catch (Exception)
                {

                }
            }
            else
            {
                if (int.TryParse(str, out var newValue))
                {
                    Value = newValue;
                }
            }
            return ValueToString();
        }

        [GeneratedRegex(@"(0[xX])?[0-9a-fA-F]+$")]
        private static partial Regex HexadecimalRegex();
    }
}
