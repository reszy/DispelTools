using System.Drawing;

namespace DispelTools.Components.PictureDisplay
{
    internal interface IPictureDisplayerController
    {
        int RequiredTipWidth { get; }
        void ImageReloaded(Image newImage);
        void DrawTip(Graphics g, Font font, Rectangle tipRectangle);
        void DrawHighlight(ICoordsConverter sender, Graphics g, Point position, Pen highlightPen, float zoom);
        void PixelSelected(ICoordsConverter sender, PixelSelectedArgs args);
    }
}
