using System.Drawing;
using System.Windows.Shapes;

namespace View.Components.PictureDisplay
{
    internal interface IPictureDisplayerController
    {
        bool HasCustomHighlight { get; }
        Polygon CreateCustomHighlight();
        Point GetCustomHighlightImagePosition(Point pointerImagePosition);
        void PixelSelected(ICoordsConverter sender, PixelSelectedArgs args);
    }
}
