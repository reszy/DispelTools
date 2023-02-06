using System.Drawing;
using System.Windows.Input;
using static View.Components.PictureDisplay.PictureDisplayer;

namespace View.Components.PictureDisplay
{
    public readonly struct PixelSelectedArgs
    {

        public PixelSelectedArgs(SelectedPixelData pixelData, ModifierKeys modifierKeys)
        {
            PixelData = pixelData;
            ModifierKeys = modifierKeys;
        }

        public SelectedPixelData PixelData { get; }
        public Point Position => PixelData.Coords;
        public Color PixelColor => PixelData.Color;
        public ModifierKeys ModifierKeys { get; }
    }
}
