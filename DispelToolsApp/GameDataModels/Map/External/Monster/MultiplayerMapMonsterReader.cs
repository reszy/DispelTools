using DispelTools.Common;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.GameDataModels.Map.External.Monster
{
    class MultiplayerMapMonsterReader : IExternalEntitiesReader
    {
        private readonly IFileSystem fs;

        public MultiplayerMapMonsterReader() : this(new FileSystem()) { }
        public MultiplayerMapMonsterReader(IFileSystem fs)
        {
            this.fs = fs;
        }

        public List<MapExternalObject> GetObjects(string gamePath, string mapFilePath, MapContainer mapContainer)
        {
            int mapPixelHeight = mapContainer.Model.MapSizeInPixels.Height;
            int mapNonOccludedStartY = mapContainer.Model.MapNonOccludedStart.Y;

            var filename = fs.Path.GetFileNameWithoutExtension(mapFilePath) + ".mon";
            var filePath = fs.Path.GetDirectoryName(mapFilePath)  + "\\" + filename;

            var output = new List<MapExternalObject>();
            if (!fs.File.Exists(filePath)) return output;

            var file = fs.File.ReadAllLines(filePath);

            var InfoRelativePath = @"\Monster.ini";
            var names = LoadInfo($"{gamePath}{InfoRelativePath}", 2);
            var directory = $"{gamePath}\\MonsterInGame";

            MapExternalSpriteCache spriteCache = new MapExternalSpriteCache(directory, names);

            for (int i = 1; i < file.Length; i++)
            {
                if (file[i].StartsWith(";"))
                {
                    continue;
                }
                var line = file[i].Split(',');
                if (line.Length == 8)
                {
                    var spriteInfo = new ReferenceFileReader.OnMapSpriteInfo(
                        int.Parse(line[2]),
                        int.Parse(line[3]),
                        int.Parse(line[0]),
                        int.Parse(line[1]),
                        3,
                        false
                        );

                    output.Add(
                        new MapExternalObject(
                            spriteInfo,
                            spriteCache.GetSpriteName(spriteInfo.SpriteId),
                            spriteCache.GetSpriteFrame(spriteInfo.SpriteId, spriteInfo.SpriteSequence),
                            mapPixelHeight,
                            mapNonOccludedStartY
                            )
                        );
                }
            }
            return output;
        }

        private Dictionary<int, string> LoadInfo(string path, int spriteNameColumnIndex)
        {
            try
            {
                return fs.File.ReadLines(path)
                    .Where(line => !line.StartsWith(";"))
                    .Select(line => line.Split(','))
                    .Where(fields => fields[1] != "null")
                    .ToDictionary(fields => int.Parse(fields[0]), fields => fields[spriteNameColumnIndex]);
            }
            catch (Exception e)
            {
                throw new ReadFileException(path, e);
            }
        }
    }
}
