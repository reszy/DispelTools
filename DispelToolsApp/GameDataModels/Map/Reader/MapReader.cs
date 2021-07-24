using DispelTools.Common;
using DispelTools.DebugTools.MetricTools;
using DispelTools.ImageProcessing.Sprite;
using System;
using System.Collections.Generic;
using System.IO;

namespace DispelTools.GameDataModels.Map.Reader
{
    public partial class MapReader
    {
        private readonly string filename;
        private readonly string mapName;

        private readonly WorkReporter workReporter;

        private MapModel map;
        private readonly List<SpriteSequence> sprites = new List<SpriteSequence>();

        public MapReader(string filename, WorkReporter workReporter)
        {
            this.filename = filename;
            this.workReporter = workReporter;
            mapName = Path.GetFileNameWithoutExtension(filename);
        }

        public MapContainer ReadMap(bool skipImages)
        {
            workReporter.SetTotal(9);
            using (var file = new BinaryReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
            {
                workReporter.ReportProgress(0);

                int width = file.ReadInt32();
                int height = file.ReadInt32();
                map = new MapModel(width, height);

                ReadFirstBlock(file);
                workReporter.ReportProgress(1);

                ReadSecondBlock(file);
                workReporter.ReportProgress(2);

                ReadSpritesBlock(file, skipImages);
                workReporter.ReportProgress(3);

                ReadInternalSpriteInfo(file);
                workReporter.ReportProgress(4);

                ReadTiledObjectsBlock(file);
                workReporter.ReportProgress(5);

                ReadEventAndBtlgBlock(file);
                workReporter.ReportProgress(6);

                ReadTilesAndAccessBlock(file);
                workReporter.ReportProgress(7);

                ReadRoofTiles(file);
                workReporter.ReportProgress(8);

                map.InternalSpriteInfos.Sort(new SpriteSorter());
                map.TiledObjectInfos.Sort(new BtlSorter());

                workReporter.ReportProgress(9);
            }
            return new MapContainer(mapName, map, sprites);
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

        private void ReadSpritesBlock(BinaryReader file, bool skipImages)
        {
            int spritesCount = file.ReadInt32();
            var spriteLoader = new SpriteLoader(file, filename);
            for (int i = 0; i < spritesCount; i++)
            {
                int imageStamp = file.ReadInt32();
                int imageOffset = imageStamp == 6 ? 1904 : (imageStamp == 9 ? 2996 : throw new NotImplementedException($"Unexpected imageStamp {imageStamp}"));
                file.Skip(264);
                if (skipImages)
                {
                    spriteLoader.SkipSequence();
                }
                else
                {
                    sprites.Add(spriteLoader.LoadSequence());
                }
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
                    map.AddSriteInfo(sprId, sprX, sprY, sprBottomRightX, sprBottomRightY);
                }
                else
                {
                    Metrics.Count(MetricFile.MapReadMetric, filename, "missedInternalSprite");
                }
            }
        }

        private void ReadTiledObjectsBlock(BinaryReader file)
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
            int toUndo = backPos - lastPos - 4;
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

            map.AddTiledObject(x, y, ids);

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
                    map.SetGtl(x, y, gtlBytes);
                    map.SetCollision(x, y, (bytes & 0x1) == 1);
                }
            }
        }
        private void ReadRoofTiles(BinaryReader file)
        {
            if (file.BaseStream.Length >= file.BaseStream.Position + map.TiledMapSize.Height * map.TiledMapSize.Width * 4)
            {
                for (int y = 0; y < map.TiledMapSize.Height; y++)
                {
                    for (int x = 0; x < map.TiledMapSize.Width; x++)
                    {
                        int bytes = file.ReadInt32();
                        int btlBytes = bytes;
                        map.SetRoofBtl(x, y, btlBytes);
                    }
                }
            }
        }



        private class SpriteSorter : IComparer<InternalSpriteInfo>
        {
            public int Compare(InternalSpriteInfo a, InternalSpriteInfo b) => (a.Position.Y + a.BottomRightPosition.Y) - (b.Position.Y + b.BottomRightPosition.Y);
        }

        private class BtlSorter : IComparer<TiledObjectsInfo>
        {
            public int Compare(TiledObjectsInfo a, TiledObjectsInfo b) => (a.Position.Y + a.Size * TileSet.TILE_HEIGHT) - (b.Position.Y + b.Size * TileSet.TILE_HEIGHT);
        }
    }
}
