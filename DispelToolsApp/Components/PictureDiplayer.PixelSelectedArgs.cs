using System.Drawing;

namespace DispelTools.Components
{
    public partial class PictureDiplayer
    {
        public struct PixelSelectedArgs
        {
            public PixelSelectedArgs(Point position, Color pixelColor)
            {
                Position = position;
                PixelColor = pixelColor;
            }

            public Point Position { get; set; }
            public Color PixelColor { get; set; }
        }
    }
}
