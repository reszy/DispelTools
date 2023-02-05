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

namespace View.Components.PictureDisplay
{
    /// <summary>
    /// Interaction logic for LabelWithValue.xaml
    /// </summary>
    public partial class LabelWithValue : UserControl
    {
        public readonly static DependencyProperty LabelTextProperty = DependencyProperty.Register(
            "LabelText",
            typeof(string),
            typeof(UserControl),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.AffectsRender,
                LabelTextPropertyChanged
            ));

        private static void LabelTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is LabelWithValue lwv)
            {
                lwv.Label.Content = e.NewValue;
            }
        }

        public readonly static DependencyProperty ValueTextProperty = DependencyProperty.Register(
            "ValueText",
            typeof(string),
            typeof(UserControl),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.AffectsRender,
                ValueTextPropertyChanged
            ));

        private static void ValueTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LabelWithValue lwv)
            {
                lwv.Value.Content = e.NewValue;
            }
        }

        public LabelWithValue()
        {
            InitializeComponent();
        }

        public string LabelText { get => (string)Label.Content; set => Label.Content = value; }
        public string ValueText { get => (string)Value.Content; set => Value.Content = value; }
    }
}
