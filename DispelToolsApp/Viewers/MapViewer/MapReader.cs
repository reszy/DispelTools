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
                ReadInternalSpriteInfo(file);
                ReadFifthBlock(file);
                ReadEventAndBtlgBlock(file);
                ReadTilesAndAccessBlock(file);

                map.GetSpritesData().Sort(new SpriteSorter());
                map.GetBtlData().Sort(new BtlSorter());
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

        private void ReadInternalSpriteInfo(BinaryReader file)
        {
            int count = file.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                int sprId = file.ReadInt32();
                file.ReadInt32();
                file.ReadInt32();
                int sprBottomRightX = file.ReadInt32();
                int sprBottomRightY = file.ReadInt32();
                int sprX = file.ReadInt32();
                int sprY = file.ReadInt32();
                if (sprId >= 0 && sprId < sprites.Count)
                {
                    int restOfFramesByteCount = (sprites[sprId].FrameCount - 1) * 6 * 4;
                    file.Skip(restOfFramesByteCount);
                    map.SetSprite(sprId, sprX, sprY, sprBottomRightX, sprBottomRightY);
                }
                else
                {
                    Metrics.Count(MetricFile.MapReadMetric, filename, "missedInternalSprite");
                }
            }
        }

        private void ReadFifthBlock(BinaryReader file)
        {
            int bundlesCount = file.ReadInt32();
            int number1 = file.ReadInt32();

            for (int i = 0; i < bundlesCount; i++)
            {
                ReadInfoChunk(file);
            }

            int backPos = 20;
            file.SetPosition(file.BaseStream.Position - backPos);
            int lastPos = 0;
            for (int i = 0; i < backPos; i++)
            {
                byte v = file.ReadByte();
                if (v == 1)
                {
                    lastPos = i;
                }
            }
            int toUndo = backPos - lastPos + 4;
            Metrics.Count(MetricFile.MapReadMetric, Path.GetFileName(filename), "ToUndo", toUndo);
            file.SetPosition(file.BaseStream.Position - toUndo);
        }

        private void ReadInfoChunk(BinaryReader file)
        {
            file.Skip(264);

            int s8 = file.ReadInt32();
            int s0_1 = file.ReadInt32();
            int s1 = file.ReadInt32();
            int s0_2 = file.ReadInt32();

            if (s8 != 8 && s0_1 != 0 && s0_2 != 0 && s1 != 1)
            {
                Metrics.Count(MetricFile.MapReadMetric, Path.GetFileName(filename), "WrongSequence");
            }

            int v1 = file.ReadInt32();
            int v2 = file.ReadInt32();
            int v3 = file.ReadInt32();
            int v4 = file.ReadInt32();
            int x = file.ReadInt32();
            int y = file.ReadInt32();
            int v7 = file.ReadInt32();
            int v8 = file.ReadInt32();

            int c1 = file.ReadInt32();
            int c2 = file.ReadInt32();
            int c3 = file.ReadInt32();

            int[] ids = new int[c3];
            for (int i = 0; i < c3; i++)
            {
                ids[i] = file.ReadInt16();
            }

            map.SetBtl(x, y, ids);

            file.Skip(84);

            file.Skip((c1 + c2 + c3) * 4);
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

            int imageWidth = generatorOptions.Occlusion ? map.OccludedMapSizeInPixels.Width : map.MapSizeInPixels.Width;
            int imageHeight = generatorOptions.Occlusion ? map.OccludedMapSizeInPixels.Height : map.MapSizeInPixels.Height;
            var mapImage = new DirectBitmap(imageWidth, imageHeight);

            int total = map.TiledMapSize.Width * map.TiledMapSize.Height;
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
                foreach (var spriteData in map.GetSpritesData())
                {
                    var sprite = sprites[spriteData.Id];
                    PlotSpriteOnBitmap(ref mapImage, sprite.GetFrame(0).Bitmap, spriteData.Position.X, spriteData.Position.Y);
                }
            }
            if (generatorOptions.BTL)
            {
                foreach (var btlData in map.GetBtlData())
                {
                    for (int i = 0; i < btlData.Size; i++)
                    {
                        var tile = btl[btlData.GetId(i)];
                        tile.PlotTileOnBitmap(ref mapImage, btlData.Position.X + 64, btlData.Position.Y + (i * TileSet.TILE_HEIGHT) - 32);
                    }
                }
            }
            return mapImage;
        }

        private class SpriteSorter : IComparer<MapModel.SpriteData>
        {
            public int Compare(MapModel.SpriteData a, MapModel.SpriteData b) => (a.Position.Y + a.BottomRightPosition.Y) - (b.Position.Y + b.BottomRightPosition.Y);
        }

        private class BtlSorter : IComparer<MapModel.BtlData>
        {
            public int Compare(MapModel.BtlData a, MapModel.BtlData b) => (a.Position.Y + a.Size * TileSet.TILE_HEIGHT) - (b.Position.Y + b.Size * TileSet.TILE_HEIGHT);
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
                    foreach (var sprite in sprites)
                    {
                        sprite.Dispose();
                    }
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
