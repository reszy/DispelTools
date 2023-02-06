using DispelTools.GameDataModels.Map;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Shapes;
using View.Components.PictureDisplay;
using View.Views;

namespace View.ViewModels
{
    internal class MapViewerController : IPictureDisplayerController
    {
        public bool EvenTiles { get; set; }

        private readonly MapViewerView infoSource;

        public MapViewerController(MapViewerView infoSource)
        {
            this.infoSource = infoSource;
        }

        public bool HasCustomHighlight => true;

        public (Polygon polygon, System.Windows.Point offset) CreateCustomHighlight()
        {
            List<System.Windows.Point> points = new() {
                new System.Windows.Point(-32, 0),
                new System.Windows.Point(0, -16),
                new System.Windows.Point(32, 0),
                new System.Windows.Point(0, 16),
            };
            return (
                new Polygon()
                {
                    StrokeThickness = 2,
                    Points = new PointCollection(points),
                    Fill = new SolidColorBrush(Colors.Transparent),
                    Stretch = Stretch.Uniform,
                    Width = 62,
                    Height = 32
                },
                new(32, 16)
            );
        }

        public Point GetCustomHighlightImagePosition(Point pointerImagePosition)
        {
            return GetClosestTileCenter(pointerImagePosition);
        }

        public void PixelSelected(ICoordsConverter sender, PixelSelectedArgs args)
        {
            infoSource.CreateTileInfo(args.Position);
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
    }
}
