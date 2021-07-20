using DispelTools.Common;
using DispelTools.DebugTools.MetricTools;
using DispelTools.ImageProcessing.Sprite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using static DispelTools.Viewers.MapViewer.MapReader.WorkReporter;

namespace DispelTools.Viewers.MapViewer
{
    public partial class MapReader : IDisposable
    {
        private const uint WHITE_COLOR = 0xFF000000;
        private readonly string filename;

        private readonly WorkReporter workReporter = new WorkReporter();
        private readonly BackgroundWorker backgroundWorker;

        private MapModel map;
        private List<SpriteSequence> sprites = new List<SpriteSequence>();
        private TileSet btl;
        private TileSet gtl;
        private int progressTrack = 0;

        private bool tilesLoaded = false;
        private bool occluded;
        private bool disposedValue;
        public class GeneratorOptions
        {
            public bool Occlusion { get; set; } = true;
            public bool GTL { get; set; } = true;
            public bool Sprites { get; set; } = false;
            public bool Collisions { get; set; } = false;
            public bool BTL { get; set; } = false;
            public bool BLDG { get; set; } = false;
        }

        public TileSet Btl => btl;
        public TileSet Gtl => gtl;
        public List<SpriteSequence> Sprites => sprites;
        public bool MapModelLoaded => map != null;
        public MapReader(string filename, BackgroundWorker backgroundWorker)
        {
            this.filename = filename;
            this.backgroundWorker = backgroundWorker;
            map = null;
            backgroundWorker.WorkerReportsProgress = true;
            workReporter.ReportWork += ProgressChanged;
        }

        public DirectBitmap GenerateMap(GeneratorOptions generatorOptions)
        {
            if (map == null)
            {
                ReadMap();
            }
            if (!tilesLoaded)
            {
                LoadTiles();
            }
            occluded = generatorOptions.Occlusion;
            var mapImage = CreateImageOfMap(generatorOptions);
            backgroundWorker.ReportProgress(3000, "Complete");
            return mapImage;
        }
        public void ReadMap()
        {
            using (var file = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                int width = file.ReadInt32();
                int height = file.ReadInt32();
                map = new MapModel(width, height);

                ReadFirstBlock(file);
                ReadSecondBlock(file);
                ReadSpritesBlock(file);
                ReadFourthBlock(file);
                ReadEventAndBtlgBlock(file);
                ReadTilesAndAccessBlock(file);
            }
        }

