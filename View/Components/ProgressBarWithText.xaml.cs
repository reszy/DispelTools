using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for ProgressBarWithText.xaml
    /// </summary>
    public partial class ProgressBarWithText : UserControl
    {
        private int _value = 0;
        private string text = string.Empty;
        private int lastProgressWidth = 0;
        public ProgressBarWithText()
        {
            InitializeComponent();
        }

        public string Text { get => text; set { if (text != value) { text = value; Label.Content = value; } } }
        public int Maximum { get; set; } = 1;
        public int Value
        {
            get => _value; set
            {
                if (_value != value)
                {
                    _value = value;
                    UpdateBar();
                }
            }
        }
        private int CalculateProgressWidth(int value) => (int)((double)value / Maximum * RenderSize.Width);

        private void UpdateBar()
        {
            int progressWidth = CalculateProgressWidth(_value);
            if (progressWidth != lastProgressWidth)
            {
                lastProgressWidth = progressWidth;
                ProgressBar.Width = progressWidth;
            }
        }
    }
}
