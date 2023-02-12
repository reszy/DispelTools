namespace DispelTools.GameDataModels.Map.External.Extra
{
    internal class MapExtraReader : ReferenceFileReader
    {
        protected override string InfoRelativePath { get; } = @"\Extra.ini";

        protected override int InfoSpriteNameColumnIndex { get; } = 1;

        protected override string SpriteDirectoryName { get; } = "ExtraInGame";

        protected override string ReferencePrefix { get; } = "Ext";

        internal override DataEditor.MapperDefinition MapperDefinition { get; } = new DataEditor.Mappers.ExtRefMapper();

        internal override string[] ValuesMapping { get; } = { "xPos", "yPos", "ExtId", "rotation", "type", "closed", "number in file" };

        protected override OnMapSpriteInfo ProcessItem(DataEditor.Data.PropertyItem item, object[] values)
        {
            var rotation = (byte)values[3];
            var type = (byte)values[4];
            var closed = (int)values[5];

            int sequence = rotation;
            if (type == 0) sequence = 2 * closed + rotation;

            return new OnMapSpriteInfo(
                (int)values[0],
                (int)values[1],
                (byte)values[6],
                (byte)values[2],
                sequence,
                false
                );
        }

    }
}