        public string GetStats()
        {
            var sb = new StringBuilder();
            if (MapModelLoaded)
            {
                sb.AppendLine("--Map Model--");
                sb.Append("Height: ");
                sb.Append(map.TiledMapSize.Height);
                sb.AppendLine();
                sb.Append("Width: ");
                sb.Append(map.TiledMapSize.Width);
                sb.AppendLine();
                sb.Append("Sprites included: ");
                sb.Append(sprites.Count);
                sb.AppendLine();
                sb.Append("Sprites on map: ");
                sb.Append(map.GetSpritesData().Count);
                sb.AppendLine();
            }
            if (tilesLoaded)
            {
                sb.AppendLine();
                sb.AppendLine("--Tiles--");
                sb.Append("GTL: ");
                sb.AppendLine();
                sb.Append(gtl.Count);
                sb.AppendLine();
                sb.Append("BTL: ");
                sb.Append(btl.Count);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public Point TranslateImageToMapPosition(Point position)
        {
            int diagonal = map.MapDiagonalTiles;

            int sx = position.X;
            int sy = position.Y;

            int mx = (sx / TileSet.TILE_WIDTH_HALF - (sy - (diagonal / 2 * TileSet.TILE_HEIGHT_HALF)) / TileSet.TILE_HEIGHT_HALF) / 2;
            int my = (sy - (diagonal / 2 * TileSet.TILE_HEIGHT_HALF)) / TileSet.TILE_HEIGHT_HALF + mx;

            return new Point(mx, my);
        }

        private void ReadFirstBlock(BinaryReader file)
        {
            int multiplier = file.ReadInt32();
            int size = file.ReadInt32();
            file.BaseStream.Seek(8, SeekOrigin.Begin);
            file.Skip(multiplier * size * 4);//skip unknown data
        }

        private void ReadSecondBlock(BinaryReader file)
        {
            int size = file.ReadInt32();
            file.Skip(size * 2);
        }

        private void ReadSpritesBlock(BinaryReader file)
        {
            int spritesCount = file.ReadInt32();
            var spriteLoader = new SpriteLoader(file, filename);
            for (int i = 0; i < spritesCount; i++)
            {
                int imageStamp = file.ReadInt32();
                int imageOffset = imageStamp == 6 ? 1904 : (imageStamp == 9 ? 2996 : throw new NotImplementedException($"Unexpected imageStamp {imageStamp}"));
                file.Skip(264);
                sprites.Add(spriteLoader.LoadSequence());
                file.Skip(imageOffset);
            }
        }

        private void ReadFourthBlock(BinaryReader file)
        {
            int zeros = 0;
            int data = 0;
            long dataStart = file.BaseStream.Position;
            long zerosStart = 0;

            int[] sequence = new[] { 8, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };
            int sequenceLength = 12;
            int sequencePointer = 0;
            long sequenceStart = dataStart;

            int count = file.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                int sprId = file.ReadInt32();
                file.ReadInt32();
                file.ReadInt32();
                file.ReadInt32();
                file.ReadInt32();
                int sprX = file.ReadInt32();
                int sprY = file.ReadInt32();
                map.SetSprite(sprId, sprX, sprY);
            }

            int bundlesCount = 0;
            bool stop = false;
            while (!stop)
            {
                int v1 = file.ReadInt32();
                long position = file.BaseStream.Position;
                file.ReadInt32();
                int v3 = file.ReadInt32();
                file.SetPosition(position);
                if (v3 == 0)
                {
                    bundlesCount = v1;
                    file.Skip(4);
                    stop = true;
                }
            }

            while (zeros < 300)
            {
                byte value = file.ReadByte();

                if (sequencePointer != sequenceLength)
                {
                    if (value == sequence[sequencePointer])
                    {
                        sequencePointer++;
                        if (sequenceStart == 0)
                        {
                            sequenceStart = file.BaseStream.Position;
                        }
                        if (sequencePointer == sequenceLength)
                        {
                            Metrics.Count(MetricFile.MapReadMetric, $"{Path.GetFileName(filename)}.sequenceFound");
                        }
                    }
                    else
                    {
                        sequencePointer = 0;
                        if (value == sequence[sequencePointer])
                        {
                            sequencePointer++;
                            sequenceStart = file.BaseStream.Position;
                        }
                    }
                }

                if (value != 0)
                {
                    zerosStart = 0;
                    if (dataStart == 0)
                    {
                        dataStart = file.BaseStream.Position;
                    }
                    if (zeros > 100)
                    {
                        Metrics.Count(MetricFile.MapReadMetric, $"{Path.GetFileName(filename)}.zeros", zeros.ToString());
                        Metrics.Count(MetricFile.MapReadMetric, "zeros", zeros.ToString());
                    }
                    data++;
                    zeros = 0;
                }
                else
                {
                    zeros++;
                    if (value == 0 && zerosStart == 0)
                    {
                        zerosStart = file.BaseStream.Position;
                    }
                    if (zeros < 4)
                    {
                        data++;
                    }
                    if (sequencePointer == sequenceLength && zeros > 100)
                    {
                        long diff = zerosStart - sequenceStart + 3;
                        Metrics.Count(MetricFile.MapReadMetric, "sequenceLength", diff.ToString());
                        Metrics.Count(MetricFile.MapReadMetric, $"{Path.GetFileName(filename)}.sequenceLength", diff.ToString());
                        data = 0;
                        dataStart = 0;
                        sequenceStart = 0;
                        sequencePointer = 0;
                    }
                }
            }
            file.SetPosition(zerosStart + 2);
        }

        private void ReadEventAndBtlgBlock(BinaryReader file) => file.Skip(map.TiledMapSize.Width * map.TiledMapSize.Height * 4);//TODO event layer

        private void ReadTilesAndAccessBlock(BinaryReader file)
        {
            for (int y = 0; y < map.TiledMapSize.Height; y++)
            {
                for (int x = 0; x < map.TiledMapSize.Width; x++)
                {
                    int bytes = file.ReadInt32();
                    int gtlBytes = bytes >> 10;
                    map.SetIds(x, y, gtlBytes, 0);
                    map.SetCollision(x, y, (bytes & 0x1) == 1);
                }
            }
        }

        private void LoadTiles()
        {
            progressTrack = 0;
            backgroundWorker.ReportProgress(progressTrack, "Loading BTL...");
            btl = TileSet.LoadTileSet(filename.Replace(".map", ".btl"), workReporter);
            progressTrack = 1000;
            backgroundWorker.ReportProgress(progressTrack, "Loading GTL...");
            gtl = TileSet.LoadTileSet(filename.Replace(".map", ".gtl"), workReporter);
            tilesLoaded = true;
        }

        private DirectBitmap CreateImageOfMap(GeneratorOptions generatorOptions)
        {
            progressTrack = 2000;

            var imageWidth = generatorOptions.Occlusion ? map.OccludedMapSizeInPixels.Width :map.MapSizeInPixels.Width;
            var imageHeight = generatorOptions.Occlusion ? map.OccludedMapSizeInPixels.Height:map.MapSizeInPixels.Height;
            var mapImage = new DirectBitmap(imageWidth, imageHeight);

            int total = map.MapSizeInPixels.Width * map.MapSizeInPixels.Height;
            backgroundWorker.ReportProgress(progressTrack, "Generating map...");
            workReporter.ReportProgress(0, total);

            for (int y = 0; y < map.TiledMapSize.Height; y++)
            {
                for (int x = 0; x < map.TiledMapSize.Width; x++)
                {
                    var tile = gtl[map.GetGtlId(x, y)];
                    if (generatorOptions.Collisions && map.GetCollision(x, y))
                    {
                        tile = tile.MixColor(Color.Red, 128);
                    }
                    if (map.GetBldg(x, y) > 0)
                    {
                        tile = tile.MixColor(Color.Blue, 128);
                    }
                    var mapCoords = ConvertMapCoordsToImageCoords(x, y);
                    if (occluded)
                    {
                        mapCoords.X -= map.MapNonOccludedStart.X;
                        mapCoords.Y -= map.MapNonOccludedStart.Y;
                    }
                    tile.PlotTileOnBitmap(ref mapImage, mapCoords.X, mapCoords.Y);
                    workReporter.ReportProgress(x + y * map.TiledMapSize.Width, total);
                }
            }
            if (generatorOptions.Sprites)
            {
                map.GetSpritesData().Sort(new SpriteSorter());
                foreach (var spriteData in map.GetSpritesData())
                {
                    if (sprites.Count > spriteData.Id)
                    {
                        var sprite = sprites[spriteData.Id];
                        PlotSpriteOnBitmap(ref mapImage, sprite.GetFrame(0).Bitmap, spriteData.Position.X, spriteData.Position.Y);
                    }
                }
            }
            return mapImage;
        }

        private class SpriteSorter : IComparer<MapModel.SpriteData>
        {
            public int Compare(MapModel.SpriteData x, MapModel.SpriteData y) => x.Position.Y - y.Position.Y;
        }

        private void PlotSpriteOnBitmap(ref DirectBitmap parent, DirectBitmap sprite, int destX, int destY)
        {
            //TODO Move comments to docs
            //destX += (map.Width / 5) * TileSet.TILE_WIDTH *;
            //destY += (map.Height/5) * TileSet.TILE_HEIGHT;

            //cat1 5x5
            //destX += 2400;
            //destY += 784;

            //dun1 10x10
            //destX += 2400 * 2 - TileSet.TILE_HORIZONTAL_OFFSET_HALF;
            //destY += 784 * 2 + TileSet.TILE_HEIGHT;

            //catp 8x8
            //destX += 3808; //2400 * 1.6 - TileSet.TILE_HORIZONTAL_OFFSET_HALF;
            //destY += 1280; //784 * 1.6 + TileSet.TileHeight

            //map1 20x20
            //destX += 9568; //2400 * 1.6 - TileSet.TILE_HORIZONTAL_OFFSET_HALF;
            //destY += 3200; //784 * 1.6 + TileSet.TileHeight
            if (!occluded)
            {
                destX += map.MapNonOccludedStart.X;
                destY += map.MapNonOccludedStart.Y;
            }
            if (destX + sprite.Width <= parent.Width && destX >= 0 && destY >= 0 && destY + sprite.Height <= parent.Height)
            {
                for (int y = 0; y < sprite.Height; y++)
                {
                    for (int x = 0; x < sprite.Width; x++)
                    {
                        var color = sprite.GetPixel(x, y);
                        if ((uint)color.ToArgb() != WHITE_COLOR)
                        {
                            int finalX = destX + x;
                            int finalY = destY + y;
                            parent.SetPixel(finalX, finalY, color);
                        }
                    }
                }
            }
        }

        private Point ConvertMapCoordsToImageCoords(int x, int y)
        {
            return new Point(
                   (x + y) * TileSet.TILE_HORIZONTAL_OFFSET_HALF,
                   (-x + y) * TileSet.TILE_HEIGHT_HALF + (map.MapDiagonalTiles / 2 * TileSet.TILE_HEIGHT_HALF));
        }

        private void ProgressChanged(object sender, ProgressReportArgs e) => backgroundWorker.ReportProgress(progressTrack + (int)((double)e.Progress / e.Max * 1000));

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    backgroundWorker?.Dispose();
                    btl?.Dispose();
                    gtl?.Dispose();
                    workReporter.ReportWork -= ProgressChanged;
                }
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
