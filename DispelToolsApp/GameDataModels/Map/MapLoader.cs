using DispelTools.Common;
using DispelTools.Common.DataProcessing;
using DispelTools.GameDataModels.Map.External.Extra;
using DispelTools.GameDataModels.Map.External.Monster;
using DispelTools.GameDataModels.Map.External.Npc;
using DispelTools.GameDataModels.Map.Generator;
using DispelTools.GameDataModels.Map.Reader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.GameDataModels.Map
{
    public partial class MapContainer
    {
        public static MapContainer ReadMap(IFileSystem fs, string mapFile, WorkReporter workReporter)
        {
            var mapReader = new MapReader(mapFile, workReporter);

            var map = mapReader.ReadMap(false);
            map.SourceDirectory = fs.Path.GetDirectoryName(mapFile) ?? string.Empty;

            return map;
        }

        public void LoadGtlTiles(IFileSystem fs, WorkReporter workReporter)
        {
            Gtl = TileSet.LoadTileSet($"{SourceDirectory}\\{MapName}.gtl", workReporter);
        }
        public void LoadBtlTiles(IFileSystem fs, WorkReporter workReporter)
        {
            Btl = TileSet.LoadTileSet($"{SourceDirectory}\\{MapName}.btl", workReporter);
        }

        public void LoadExternal(IFileSystem fs, WorkReporter workReporter)
        {
            workReporter.SetTotal(4);
            AddEntities(ExtraEntities, new MapExtraReader(), fs, workReporter);
            workReporter.ReportProgress(1);
            AddEntities(MonsterEntities, new MapMonsterReader(), fs, workReporter);
            AddEntities(MonsterEntities, new MultiplayerMapMonsterReader(), fs, workReporter);
            workReporter.ReportProgress(2);
            AddEntities(NpcEntities, new MapNpcReader(), fs, workReporter);
            workReporter.ReportProgress(3);
        }

        private void AddEntities(List<External.MapExternalObject> target, External.IExternalEntitiesReader reader, IFileSystem fs, WorkReporter workReporter)
        {
            try
            {
                target.AddRange(reader.GetObjects(Settings.GameRootDir, $"{SourceDirectory}\\{MapName}.map", this));
            }
            catch (ReadFileException e)
            {
                workReporter.ReportWarning(e.Message);
                
            }
        }
    }
}
