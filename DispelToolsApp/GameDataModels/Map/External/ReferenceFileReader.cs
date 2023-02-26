using DispelTools.Common;
using DispelTools.Common.DataProcessing;
using DispelTools.DataEditor;
using System.IO.Abstractions;

namespace DispelTools.GameDataModels.Map.External
{
    internal abstract class ReferenceFileReader : IExternalEntitiesReader
    {
        protected abstract string InfoRelativePath { get; }
        protected abstract int InfoSpriteNameColumnIndex { get; }
        protected abstract string SpriteDirectoryName { get; }
        protected abstract string ReferencePrefix { get; }
        internal abstract DataEditor.MapperDefinition MapperDefinition { get; }
        internal abstract string[] ValuesMapping { get; }

        public List<MapExternalObject> GetObjects(string gamePath, string mapFilePath, MapContainer mapContainer, WorkReporter workReporter)
        {
            var mapName = Path.GetFileNameWithoutExtension(mapFilePath);
            int mapPixelHeight = mapContainer.Model.MapSizeInPixels.Height;
            int mapNonOccludedStartY = mapContainer.Model.MapNonOccludedStart.Y;

            var objects = new List<MapExternalObject>();
            var directory = $"{gamePath}\\{SpriteDirectoryName}";
            var mapRefPath = $"{directory}\\{ReferencePrefix}{mapName}.ref";

            if (!File.Exists(mapRefPath)) return objects;

            var names = LoadInfo($"{gamePath}{InfoRelativePath}", InfoSpriteNameColumnIndex);

            var spriteCache = new MapExternalSpriteCache(directory, names);

            var loader = new DispelTools.DataEditor.SimpleDataLoader(new FileSystem(), MapperDefinition);
            var dataContainer = loader.LoadData(mapRefPath, workReporter);
            var fieldMapping = new FieldMapping(MapperDefinition, ValuesMapping);

            for (int i = 0; i < dataContainer.Count; i++)
            {
                var values = fieldMapping.Convert(dataContainer[i]);
                var processed = ProcessItem(dataContainer[i], values);

                objects.Add(new MapExternalObject(
                    processed,
                    spriteCache.GetSpriteName(processed.SpriteId),
                    spriteCache.GetSpriteFrame(processed.SpriteId, processed.SpriteSequence),
                    mapPixelHeight,
                    mapNonOccludedStartY));
            }

            return objects;
        }

        protected abstract OnMapSpriteInfo ProcessItem(DataEditor.Data.DataItem item, object[] values);

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
