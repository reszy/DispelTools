using System.Collections.Generic;
using System.Drawing;

namespace DispelTools.Viewers.MapViewer
{
    public class MapModel
    {
        private static readonly int MAP_CHUNK_SIZE = 25;

        private readonly MapCell[,] cells;
        private readonly List<SpriteData> sprites;

        public Size MapSize { get; }
        public Size TiledMapSize { get; }
        public int MapDiagonalTiles { get; }
        public Size MapSizeInPixels { get; }
        public Size OccludedMapSizeInPixels { get; }
        public Point MapNonOccludedStart { get; }

        public MapModel(int width, int height)
        {
            sprites = new List<SpriteData>();
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

        private struct MapCell
        {
            public int Gtl { get; set; }
            public bool Collision { get; set; }
            public int Btl { get; set; }
            public int Bldg { get; set; }
        }
        public struct SpriteData
        {
            public SpriteData(int id, Point position, Point bottomRightPosition)
            {
                Id = id;
                Position = position;
                BottomRightPosition = bottomRightPosition;
            }

            public int Id { get; set; }
            public Point Position { get; set; }
            public Point BottomRightPosition { get; set; }
        }

        public void SetIds(int x, int y, int gtlId, int btlId)
        {
            cells[x, y].Btl = btlId;
            cells[x, y].Gtl = gtlId;
        }
        public void SetCollision(int x, int y, bool collision) => cells[x, y].Collision = collision;
        public void SetBldg(int x, int y, int bldg) => cells[x, y].Bldg = bldg;

        public int GetGtlId(int x, int y) => cells[x, y].Gtl;
        public int GetBtlId(int x, int y) => cells[x, y].Btl;
        public bool GetCollision(int x, int y) => cells[x, y].Collision;
        public int GetBldg(int x, int y) => cells[x, y].Bldg;

        public void SetSprite(int id, int x, int y, int bottomRightX, int bottomRightY) => sprites.Add(new SpriteData(id, new Point(x, y), new Point(bottomRightX, bottomRightY)));
        public List<SpriteData> GetSpritesData() => sprites;
    }
}
