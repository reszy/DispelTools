using DispelTools.GameDataModels.Sprite;
using System;
using System.Collections.Generic;
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
        public List<External.MapExternalObject> Entities { get; }

        public MapContainer(string mapName, MapModel model, List<SpriteSequence> sprites)
        {
            MapName = mapName;
            Model = model;
            InternalSprites = sprites;
            Entities = new List<External.MapExternalObject>();
        }

        public string GetStats()
        {
            var sb = new StringBuilder();

            sb.AppendLine("--Map Model--");
            sb.Append("Name: ");
            sb.Append(MapName);
            sb.AppendLine();
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

        public System.Drawing.Point TranslateImageToMapCoords(int x, int y, bool ocluded)
        {
            if(ocluded)
            {
                x += Model.MapNonOccludedStart.X;
                y += Model.MapNonOccludedStart.Y;
            }
            //double XM = 0;//Map x
            //double YM = 0;//Map y
            //double g = TileSet.TILE_HORIZONTAL_OFFSET_HALF;
            //double j = TileSet.TILE_HEIGHT_HALF;
            //double k = Model.MapDiagonalTiles;
            //double a = (2 * x / g - 2 * y / j + k) / 4;
            //double b = (x - (((2 * x) / g - (2 * y) / j + k) / 4) * g) / g;

            //return new System.Drawing.Point(
            //    (int)Math.Floor(a)
            //    ,
            //    (int)Math.Floor(b)
            //    );

            var tileSideLength = Math.Sqrt(TileSet.TILE_HEIGHT_HALF * TileSet.TILE_HEIGHT_HALF + TileSet.TILE_HORIZONTAL_OFFSET_HALF * TileSet.TILE_HORIZONTAL_OFFSET_HALF);

            var h = Model.MapSizeInPixels.Height;

            double xLineOffset = (h / 2);
            double yLineOffset = (h / 2);


            var pointCrossingYLine = crossWithLineY(xLineCalculateOffset(x, y));
            var pointCrossingXLine = crossWithLineX(yLineCalculateOffset(x, y));


            return new System.Drawing.Point(
                (int)Math.Floor(pointsDistance(pointCrossingXLine.x, pointCrossingXLine.y, x, y) / tileSideLength)
                ,
                (int)Math.Floor(pointsDistance(pointCrossingYLine.x, pointCrossingYLine.y, x, y) / tileSideLength)
                );

            double xLineCalculateOffset(double fx, double fy) => fy - (+0.5 * fx);
            double yLineCalculateOffset(double fx, double fy) => fy - (-0.5 * fx);
            (double x, double y) crossWithLineX(double lineOfPointOffset)
            {
                // y = (-0.5)*x + lineOfPointOffset
                // y = 0.5*x + xLineOffset

                // y = (-0.5)*x + lineOfPointOffset
                // (-0.5)*x + lineOfPointOffset = 0.5*x + xLineOffset

                // y = (-0.5)*x + lineOfPointOffset
                // (-0.5)*x - 0.5*x = xLineOffset - lineOfPointOffset

                // y = (-0.5)*x + lineOfPointOffset
                // -x= xLineOffset - lineOfPointOffset

                // y = (-0.5)*(-xLineOffset + lineOfPointOffset) + lineOfPointOffset
                // x= -xLineOffset + lineOfPointOffset

                var tmpX = lineOfPointOffset - xLineOffset;
                var tmpY = (-0.5) * tmpX + lineOfPointOffset;
                return (tmpX, tmpY);
            };
            (double x, double y) crossWithLineY(double lineOfPointOffset)
            {
                // y = 0.5*x + lineOfPointOffset
                // y = (-0.5)*x + yLineOffset

                // y = 0.5*x + lineOfPointOffset
                // 0.5*x + lineOfPointOffset = (-0.5)*x + yLineOffset

                // y = 0.5*x + lineOfPointOffset
                // 0.5*x + 0.5*x = yLineOffset - lineOfPointOffset

                // y = 0.5*x + lineOfPointOffset
                // x= yLineOffset - lineOfPointOffset

                // y = 0.5*(yLineOffset - lineOfPointOffset) + lineOfPointOffset
                // x= yLineOffset - lineOfPointOffset

                var tmpX = yLineOffset - lineOfPointOffset;
                var tmpY = 0.5 * tmpX + lineOfPointOffset;
                return (tmpX, tmpY);
            };
            double pointsDistance(double p1x, double p1y, double p2x, double p2y)
            {
                var fx = p1x - p2x;
                var fy = p1y - p2y;
                return Math.Sqrt(fx * fx + fy * fy);
            }
            //
            //
            // xM = a = Map x
            // yM = b = Map y
            //return new Point(
            //       (x + y) * TileSet.TILE_HORIZONTAL_OFFSET_HALF,
            //       (-x + y) * TileSet.TILE_HEIGHT_HALF + (Model.MapDiagonalTiles / 2 * TileSet.TILE_HEIGHT_HALF));
            //
            // x = (a + b) * g
            // x = a * g + b * g
            // x - a * g = b * g
            // b = (x - a * g) / g
            // a = (x - b * g) / g
            // 
            // 
            // y = (-a + b) * j + (k / 2 * j)
            // 
            // y = (-xM + ((x - xM * g) / g)) * j + (k / 2 * j)
            // 
            // a = xM
            // b = yM
            // j = TileSet.TILE_HEIGHT_HALF
            // k = Model.MapDiagonalTiles
            // g = TileSet.TILE_HORIZONTAL_OFFSET_HALF
            // 
            // b = (x-a*g)/g
            // y  = (-a + ((x - a * g) / g))*j+(k/2*j)
            // a  = ((2*x)/g-(2*y)/j+k)/4
            // b = (x-(((2*x)/g-(2*y)/j+k)/4)*g)/g



            //xLine
            //y=0.5x + (h/2)
            //xD = abs(0.5 * x + y + (h/2)) / sqrt(0.25)
            //yLine
            //y=-0.5x + (h/2)
            //yD = abs(-0.5 * x + y + (h / 2)) / sqrt(0.25)
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    InternalSprites.Clear();
                    Gtl?.Dispose();
                    Btl?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
