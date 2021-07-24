using System.Drawing;

namespace DispelTools.GameDataModels.Map
{
    public class TiledObjectsInfo
    {
        public int Size { get; }
        public Point Position { get; }
        private readonly int[] ids;

        public TiledObjectsInfo(int x, int y, int[] ids)
        {
            Size = ids.Length;
            this.ids = ids;
            Position = new Point(x, y);
        }

        public int GetId(int i) => ids[i];
    }
}
