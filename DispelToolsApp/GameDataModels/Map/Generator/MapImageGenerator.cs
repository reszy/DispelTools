using DispelTools.Common;
using DispelTools.Common.DataProcessing;
using DispelTools.GameDataModels.Map.External;
using System.Collections.Generic;
using System.Drawing;

namespace DispelTools.GameDataModels.Map.Generator
{
    public class MapImageGenerator
    {
        private readonly WorkReporter workReporter;
        private readonly MapContainer mapContainer;
        private readonly TextGenerator textGenerator;
        private GeneratorOptions generatorOptions;

        private List<(Point point, Color color)> debugDots;

        private int progressTracker = 0;
        private MapModel Model => mapContainer?.Model;

        public MapImageGenerator(WorkReporter workReporter, MapContainer mapContainer, TextGenerator textGenerator)
        {
            this.workReporter = workReporter;
            this.mapContainer = mapContainer;
            this.textGenerator = textGenerator;
            debugDots = new List<(Point point, Color color)>();
        }
        public DirectBitmap GenerateMap(GeneratorOptions generatorOptions)
        {
            debugDots.Clear();
            this.generatorOptions = generatorOptions;
            int imageWidth = generatorOptions.Occlusion ? Model.OccludedMapSizeInPixels.Width : Model.MapSizeInPixels.Width;
            int imageHeight = generatorOptions.Occlusion ? Model.OccludedMapSizeInPixels.Height : Model.MapSizeInPixels.Height;
            var mapImage = new DirectBitmap(imageWidth, imageHeight);

            workReporter.SetTotal(CalculateTotalProgress());
            var offset = CalculateOcclusionOffset();

            if (generatorOptions.GTL || generatorOptions.Events || generatorOptions.Collisions)
            {
                PlotBase(mapImage);
            }
            PlotObjects(mapImage, offset);
            if (generatorOptions.Roofs)
            {
                PlotRoofs(mapImage);
            }
            if (generatorOptions.DebugDots)
            {
                if (debugDots.Count > 0) workReporter.SetText("Printing debug dots...");
                foreach (var (point, color) in debugDots) DrawDot(mapImage, point, color);
            }
            return mapImage;
        }

        private void PlotBase(DirectBitmap image)
        {
            var defaultTile = TileSet.BlankTile;
            for (int y = 0; y < Model.TiledMapSize.Height; y++)
            {
                for (int x = 0; x < Model.TiledMapSize.Width; x++)
                {
                    var tile = generatorOptions.GTL ? mapContainer.Gtl[Model.GetGtlId(x, y)] : defaultTile;

                    var drawCollision = generatorOptions.Collisions && Model.GetCollision(x, y);
                    var eventId = Model.GetEventId(x, y);

                    var drawEvent = generatorOptions.Events && eventId > 0;

                    if (drawCollision || drawEvent)
                    {
                        var tintColor = drawEvent && drawCollision ? Color.Yellow : (drawEvent ? Color.Blue : Color.Red);
                        tile = tile.MixColor(tintColor, 64);
                    }
                    var mapCoords = ConvertMapCoordsToImageCoords(x, y);
                    if (generatorOptions.Occlusion)
                    {
                        mapCoords.X -= Model.MapNonOccludedStart.X;
                        mapCoords.Y -= Model.MapNonOccludedStart.Y;
                    }
                    tile.PlotTileOnBitmap(image, mapCoords.X, mapCoords.Y);

                    if (drawEvent)
                    {
                        textGenerator.PlotIdOnMap(image, eventId, mapCoords.X + TileSet.TILE_WIDTH_HALF, mapCoords.Y + TextGenerator.DigitHeight);
                    }
                }
                workReporter.ReportProgress(++progressTracker);
            }
        }
        private void PlotTiledObjects(DirectBitmap image, Point offset)
        {
            foreach (var btlData in Model.TiledObjectInfos)
            {
                PlotSingleTiledObject(image, btlData, offset);
                workReporter.ReportProgress(++progressTracker);
            }
        }
        private void PlotInternalSprites(DirectBitmap image, Point offset)
        {
            foreach (var spriteData in Model.InternalSpriteInfos)
            {
                PlotSingleSprite(image, spriteData, offset);
                workReporter.ReportProgress(++progressTracker);
            }
        }


