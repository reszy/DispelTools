using System.Collections.Generic;
using System.Drawing;

namespace DispelTools.Viewers.MapViewer
{
    public class MapModel
    {
        private readonly MapCell[,] cells;
        private readonly List<SpriteData> sprites;

        public int Width { get; }
        public int Height { get; }

        public MapModel(int width, int height)
        {
            sprites = new List<SpriteData>();
            cells = new MapCell[width, height];
            Width = width;
            Height = height;
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
            public SpriteData(int id, Point position)
            {
                Id = id;
                Position = position;
            }

            public int Id { get; set; }
            public Point Position { get; set; }
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

        public void SetSprite(int id, int x, int y) => sprites.Add(new SpriteData(id, new Point(x, y)));
        public List<SpriteData> GetSprites() => sprites;
    }
}
