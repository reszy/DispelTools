using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for NotifyTextBox.xaml
    /// </summary>
    public partial class NotifyTextBox : UserControl
    {
        public NotifyTextBox()
        {
            InitializeComponent();
        }
        private string strValue = string.Empty;

        public string StrValue
        {
            get => strValue;
            set
            {
                strValue = value;
                ValueChanged?.Invoke(this, new RoutedEventArgs());
            }
        }
        public Action<object, RoutedEventArgs>? ValueChanged { get; set; }
        public string Text { get => TextBox.Text; set => TextBox.Text = value; }
    }
}
