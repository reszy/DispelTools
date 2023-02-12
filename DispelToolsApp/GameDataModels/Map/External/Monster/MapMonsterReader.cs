namespace DispelTools.GameDataModels.Map.External.Monster
{
    internal class MapMonsterReader : ReferenceFileReader
    {
        protected override string InfoRelativePath { get; } = @"\Monster.ini";

        protected override int InfoSpriteNameColumnIndex { get; } = 2;

        protected override string SpriteDirectoryName { get; } = "MonsterInGame";

        protected override string ReferencePrefix { get; } = "Mon";

        internal override DataEditor.MapperDefinition MapperDefinition { get; } = new DataEditor.Mappers.MonRefMapper();

        internal override string[] ValuesMapping { get; } = { "posX", "posY", "monId", "fileId" };

        protected override OnMapSpriteInfo ProcessItem(DataEditor.Data.PropertyItem item, object[] values)
        {
            return new OnMapSpriteInfo(
                (int)values[0],
                (int)values[1],
                (int)values[3],
                (int)values[2],
                3,
                false
                );
        }

    }
}
