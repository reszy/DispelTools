using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        public readonly static DependencyProperty ValueTextProperty = DependencyProperty.Register(
            "Fill",
            typeof(Brush),
            typeof(UserControl),
            new FrameworkPropertyMetadata(
                new SolidColorBrush(Color.FromRgb(0, 192, 0)),
                FrameworkPropertyMetadataOptions.AffectsRender,
                FillPropertyChanged
            ));

        private static void FillPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProgressBarWithText pbwt && e.NewValue is Brush brush)
            {
                pbwt.Fill = brush;
            }
        }
        public Brush Fill { get => ProgressBar.Fill; set => ProgressBar.Fill = value; }
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
        private int CalculateProgressWidth(int value) => (int)((double)value / Maximum * (RenderSize.Width - Border.BorderThickness.Right - Border.BorderThickness.Left));

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
