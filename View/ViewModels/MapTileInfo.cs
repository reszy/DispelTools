using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace View.ViewModels
{
    internal class MapTileInfo
    {
        public MapTileInfo(string type, int id, string spriteName, int spriteId)
        {
            Type = type;
            Id = id;
            this.SpriteName = spriteName;
            SpriteId = spriteId;
        }

        public string Type { get; }
        public int Id { get; }
        public string SpriteName { get; }
        public int SpriteId { get; }
    }
}
