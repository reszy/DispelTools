using DispelTools.GameDataModels.Map;
using DispelTools.GameDataModels.Sprite;
using System.IO;
using System.Linq;
using System.Collections.Generic;
namespace DispelTools.GameDataModels.Map.External.Npc
{
    internal class MapNpcReader : ReferenceFileReader
    {
        protected override string InfoRelativePath { get; } = @"\Npc.ini";

        protected override int InfoSpriteNameColumnIndex { get; } = 1;

        protected override string SpriteDirectoryName { get; } = "NpcInGame";

        protected override string ReferencePrefix { get; } = "Npc";

        internal override DataEditor.Mapper Mapper { get; } = new DataEditor.Mappers.NpcRefMapper();

        internal override string[] ValuesMapping { get; } = { "goto1X", "goto1Y", "npcId", "lookingDirection", "id" };

        protected override OnMapSpriteInfo ProcessItem(Components.CustomPropertyGridControl.PropertyItem item, object[] values)
        {
            var direction = (int)values[3];
            return new OnMapSpriteInfo(
                (int)values[0],
                (int)values[1],
                (int)values[4],
                (int)values[2],
                direction > 4 ? 8 - direction : direction,
                direction > 4
                );
        }
    }
}
