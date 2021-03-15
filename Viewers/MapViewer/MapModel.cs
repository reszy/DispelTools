namespace DispelTools.Viewers.MapViewer
{
    public class MapModel
    {
        private readonly MapCell[,] cells;

        public MapModel(int width, int height)
        {
            cells = new MapCell[width, height];
        }

        private struct MapCell
        {
            public int Gtl { get; set; }
            public bool Collision { get; set; }
            public int Btl { get; set; }
            public int Bldg { get; set; }
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
    }
}
