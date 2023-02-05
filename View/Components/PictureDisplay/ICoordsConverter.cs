using System.Drawing;

namespace View.Components.PictureDisplay
{
    public interface ICoordsConverter
    {
        Point ConvertToImageCoords(Point point);
        System.Windows.Point ConvertToPictureBoxCoords(Point point);
    }
}
