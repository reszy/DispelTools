using DispelTools.Common;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using DispelTools.ImageAnalyzer;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace View.Components.PictureDisplay
{
    /// <summary>
    /// Interaction logic for PictureDisplayer.xaml
    /// </summary>
    public partial class PictureDisplayer : UserControl, ICoordsConverter
    {
        public delegate void PixelSelectedHandler(object sender, PixelSelectedArgs point);
        public event PixelSelectedHandler? PixelSelectedEvent;
        public enum MouseMode { Pointer, RectSelector, RowSelector };
        public MouseMode CurrentMouseMode { get; set; }

        private readonly ImageTransformer imageTransformer;

        private RawBitmap? imageSource;


        //VIEWPORT
        private Point startingPoint;
        private bool panning;

        private Point pointingAt;

        //ZOOM
        private ImageZoom zoom;

        //HIGLIGHT
        private HighlightCursor highlight;

        //DATA TIP
        private SelectedPixelData? selectedPixel;
        private bool showHex = false;


        //SELECTION
        private bool selecting;
        private Point selectStart;
        private Point selectEnd;

        private DataAnalyzedBitmap.DataPixel selectedPixelData;
        private IPictureDisplayerController? subComponent;

        private bool displayTipInTopHalf = true;

        public bool ShowDataTip
        {
            get => DataTip.Visibility == System.Windows.Visibility.Visible;
            set => DataTip.Visibility = value ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }
        public DataAnalyzedBitmap DataAnalyzedBitamp { private get; set; }

        public record SelectedPixelData(Point Coords, System.Drawing.Color Color)
        {
            public static SelectedPixelData CreateFrom(RawBitmap image, int x, int y)
            {
                Point coords = new(Math.Clamp(x, 0, image.Width - 1), Math.Clamp(y, 0, image.Height - 1));
                return new(coords, image.GetPixel(coords.X, coords.Y));
            }

            internal static SelectedPixelData CreateFrom(RawBitmap imageSource, Point point) => CreateFrom(imageSource, point.X, point.Y);
        }


        public PictureDisplayer()
        {
            InitializeComponent();
            highlight = new(ThisCanvas);
            imageTransformer = new(ImageElement);
            zoom = new();
            ResetImage();
        }

        private void MouseMoved(object sender, MouseEventArgs e)
        {
            var Location = e.GetPosition(this);
            if (ImageElement.Source is not null && imageSource is not null)
            {
                if (panning)
                {
                    var delta = new Point((int)Location.X - startingPoint.X, (int)Location.Y - startingPoint.Y);
                    startingPoint = new Point((int)Location.X, (int)Location.Y);

                    imageTransformer.Move(delta.X, delta.Y);
                    highlight.Move(delta.X, delta.Y);
                }
                else
                {
                    pointingAt = new Point((int)Location.X, (int)Location.Y);
                    Highlight(ConvertToImageCoords(pointingAt));
                }
                if (selecting)
                {
                    selectEnd = ConvertToImageCoords(pointingAt);
                }
                Invalidate();
            }
            if ((Location.Y > Height / 2) ^ displayTipInTopHalf)
            {
                displayTipInTopHalf = !displayTipInTopHalf;
                if (displayTipInTopHalf)
                {
                    Canvas.SetTop(DataTip, 20);
                    Canvas.SetBottom(DataTip, double.NaN);
                }
                else
                {
                    Canvas.SetBottom(DataTip, 20);
                    Canvas.SetTop(DataTip, double.NaN);
                }
            }
        }
        private void MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ImageElement.Source is not null && imageSource is not null)
            {
                var Location = e.GetPosition(this);
                if (e.RightButton == MouseButtonState.Pressed && !selecting)
                {
                    Cursor = Cursors.Hand;
                    panning = true;
                    Mouse.Capture(ThisCanvas, CaptureMode.SubTree);
                    startingPoint = new Point((int)Location.X, (int)Location.Y);
                }
                else
                {
                    selectedPixel = SelectedPixelData.CreateFrom(imageSource, ConvertToImageCoords(Location));
                    subComponent?.PixelSelected(this, new PixelSelectedArgs(selectedPixel, Keyboard.Modifiers));
                    switch (CurrentMouseMode)
                    {
                        case MouseMode.RectSelector:
                        case MouseMode.RowSelector:
                            {
                                selectStart = selectEnd = ConvertToImageCoords(pointingAt);
                                selecting = true;
                            }
                            break;
                        case MouseMode.Pointer:
                        default:
                            {
                                PixelSelectedEvent?.Invoke(this, new PixelSelectedArgs(selectedPixel, Keyboard.Modifiers));
                                Invalidate();
                            }
                            break;
                    }
                }
            }
        }

        private void MouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Released)
            {
                if (panning) e.Handled = true;
                this.Cursor = Cursors.Arrow;
                panning = false;
                selecting = false;
                Mouse.Capture(null);
            }
        }
        private void MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            if (imageSource is not null)
            {
                if (!panning)
                {
                    if (e.Delta / 120 > 0)
                    {
                        zoom.StepUp();
                    }
                    else
                    {
                        zoom.StepDown();
                    }
                    var origin = e.GetPosition(this);
                    imageTransformer.Zoom((int)origin.X, (int)origin.Y, zoom.GetZoom());
                    highlight.SetScale(zoom.GetZoom());
                    Invalidate();
                }
            }
        }
        public void ClearImage()
        {
            imageSource = null;
            ImageElement.Source = null;
            ResetImage();
            DataAnalyzedBitamp = null;
            Invalidate();
        }

        public void SetImage(RawBitmap bitmap, bool reset, DataAnalyzedBitmap data = null)
        {
            imageSource = bitmap;
            ImageElement.Source = DisplayedImage.Create(imageSource);
            if (reset)
            {
                ResetImage();
                InitImageInContainer();
                DataAnalyzedBitamp = data;
            }
            Invalidate();
        }

        public void Invalidate()
        {
            Zoom.ValueText = $"{zoom.GetZoom():0.####}";
            var pointingPosition = ConvertToImageCoords(pointingAt);
            XPos.ValueText = pointingPosition.X.ToString();
            YPos.ValueText = pointingPosition.Y.ToString();
            if (selectedPixel is not null)
            {
                Ch1.ValueText = selectedPixel.Color.R.ToString();
                Ch2.ValueText = selectedPixel.Color.G.ToString();
                Ch3.ValueText = selectedPixel.Color.B.ToString();
                Ch4.ValueText = selectedPixel.Color.A.ToString();
            }
        }

        public void DrawAdditionalInfo(AdditionalInfo additionalInfo)
        {
            SubComponentTipPanel.Children.Clear();
            foreach (var info in additionalInfo.ToUiElements())
                SubComponentTipPanel.Children.Add(info);
        }

        public void DrawAdditionalText(string text)
        {
            TemporalTip.Visibility = string.IsNullOrEmpty(text) ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
            TemporalTip.Content = text;
        }

        /////////////////
        //  FROM CORE  //
        /////////////////

        internal void SetController(IPictureDisplayerController displayerController)
        {
            subComponent = displayerController;
            if (subComponent.HasCustomHighlight)
            {
                var customHighlight = subComponent.CreateCustomHighlight();
                highlight.SetCustomCursor(customHighlight.polygon, customHighlight.offset);
            }
            else
            {
                highlight.SetCustomCursor(null, null);
            }
        }

        public void ResetImage()
        {
            zoom.Reset();
            imageTransformer.Reset();
            startingPoint = Point.Empty;
            panning = false;
        }

        public Point ConvertToImageCoords(System.Windows.Point point)
        {
            return new Point((int)Math.Floor((point.X - imageTransformer.X) / zoom.GetZoom()),
                             (int)Math.Floor((point.Y - imageTransformer.Y) / zoom.GetZoom()));
        }
        public Point ConvertToImageCoords(Point point)
        {
            return new Point((int)Math.Floor((point.X - imageTransformer.X) / zoom.GetZoom()),
                             (int)Math.Floor((point.Y - imageTransformer.Y) / zoom.GetZoom()));
        }

        public System.Windows.Point ConvertToPictureBoxCoords(Point point)
        {
            return new((double)Math.Floor(point.X * zoom.GetZoom() + imageTransformer.X),
                        (double)Math.Floor(point.Y * zoom.GetZoom() + imageTransformer.Y));
        }

        public void InitImageInContainer()
        {
            var imageSize = new Size((int)ImageElement.Source.Width, (int)ImageElement.Source.Height);
            var containerSize = this.RenderSize;
            double imageRatio = imageSize.Width / (double)imageSize.Height;
            double containerRatio = containerSize.Width / (double)containerSize.Height;
            var fitSize = containerRatio > imageRatio ?
                new Size(imageSize.Width * (int)containerSize.Height / imageSize.Height, (int)containerSize.Height) :
                new Size((int)containerSize.Width, imageSize.Height * (int)containerSize.Width / imageSize.Width);

            double defaultZoom = (double)fitSize.Width / imageSize.Width;
            zoom.InitZoom(defaultZoom);
            imageTransformer.Zoom(0, 0, zoom.GetZoom());
        }

        private void Highlight(Point point)
        {
            if (imageSource is null) return;
            if (point.X >= 0 && point.X < imageSource.Width && point.Y >= 0 && point.Y < imageSource.Height)
            {
                var highlightPixelColor = imageSource.GetPixel(point.X, point.Y);
                highlight.SetColorInversed(highlightPixelColor);
                if (subComponent is not null && subComponent.HasCustomHighlight)
                {
                    var position = subComponent.GetCustomHighlightImagePosition(point);
                    var canvasposition = ConvertToPictureBoxCoords(position);
                    highlight.SetPosition(canvasposition, zoom.GetZoom());
                }
                else
                {
                    highlight.SetPosition(new System.Windows.Point(pointingAt.X, pointingAt.Y), zoom.GetZoom());
                }
            }
        }

        internal void CenterView()
        {
            if (imageSource is null) return;
            imageTransformer.X = (this.Width / 2) - (ImageElement.Width / 2);
            imageTransformer.Y = (this.Height / 2) - (ImageElement.Height / 2);
        }
    }
}
