using System.Drawing;
using System.Windows.Input;

namespace View.Components.PictureDisplay
{
    public readonly struct PixelSelectedArgs
    {
        public PixelSelectedArgs(Point position, Color pixelColor, ModifierKeys modifierKeys)
        {
            Position = position;
            PixelColor = pixelColor;
            ModifierKeys = modifierKeys;
        }

        public Point Position { get; }
        public Color PixelColor { get; }
        public ModifierKeys ModifierKeys { get; }
    }
}
