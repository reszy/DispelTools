﻿using System.Drawing;

namespace DispelTools.GameDataModels.Map
{
    public class TiledObjectsInfo : IInterlacedOrderObject
    {
        public int Order { get; }
        public int Size { get; }
        public Point Position { get; }

        private readonly int[] ids;

        public TiledObjectsInfo(int order, int x, int y, int[] ids)
        {
            Order = order;
            Size = ids.Length;
            this.ids = ids;
            Position = new Point(x, y);
        }

        public int GetId(int i) => ids[i];

        public int PositionOrder => Position.Y + Size * TileSet.TILE_HEIGHT;

        public int TypeOrder => 0;

    }
}
