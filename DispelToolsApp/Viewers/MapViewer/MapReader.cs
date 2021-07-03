using DispelTools.Common;
using DispelTools.ImageProcessing.Sprite;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using static DispelTools.Viewers.MapViewer.MapReader.WorkReporter;

namespace DispelTools.Viewers.MapViewer
{
    public class MapReader : IDisposable
    {
        private readonly string filename;
        private readonly int fileOffset;
        private int width;
        private int height;
        private readonly WorkReporter workReporter = new WorkReporter();
        private readonly BackgroundWorker backgroundWorker;
        private MapModel map;
        private TileSet btl;
        private TileSet gtl;
        private int progressTrack = 0;

        private bool knownOffset;

        private bool mapLoaded = false;
        private bool tilesLoaded = false;
        private bool disposedValue;

        public bool GTL { get; set; } = true;
        public bool Collisions { get; set; } = false;
        public bool BTL { get; set; } = false;
        public bool BLDG { get; set; } = false;

        public TileSet Btl => btl;
        public TileSet Gtl => gtl;

        public MapReader(string filename, int fileOffset, BackgroundWorker backgroundWorker)
        {
            this.filename = filename;
            this.fileOffset = fileOffset;
            this.backgroundWorker = backgroundWorker;
            map = null;
            backgroundWorker.WorkerReportsProgress = true;
            workReporter.ReportWork += ProgressChanged;
            knownOffset = true;
        }
        public MapReader(string filename)
        {
            this.filename = filename;
            knownOffset = false;
            map = null;
        }

        private void ProgressChanged(object sender, ProgressReportArgs e) => backgroundWorker.ReportProgress(progressTrack + (int)((double)e.Progress / e.Max * 1000));

        private void ReadMap()
        {
            using (var file = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                file.BaseStream.Seek(fileOffset, SeekOrigin.Begin);
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int bytes = file.ReadInt32();
                        int gtlBytes = bytes >> 10;
                        map.SetIds(x, y, gtlBytes, 0);
                        map.SetCollision(x, y, (bytes & 0x1) == 1);
                    }
                }
            }
            mapLoaded = true;
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

        private DirectBitmap CreateImageOfMap()
        {
            progressTrack = 2000;
            int imageWidth = (height + width) * (TileSet.TILE_WIDTH / 2) + TileSet.TILE_WIDTH;
            int imageHeight = (height + width) * (TileSet.TILE_HEIGHT / 2) + (TileSet.TILE_HEIGHT / 2);
            var mapImage = new DirectBitmap(imageWidth, imageHeight);
            int total = height * width;
            backgroundWorker.ReportProgress(progressTrack, "Generating map...");
            workReporter.ReportProgress(0, total);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var tile = gtl[map.GetGtlId(x, y)];
                    if (Collisions && map.GetCollision(x, y))
                    {
                        tile = tile.MixColor(Color.Red, 128);
                    }
                    if (map.GetBldg(x, y) > 0)
                    {
                        tile = tile.MixColor(Color.Blue, 128);
                    }
                    var mapCoords = ConvertCoordsToMap(x, y);
                    tile.PlotTileOnBitmap(ref mapImage, mapCoords.X, mapCoords.Y);
                    workReporter.ReportProgress(x + y * width, total);
                }
            }
            return mapImage;
        }

        public DirectBitmap GenerateMap()
        {
            if (!knownOffset)
            {
                return null;
            }
            if (map == null)
            {
                using (var file = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    width = file.ReadInt32() * 25 - 1;
                    height = file.ReadInt32() * 25 - 1;
                    map = new MapModel(width, height);
                }
            }
            if (!mapLoaded)
            {
                ReadMap();
            }
            if (!tilesLoaded)
            {
                LoadTiles();
            }
            var mapImage = CreateImageOfMap();
            backgroundWorker.ReportProgress(3000, "Complete");
            return mapImage;
        }

        private Point ConvertCoordsToMap(int x, int y) => new Point((x + y) * (TileSet.TILE_WIDTH / 2), (-x + y) * (TileSet.TILE_HEIGHT / 2) + ((height + width) / 2 * (TileSet.TILE_HEIGHT / 2)));

        public class WorkReporter
        {
            public class ProgressReportArgs : EventArgs
            {
                public int Max { get; set; }
                public int Progress { get; set; }
            }
            public void ReportProgress(int progress, int max) => ReportWork?.Invoke(this, new ProgressReportArgs() { Progress = progress, Max = max });
            public delegate void ReportWorkHandler(object sender, ProgressReportArgs e);
            public event ReportWorkHandler ReportWork;
        }

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

        public void SeekMapTiles()
        {
            using (var reader = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                reader.BaseStream.Seek(8, SeekOrigin.Begin);//Set position after map dimensions
                int multiplier = reader.ReadInt32();
                int size = reader.ReadInt32();
                reader.BaseStream.Seek(8, SeekOrigin.Begin);
                reader.Skip(multiplier * size * 4);//skip unknown data

                size = reader.ReadInt32();
                reader.Skip(size * 2);

                int spritesCount = reader.ReadInt32();

                var spriteLoader = new SpriteLoader(reader, filename);
                for (int i = 0; i < spritesCount; i++)
                {
                    int imageStamp = reader.ReadInt32();
                    int imageOffset = imageStamp == 6 ? 1904 : (imageStamp == 9 ? 2996 : throw new NotImplementedException($"Unexpected imageStamp {imageStamp}"));
                    reader.Skip(264);
                    spriteLoader.SkipSequence();
                    reader.Skip(imageOffset);
                }

            }
        }
    }
}
