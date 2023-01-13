using DispelTools.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.GameDataModels.Map.External
{
    internal abstract class ReferenceFileReader
    {
        protected abstract string InfoRelativePath { get; }
        protected abstract int InfoSpriteNameColumnIndex { get; }
        protected abstract string SpriteDirectoryName { get; }
        protected abstract string ReferencePrefix { get; }
        internal abstract DataEditor.Mapper Mapper { get; }
        internal abstract string[] ValuesMapping { get; }

        public List<MapExternalObject> GetObjects(string gamePath, string mapName, MapContainer mapContainer)
        {
            int mapPixelHeight = mapContainer.Model.MapSizeInPixels.Height;
            int mapNonOccludedStartY = mapContainer.Model.MapNonOccludedStart.Y;

            var names = LoadInfo($"{gamePath}{InfoRelativePath}", InfoSpriteNameColumnIndex);

            List<MapExternalObject> objects = new List<MapExternalObject>();
            var directory = $"{gamePath}\\{SpriteDirectoryName}";
            var mapRefPath = $"{directory}\\{ReferencePrefix}{mapName}.ref";

            if (!File.Exists(mapRefPath)) return objects;

            MapExternalSpriteCache spriteCache = new MapExternalSpriteCache(directory, names);

            List<Components.CustomPropertyGridControl.PropertyItem> items = Mapper.ReadFile(mapRefPath);
            var fieldMapping = Mapper.CreateMapping(ValuesMapping);

            foreach (var item in items)
            {
                var values = fieldMapping.Convert(item);
                var processed = ProcessItem(item, values);

                objects.Add(new MapExternalObject(
                    processed,
                    spriteCache.GetSpriteName(processed.SpriteId),
                    spriteCache.GetSpriteFrame(processed.SpriteId, processed.SpriteSequence),
                    mapPixelHeight,
                    mapNonOccludedStartY));
            }

            return objects;
        }

        protected abstract OnMapSpriteInfo ProcessItem(Components.CustomPropertyGridControl.PropertyItem item, object[] values);

        public class OnMapSpriteInfo
        {
            public OnMapSpriteInfo(int x, int y, int dbId, int spriteId, int spriteSequence, bool flip)
            {
                X = x;
                Y = y;
                DbId = dbId;
                SpriteId = spriteId;
                SpriteSequence = spriteSequence;
                Flip = flip;
            }
            public int X { get; }
            public int Y { get; }
            public int DbId { get; }
            public int SpriteId { get; }
            public int SpriteSequence { get; }
            public bool Flip { get; }
        }

        private Dictionary<int, string> LoadInfo(string path, int spriteNameColumnIndex)
        {
            try
            {
                return File.ReadLines(path)
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
