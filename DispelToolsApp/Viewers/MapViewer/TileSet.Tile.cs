using DispelTools.Common;
using DispelTools.ImageProcessing;
using System.Drawing;
using System.IO;

namespace DispelTools.Viewers.MapViewer
{
    public partial class TileSet
    {
        public class Tile
        {
            private Color[] pixels;
            private static int[,] mask = CreateMask();

            private Tile(BinaryReader reader, ColorManagement colorManagement)
            {
                pixels = new Color[TILE_PIXEL_NUMBER];
                for (int i = 0; i < TILE_PIXEL_NUMBER; i++)
                {
                    byte[] pixelBytes = reader.ReadBytes(colorManagement.BytesConsumed);
                    pixels[i] = colorManagement.ProduceColor(pixelBytes);
                }
            }
            private Tile(Color color)
            {
                pixels = new Color[TILE_PIXEL_NUMBER];
                for (int i = 0; i < TILE_PIXEL_NUMBER; i++)
                {
                    pixels[i] = color;
                }
            }
            private Tile(Color[] pixels)
            {
                this.pixels = pixels;
            }

            public static Tile ReadTile(BinaryReader reader, ColorManagement colorManagement) => new Tile(reader, colorManagement);
            public static Tile CreateTile(Color color) => new Tile(color);

            private static int[,] CreateMask()
            {
                int[,] mask = new int[2, TILE_HEIGHT];
                int pixelsX = 1;
                int step = 2;
                int direction = 1;
                int limit = 31;

                for (int y = 0; y < TILE_HEIGHT; y++)
                {
                    mask[0, y] = TILE_WIDTH / 2 - pixelsX;
                    mask[1, y] = pixelsX * 2;
                    pixelsX += step * direction;
                    if (pixelsX > limit)
                    {
                        direction = -1;
                        pixelsX = limit;
                    }

                }
                return mask;
            }
            public void PlotTileOnBitmap(ref DirectBitmap parent, int destX, int destY)
            {
                if (destX + TILE_WIDTH <= parent.Width && destX >= 0 && destY >= 0 && destY + TILE_HEIGHT <= parent.Height)
                {
                    int i = 0;
                    for (int y = 0; y < TILE_HEIGHT; y++)
                    {
                        for (int x = 0; x < mask[1, y]; x++)
                        {
                            var pixel = pixels[i++];
                            int finalX = destX + x + mask[0, y];
                            int finalY = destY + y;
                            if (pixel.ToArgb() != Color.Black.ToArgb())
                            {
                                parent.SetPixel(finalX, finalY, pixel);
                            }
                        }
                    }
                }
            }

            public Tile MixColor(Color color, byte alpha)
            {
                var pixels = new Color[TILE_PIXEL_NUMBER];
                double amount = (double)alpha / byte.MaxValue;
                for (int i = 0; i < TILE_PIXEL_NUMBER; i++)
                {
                    var baseColor = this.pixels[i];
                    byte r = (byte)((color.R * amount) + baseColor.R * (1 - amount));
                    byte g = (byte)((color.G * amount) + baseColor.G * (1 - amount));
                    byte b = (byte)((color.B * amount) + baseColor.B * (1 - amount));
                    pixels[i] = Color.FromArgb(r, g, b);
                }
                return new Tile(pixels);
            }
        }
    }
}
