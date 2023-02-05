using DispelTools.Common;
using DispelTools.Common.DataProcessing;
using DispelTools.ImageProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace DispelTools.GameDataModels.Map
{
    public partial class TileSet : IDisposable
    {
        public static readonly int TILE_WIDTH = 62;
        public static readonly int TILE_HEIGHT = 32;

        public static readonly int TILE_WIDTH_HALF = TILE_WIDTH / 2;
        public static readonly int TILE_HEIGHT_HALF = TILE_HEIGHT / 2;

        public static readonly int TILE_HORIZONTAL_OFFSET_HALF = 32;

        public static readonly int TILE_PIXEL_NUMBER = 32 * 32;

        private readonly ColorManagement colorManager = ColorManagement.From(ColorManagement.ColorMode.RGB16_565);

        private readonly List<Tile> tiles;
        private static Tile unknownTile;
        private static Tile blankTile;
        private bool disposedValue;

        public static Tile UnknownTile { get { if (unknownTile == null) { unknownTile = Tile.CreateTile(Color.Magenta); } return unknownTile; } }
        public static Tile BlankTile { get { if (blankTile == null) { blankTile = Tile.CreateTile(Color.FromArgb(0, 0, 0, 0)); } return blankTile; } }

        public int Count => tiles.Count;

        public static TileSet LoadTileSet(string filename, WorkReporter workReporter) => new TileSet(filename, workReporter);

        private TileSet(string filename, WorkReporter workReporter)
        {
            using (var file = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                int tileNumber = (int)(file.BaseStream.Length / (32 * 32 * 2));
                tiles = new List<Tile>(tileNumber);
                workReporter.SetTotal(tileNumber);
                for (int i = 0; i < tileNumber || file.BaseStream.Length > file.BaseStream.Position; i++)
                {
                    tiles.Add(Tile.ReadTile(file, colorManager));
                    workReporter.ReportProgress(i);
                }
            }
        }

        public Tile this[int i] => i < tiles.Count ? tiles[i] : UnknownTile;

        public RawRgb TileSetToOneBitmap()
        {
            int w = (int)Math.Ceiling(Math.Sqrt((double)tiles.Count / 4));
            int h = w * 4;
            var bigBitmap = new RawRgb((TILE_WIDTH * w) + (TILE_WIDTH / 2) + 1, TILE_HEIGHT * (int)Math.Ceiling((float)h / 2));
            int i = 0;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (tiles.Count == i)
                    {
                        break;
                    }
                    var tile = tiles[i++];
                    int offsetX = 0;
                    int offsetY = 0;
                    if (y % 2 != 0)
                    {
                        offsetX = TILE_WIDTH / 2 + 1;
                        offsetY = TILE_HEIGHT / 2;
                    }
                    tile.PlotTileOnBitmap(bigBitmap, x * TILE_WIDTH + offsetX, y / 2 * TILE_HEIGHT + offsetY);
                }
            }
            return bigBitmap;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                tiles.Clear();
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}