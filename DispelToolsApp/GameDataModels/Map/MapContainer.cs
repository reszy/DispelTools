using DispelTools.ImageProcessing.Sprite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DispelTools.GameDataModels.Map
{
    public class MapContainer : IDisposable
    {
        private bool disposedValue;

        public string MapName { get; }
        public MapModel Model { get; }
        public List<SpriteSequence> InternalSprites { get; }
        public TileSet Gtl { get; set; }
        public TileSet Btl { get; set; }

        public MapContainer(string mapName, MapModel model, List<SpriteSequence> sprites)
        {
            MapName = mapName;
            Model = model;
            InternalSprites = sprites;
        }

        public string GetStats()
        {
            var sb = new StringBuilder();

            sb.AppendLine("--Map Model--");
            sb.Append("Height: ");
            sb.Append(Model.TiledMapSize.Height);
            sb.AppendLine();
            sb.Append("Width: ");
            sb.Append(Model.TiledMapSize.Width);
            sb.AppendLine();
            sb.Append("Sprites included: ");
            sb.Append(InternalSprites.Count);
            sb.AppendLine();
            sb.Append("Sprites on map: ");
            sb.Append(Model.InternalSpriteInfos.Count);
            sb.AppendLine();

            sb.AppendLine();
            sb.AppendLine("--Tiles--");
            sb.Append("GTL: ");
            sb.Append(Gtl.Count);
            sb.AppendLine();
            sb.Append("BTL: ");
            sb.Append(Btl.Count);
            sb.AppendLine();

            return sb.ToString();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var sprite in InternalSprites)
                    {
                        sprite.Dispose();
                    }
                    InternalSprites.Clear();
                    Gtl.Dispose();
                    Btl.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
