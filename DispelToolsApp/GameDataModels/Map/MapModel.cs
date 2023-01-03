using System.Collections.Generic;
using System.Drawing;

namespace DispelTools.GameDataModels.Map
{
    public class MapModel
    {
        private static readonly int MAP_CHUNK_SIZE = 25;

        private readonly MapCell[,] cells;
        public List<InternalSpriteInfo> InternalSpriteInfos { get; }
        public List<TiledObjectsInfo> TiledObjectInfos { get; }
        private struct MapCell
        {
            public int Gtl { get; set; }
            public bool Collision { get; set; }
            public int RoofBtl { get; set; }
            public short EventId { get; set; }
        }
        public Size MapSize { get; }
        public Size TiledMapSize { get; }
        public int MapDiagonalTiles { get; }
        public Size MapSizeInPixels { get; }
        public Size OccludedMapSizeInPixels { get; }
        public Point MapNonOccludedStart { get; }

        public MapModel(int width, int height)
        {
            InternalSpriteInfos = new List<InternalSpriteInfo>();
            TiledObjectInfos = new List<TiledObjectsInfo>();
            MapSize = new Size(width, height);
            TiledMapSize = new Size(width * MAP_CHUNK_SIZE - 1, height * MAP_CHUNK_SIZE - 1);
            MapDiagonalTiles = TiledMapSize.Width + TiledMapSize.Height;

            cells = new MapCell[TiledMapSize.Width, TiledMapSize.Height];

            int diagonal = width + height;
            MapSizeInPixels = new Size(
                diagonal * MAP_CHUNK_SIZE * TileSet.TILE_HORIZONTAL_OFFSET_HALF,
                diagonal * MAP_CHUNK_SIZE * TileSet.TILE_HEIGHT_HALF);

            double xAspect = 0.3;
            double yAspect = 0.2;

            int compensateX = (width % 2 == 0) ? TileSet.TILE_HORIZONTAL_OFFSET_HALF : 0;
            int compensateY = (width % 2 == 0) ? 0 : TileSet.TILE_HEIGHT_HALF;
            MapNonOccludedStart = new Point(
                (int)(xAspect * MapSizeInPixels.Width - compensateX),
                (int)(yAspect * MapSizeInPixels.Height - compensateY));

            OccludedMapSizeInPixels = new Size(
                MapSizeInPixels.Width - (MapNonOccludedStart.X * 2),
                MapSizeInPixels.Height - (MapNonOccludedStart.Y * 2)
                ); ;
        }

        public Point TranslateImageToMapPosition(Point position)
        {
            int sx = position.X;
            int sy = position.Y;

            int mx = (sx / TileSet.TILE_WIDTH_HALF - (sy - (MapDiagonalTiles / 2 * TileSet.TILE_HEIGHT_HALF)) / TileSet.TILE_HEIGHT_HALF) / 2;
            int my = (sy - (MapDiagonalTiles / 2 * TileSet.TILE_HEIGHT_HALF)) / TileSet.TILE_HEIGHT_HALF + mx;

            return new Point(mx, my);
        }
        public void SetGtl(int x, int y, int id) => cells[x, y].Gtl = id;
        public void SetRoofBtl(int x, int y, int id) => cells[x, y].RoofBtl = id;
        public void SetCollision(int x, int y, bool collision) => cells[x, y].Collision = collision;
        public void SetEventId(int x, int y, short eventId) => cells[x, y].EventId = eventId;

        public int GetGtlId(int x, int y) => cells[x, y].Gtl;
        public int GetRoofBtlId(int x, int y) => cells[x, y].RoofBtl;
        public bool GetCollision(int x, int y) => cells[x, y].Collision;
        public short GetEventId(int x, int y) => cells[x, y].EventId;

        public void AddTiledObject(int x, int y, int[] ids) => TiledObjectInfos.Add(new TiledObjectsInfo(x, y, ids));
        public void AddSriteInfo(int id, int x, int y, int bottomRightX, int bottomRightY) => InternalSpriteInfos.Add(new InternalSpriteInfo(id, new Point(x, y), new Point(bottomRightX, bottomRightY)));
    }
}
