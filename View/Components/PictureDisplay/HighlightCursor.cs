using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace View.Components.PictureDisplay
{
    public class HighlightCursor
    {
        private readonly Canvas canvas;

        private Polygon? custom;
        private Border standard;
        private double orginalWidth;
        private double orginalHeight;
        private System.Windows.Point origin;

        public int X { get; private set; }
        public int Y { get; private set; }
        public Point Point { get => new(X,Y); }
        public HighlightCursor(Canvas canvas)
        {
            this.canvas = canvas;
            standard= new Border() { Background = new SolidColorBrush(Colors.Transparent), BorderThickness = new System.Windows.Thickness(1) };
        }
        public void SetCustomCursor(Polygon? custom, System.Windows.Point? origin)
        {
            this.origin = origin ?? default;
            canvas.Children.Remove(this.custom);
            this.custom = custom;
            if (custom is null)
            {
                standard.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                standard.Visibility = System.Windows.Visibility.Hidden;
                orginalWidth = custom.Width;
                orginalHeight = custom.Height;
                canvas.Children.Add(custom);
            }
        }

        public void Move(int x, int y)
        {
            if (custom is null)
            {
                Canvas.SetLeft(standard, Canvas.GetLeft(standard) + x);
                Canvas.SetTop(standard, Canvas.GetTop(standard) + y);
            }
            else
            {
                Canvas.SetLeft(custom, Canvas.GetLeft(custom) + x);
                Canvas.SetTop(custom, Canvas.GetTop(custom) + y);
            }
        }

        public void SetScale(double scale)
        {
            if (custom is null)
            {
                standard.Width = scale;
                standard.Height = scale;
            }
            else
            {
                custom.Width = orginalWidth * scale;
                custom.Height = orginalHeight * scale;
            }
        }

        public void SetPosition(System.Windows.Point position, double scale)
        {
            if (custom is null)
            {
                Canvas.SetLeft(standard, position.X);
                Canvas.SetTop(standard, position.Y);
                standard.Width = scale;
                standard.Height = scale;
            }
            else
            {
                Canvas.SetLeft(custom, position.X - origin.X * scale);
                Canvas.SetTop(custom, position.Y - origin.Y * scale);
                custom.Width = orginalWidth * scale;
                custom.Height = orginalHeight * scale;
            }
        }
        public void SetColorInversed(System.Drawing.Color color)
        {
            SetColor(System.Windows.Media.Color.FromRgb((byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B)));
        }
        public void SetColor(System.Windows.Media.Color color)
        {
            var brush = new SolidColorBrush(color);
            if (custom is null)
            {
                standard.BorderBrush = brush;
            }
            else
            {
                custom.Stroke = brush;
            }
        }
    }
}
