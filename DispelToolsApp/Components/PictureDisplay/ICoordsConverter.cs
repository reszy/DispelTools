using System.Drawing;

namespace DispelTools.Components.PictureDisplay
{
    public interface ICoordsConverter
    {
        Point ConvertToImageCoords(Point point);
        PointF ConvertToPictureBoxCoords(Point point);
    }
}