        private void PlotObjects(DirectBitmap image, Point offset)
        {
            List<IInterlacedOrderObject> interlacedObjects = new List<IInterlacedOrderObject>();
            if (generatorOptions.ExternalExtra) interlacedObjects.AddRange(mapContainer.ExtraEntities);
            if (generatorOptions.ExternalMonster) interlacedObjects.AddRange(mapContainer.MonsterEntities);
            if (generatorOptions.ExternalNpc) interlacedObjects.AddRange(mapContainer.NpcEntities);
            if (generatorOptions.Sprites) interlacedObjects.AddRange(Model.InternalSpriteInfos);
            if (generatorOptions.TiledObjects) interlacedObjects.AddRange(Model.TiledObjectInfos);
            interlacedObjects.Sort(new IInterlacedOrderObjectComparer());

            foreach (var @object in interlacedObjects)
            {
                if (@object is InternalSpriteInfo spriteInfo)
                    PlotSingleSprite(image, spriteInfo, offset);
                else if (@object is TiledObjectsInfo tiledObjectsInfo)
                    PlotSingleTiledObject(image, tiledObjectsInfo, offset);
                else if (@object is MapExternalObject externalObject)
                    PlotSingleSpriteByTile(image, externalObject);

                workReporter.ReportProgress(++progressTracker);
            }
        }

        private Point CalculateOcclusionOffset() => (!generatorOptions.Occlusion) ? new Point(Model.MapNonOccludedStart.X, Model.MapNonOccludedStart.Y) : new Point();

        private void PlotSingleSprite(DirectBitmap image, InternalSpriteInfo info, Point offset)
        {
            var sprite = mapContainer.InternalSprites[info.Id];
            int destX = info.Position.X + offset.X;
            int destY = info.Position.Y + offset.Y;
            PlotSpriteOnBitmap(image, sprite.GetFrame(0).RawRgb, destX, destY);
            DebugDot(new Point(destX, info.PositionOrder), Color.Red);
        }
        private void PlotSingleSpriteByTile(DirectBitmap image, MapExternalObject info)
        {
            var mapCoords = ConvertMapCoordsToImageCoords(info.X, info.Y);
            if (generatorOptions.Occlusion)
            {
                mapCoords.X -= Model.MapNonOccludedStart.X;
                mapCoords.Y -= Model.MapNonOccludedStart.Y;
            }
            DebugDot(new Point(mapCoords.X + TileSet.TILE_WIDTH_HALF, info.PositionOrder), Color.Green);
            mapCoords.X += TileSet.TILE_WIDTH_HALF;
            mapCoords.Y += TileSet.TILE_HEIGHT_HALF;
            if (info.Flip)
                PlotFilippedSpriteOnBitmap(image, info.Graphic.RawRgb, mapCoords.X - (info.Graphic.RawRgb.Width - info.Graphic.OriginX), mapCoords.Y - info.Graphic.OriginY);
            else
                PlotSpriteOnBitmap(image, info.Graphic.RawRgb, mapCoords.X - info.Graphic.OriginX, mapCoords.Y - info.Graphic.OriginY);

        }
        private void PlotSingleTiledObject(DirectBitmap image, TiledObjectsInfo info, Point offset)
        {
            for (int i = 0; i < info.Size; i++)
            {
                var tile = mapContainer.Btl[info.GetId(i)];
                var x = info.Position.X + offset.X;
                var y = info.Position.Y + (i * TileSet.TILE_HEIGHT) + offset.Y;
                tile.PlotTileOnBitmap(image, x, y);
                DebugDot(new Point(x + TileSet.TILE_WIDTH_HALF, info.PositionOrder), Color.Blue);
            }
        }
        private class IInterlacedOrderObjectComparer : IComparer<IInterlacedOrderObject>
        {
            public int Compare(IInterlacedOrderObject a, IInterlacedOrderObject b)
            {
                var positionDiff = a.PositionOrder - b.PositionOrder;
                var typDiff = a.TypeOrder - b.TypeOrder;
                return positionDiff != 0 ? positionDiff : (typDiff != 0 ? typDiff : a.Order - b.Order);
            }
        }


