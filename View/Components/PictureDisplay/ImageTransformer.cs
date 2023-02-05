using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace View.Components.PictureDisplay
{
    public class ImageTransformer
    {
        private readonly Image image;
        public double X { get; private set; }
        public double Y { get; private set; }
        public double ZoomValue { get; private set; }

        public ImageTransformer(Image image)
        {
            this.image = image;
            Reset();
        }

        public void Reset()
        {
            X = 0;
            Y = 0;
            ZoomValue = 1.0f;
            SetPosition();
            if (image.Source is not null)
            {
                SetSize(image.Source.Width, image.Source.Height);
            }
        }

        public void Move(int x, int y)
        {
            X += x;
            Y += y;
            SetPosition();
        }

        public void Zoom(int originX, int originY, double zoom)
        {
            var zoomDiff = zoom / ZoomValue;
            X = CalcZoomMove(zoomDiff, X, originX);
            Y = CalcZoomMove(zoomDiff, Y, originY);
            ZoomValue = zoom;
            if (image.Source is not null)
            {
                var size = CalculateZoomedImageSize();
                SetPosition();
                SetSize(size.width, size.height);
            }
        }

        private void SetPosition()
        {
            Canvas.SetLeft(image, X);
            Canvas.SetTop(image, Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double CalcZoomMove(double zoomDiff, double a, double a_origin) => a_origin - ((a_origin - a) * zoomDiff);

        private void SetSize(double width, double height)
        {
            image.Width = width;
            image.Height = height;
        }
        private (double width, double height) CalculateZoomedImageSize()
        {
            double width = image.Source.Width * ZoomValue;
            double height = image.Source.Height * ZoomValue;
            return (width, height);
        }
    }
}
