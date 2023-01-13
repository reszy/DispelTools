using System.Drawing;

namespace DispelTools.Components.PictureDisplayer
{
    internal interface IPictureDisplayerController
    {
        int RequiredTipWidth { get; }
        void ImageReloaded(Image newImage);
        void DrawTip(Graphics g, Font font, Rectangle tipRectangle);
        void DrawHighlight(PictureDiplayer.ICoordsConverter sender, Graphics g, Point position, Pen highlightPen, float zoom);
        void PixelSelected(PictureDiplayer.ICoordsConverter sender, PictureDiplayer.PixelSelectedArgs args);
    }
}
