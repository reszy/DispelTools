using DispelTools.GameDataModels.Sprite;

namespace DispelTools.GameDataModels.Map.External
{
    public class MapExternalObject : IInterlacedOrderObject
    {
        internal MapExternalObject(ReferenceFileReader.OnMapSpriteInfo info, string spriteName, SpriteFrame graphic, int mapPixelHeight, int mapNonOccludedStartY)
        {
            SpriteName = spriteName;
            Graphic = graphic;
            DbId = info.DbId;
            X = info.X;
            Y = info.Y;
            PositionOrder = -(X * TileSet.TILE_HEIGHT_HALF) + (Y * TileSet.TILE_HEIGHT_HALF) + (mapPixelHeight / 2) - mapNonOccludedStartY + TileSet.TILE_HEIGHT_HALF;
            Flip = info.Flip;
        }

        public string SpriteName { get; }
        public SpriteFrame Graphic { get; }
        public int DbId { get; }
        public int X { get; }
        public int Y { get; }
        public bool Flip { get; }

        public int PositionOrder { get; }

        public int Order => X;

        public int TypeOrder => 2;
    }
}
