using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DispelTools.Components;
using DispelTools.Components.PictureDisplay;
using DispelTools.GameDataModels.Map;

namespace DispelTools.Viewers.MapViewer
{
    internal class MapDisplayerController : IPictureDisplayerController
    {
        public bool EvenTiles { get; set; }

        public int RequiredTipWidth { get; private set; }
        private List<TileInfo> info = new List<TileInfo>();

        private readonly int TileInfoWidth = 130;

        public class TileInfo
        {
            public TileInfo(string type, int id, string spriteName)
            {
                Type = type;
                Id = id;
                this.spriteName = spriteName;
            }

            public string Type { get; }
            public int Id { get; }
            public string spriteName { get; }
        }

        public delegate void InfoRequestedHandler(Point imagePosition, List<TileInfo> info);
        public event InfoRequestedHandler InfoRequestedEvent;

        public void ImageReloaded(Image newImage)
        {

        }

        public void PixelSelected(ICoordsConverter sender, PixelSelectedArgs args)
        {
            info.Clear();
            InfoRequestedEvent?.Invoke(args.Position, info);
            RequiredTipWidth = info.Count * TileInfoWidth;
        }

        private bool IsCenterOfTile(Point point) => ((point.X & 1) == (point.Y & 1)) ^ EvenTiles;

        private Point GetClosestTileCenter(Point pointerCoords)
        {
            double yTileDistance = pointerCoords.Y / (double)TileSet.TILE_HEIGHT_HALF;
            double xTileDistance = pointerCoords.X / (double)TileSet.TILE_HORIZONTAL_OFFSET_HALF;


            bool flipToCorners = EvenTiles;

            int top = (int)Math.Floor(yTileDistance);
            int bottom = (int)Math.Ceiling(yTileDistance);
            int left = (int)Math.Floor(xTileDistance);
            int right = (int)Math.Ceiling(xTileDistance);

            var corners = new Point[]
            {
                    new Point(left, top),
                    new Point(right, top),
                    new Point(left, bottom),
                    new Point(right, bottom)
            };
            var possibleCoords = new HashSet<Point>();
            foreach (var corner in corners)
            {
                if (IsCenterOfTile(corner) ^ flipToCorners)
                {
                    possibleCoords.Add(new Point(corner.X * TileSet.TILE_HORIZONTAL_OFFSET_HALF, corner.Y * TileSet.TILE_HEIGHT_HALF));
                }
            }
            var bestCoords = Point.Empty;
            double? bestDistance = null;
            foreach (var coords in possibleCoords)
            {
                float xPow = (coords.X - pointerCoords.X) * (coords.X - pointerCoords.X);
                float yPow = (coords.Y - pointerCoords.Y) * (coords.Y - pointerCoords.Y);
                double distance = Math.Sqrt(xPow + yPow);
                if (bestDistance == null || distance < bestDistance)
                {
                    bestDistance = distance;
                    bestCoords = coords;
                }
            }
            return bestCoords;
        }

        public void DrawTip(Graphics g, Font font, Rectangle tipRectangle)
        {
            if (info.Count == 0) return;

            for (int i = 0, xPos = tipRectangle.X; i < info.Count; i++, xPos += TileInfoWidth)
            {
                g.DrawString($"{info[i].Type}", font, Brushes.White, new PointF(xPos, tipRectangle.Y + 3F));
                g.DrawString($"ID: {info[i].Id}", font, Brushes.White, new PointF(xPos, tipRectangle.Y + 16F));
                g.DrawString($"SPR:{info[i].spriteName}", font, Brushes.White, new PointF(xPos, tipRectangle.Y + 29F));
            }
        }

        public void DrawHighlight(ICoordsConverter sender, Graphics g, Point position, Pen highlightPen, float zoom)
        {
            var tileCoords = sender.ConvertToPictureBoxCoords(GetClosestTileCenter(position));
            var points = new PointF[]
            {
                    new PointF(tileCoords.X - (32*zoom), tileCoords.Y),
                    new PointF(tileCoords.X, tileCoords.Y - (16*zoom)),
                    new PointF(tileCoords.X + (32*zoom), tileCoords.Y),
                    new PointF(tileCoords.X, tileCoords.Y + (16*zoom)),
                    new PointF(tileCoords.X - (32*zoom), tileCoords.Y),
            };
            g.DrawLines(highlightPen, points);
        }

    }
}
