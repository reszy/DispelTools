using System.Drawing;
using System.Windows.Forms;

namespace DispelTools.Components.PictureDisplay
{
    public struct PixelSelectedArgs
    {
        public PixelSelectedArgs(Point position, Color pixelColor, Keys modifierKeys)
        {
            Position = position;
            PixelColor = pixelColor;
            ModifierKeys = modifierKeys;
        }

        public Point Position { get; }
        public Color PixelColor { get; }
        public Keys ModifierKeys { get; }
    }
}