        private void PlotRoofs(DirectBitmap image)
        {
            for (int y = 0; y < Model.TiledMapSize.Height; y++)
            {
                for (int x = 0; x < Model.TiledMapSize.Width; x++)
                {
                    int btlId = Model.GetRoofBtlId(x, y);
                    if (btlId > 0)
                    {
                        var tile = mapContainer.Btl[btlId];
                        var mapCoords = ConvertMapCoordsToImageCoords(x, y);
                        if (generatorOptions.Occlusion)
                        {
                            mapCoords.X -= Model.MapNonOccludedStart.X;
                            mapCoords.Y -= Model.MapNonOccludedStart.Y;
                        }
                        tile.PlotTileOnBitmap(image, mapCoords.X, mapCoords.Y);
                    }
                }
                workReporter.ReportProgress(++progressTracker);
            }
        }

        private int CalculateTotalProgress()
        {
            return ((generatorOptions.GTL || generatorOptions.Events || generatorOptions.Collisions) ? Model.TiledMapSize.Height : 0)
                 + (generatorOptions.Roofs ? Model.TiledMapSize.Height : 0)
                 + (generatorOptions.Sprites ? Model.InternalSpriteInfos.Count : 0)
                 + (generatorOptions.TiledObjects ? Model.TiledObjectInfos.Count : 0)
                 + (generatorOptions.ExternalExtra ? mapContainer.ExtraEntities.Count : 0)
                 + (generatorOptions.ExternalMonster ? mapContainer.MonsterEntities.Count : 0)
                 + (generatorOptions.ExternalNpc ? mapContainer.NpcEntities.Count : 0);
        }

        private Point ConvertMapCoordsToImageCoords(int x, int y)
        {
            return new Point(
                   (x + y) * TileSet.TILE_HORIZONTAL_OFFSET_HALF,
                   (-x + y) * TileSet.TILE_HEIGHT_HALF + (Model.MapDiagonalTiles / 2 * TileSet.TILE_HEIGHT_HALF));
        }

        private void DebugDot(Point point, Color color)
        {
            debugDots.Add((point, color));
        }
        private void DrawDot(DirectBitmap parent, Point point, Color color)
        {
            for (int x = point.X - 1; x < point.X + 2; x++)
                for (int y = point.Y - 1; y < point.Y + 2; y++)
                {
                    if (x >= 0 && x < parent.Width && y >= 0 && y < parent.Height)
                        parent.SetPixel(x, y, color);
                }
        }

        private void PlotSpriteOnBitmap(DirectBitmap parent, RawRgb sprite, int destX, int destY)
        {
            //TODO Move comments to docs
            //destX += (map.Width / 5) * TileSet.TILE_WIDTH *;
            //destY += (map.Height/5) * TileSet.TILE_HEIGHT;

            //cat1 5x5
            //destX += 2400;
            //destY += 784;

            //dun1 10x10
            //destX += 2400 * 2 - TileSet.TILE_HORIZONTAL_OFFSET_HALF;
            //destY += 784 * 2 + TileSet.TILE_HEIGHT;

            //catp 8x8
            //destX += 3808; //2400 * 1.6 - TileSet.TILE_HORIZONTAL_OFFSET_HALF;
            //destY += 1280; //784 * 1.6 + TileSet.TileHeight

            //map1 20x20
            //destX += 9568; //2400 * 1.6 - TileSet.TILE_HORIZONTAL_OFFSET_HALF;
            //destY += 3200; //784 * 1.6 + TileSet.TileHeight
            if (destX + sprite.Width <= parent.Width && destX >= 0 && destY >= 0 && destY + sprite.Height <= parent.Height)
            {
                for (int y = 0; y < sprite.Height; y++)
                {
                    for (int x = 0; x < sprite.Width; x++)
                    {
                        var color = sprite.GetPixel(x, y);
                        if (color.A != 0)
                        {
                            int finalX = destX + x;
                            int finalY = destY + y;
                            parent.SetPixel(finalX, finalY, color);
                        }
                    }
                }
            }
        }
        private void PlotFilippedSpriteOnBitmap(DirectBitmap parent, RawRgb sprite, int destX, int destY)
        {
            if (destX + sprite.Width <= parent.Width && destX >= 0 && destY >= 0 && destY + sprite.Height <= parent.Height)
            {
                for (int y = 0; y < sprite.Height; y++)
                {
                    for (int x = 0; x < sprite.Width; x++)
                    {
                        var color = sprite.GetPixel(sprite.Width - x - 1, y);
                        if (color.A != 0)
                        {
                            int finalX = destX + x;
                            int finalY = destY + y;
                            parent.SetPixel(finalX, finalY, color);
                        }
                    }
                }
            }
        }
    }
}
