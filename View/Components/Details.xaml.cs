using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace View.Components
{
    /// <summary>
    /// Interaction logic for Details.xaml
    /// </summary>
    public partial class Details : UserControl
    {
        private int unfoldSize = 200;
        private bool folded = true;
        private bool autoScroll = true;

        public string Header { get => (string)HeaderLabel.Content; set => HeaderLabel.Content = value; }
        public int UnfoldSize { get => unfoldSize; set { unfoldSize = value; ContentBox.Height = unfoldSize; } }

        public static readonly RoutedEvent CollapseArrowRotation = EventManager.RegisterRoutedEvent("Collapse", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(Details));
        public static readonly RoutedEvent ExpandArrowRotation = EventManager.RegisterRoutedEvent("Expand", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(Details));

        public event RoutedEventHandler Collapse
        {
            add => AddHandler(CollapseArrowRotation, value);
            remove => RemoveHandler(CollapseArrowRotation, value);
        }
        public event RoutedEventHandler Expand
        {
            add => AddHandler(ExpandArrowRotation, value);
            remove => RemoveHandler(ExpandArrowRotation, value);
        }
        public Details()
        {
            InitializeComponent();
        }

        private void HeaderPanelClick(object? sender, MouseButtonEventArgs e)
        {
            if (folded)
            {
                ContentBox.Visibility = Visibility.Visible;
                ContentBox.Height = UnfoldSize;
                RaiseEvent(new RoutedEventArgs(ExpandArrowRotation));
            }
            else
            {
                ContentBox.Visibility = Visibility.Collapsed;
                RaiseEvent(new RoutedEventArgs(CollapseArrowRotation));
            }
            folded = !folded;
        }
        public void AddDetails(List<string> details)
        {
            foreach (var detail in details)
            {
                TextContainer.Children.Add(CreateTextBlock(detail));
            }
        }
        public void AddDetails(string details)
        {
            TextContainer.Children.Add(CreateTextBlock(details));
        }
        public void ClearDetails()
        {
            TextContainer.Children.Clear();
        }

        private static TextBlock CreateTextBlock(string text) => new() { Text = text };

        private void ScrollBoxScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {
                if (ScrollBox.VerticalOffset == ScrollBox.ScrollableHeight)
                {
                    autoScroll = true;
                }
                else
                {
                    autoScroll = false;
                }
            }
            else if (autoScroll)
            {
                ScrollBox.ScrollToVerticalOffset(ScrollBox.ExtentHeight);
            }
        }
    }
}
