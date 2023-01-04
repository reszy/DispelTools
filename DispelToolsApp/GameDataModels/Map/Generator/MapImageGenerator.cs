using DispelTools.Common;
using DispelTools.Common.DataProcessing;
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

        private int progressTracker = 0;
        private MapModel Model => mapContainer?.Model;

        public MapImageGenerator(WorkReporter workReporter, MapContainer mapContainer, TextGenerator textGenerator)
        {
            this.workReporter = workReporter;
            this.mapContainer = mapContainer;
            this.textGenerator = textGenerator;
        }
        public DirectBitmap GenerateMap(GeneratorOptions generatorOptions)
        {
            this.generatorOptions = generatorOptions;
            int imageWidth = generatorOptions.Occlusion ? Model.OccludedMapSizeInPixels.Width : Model.MapSizeInPixels.Width;
            int imageHeight = generatorOptions.Occlusion ? Model.OccludedMapSizeInPixels.Height : Model.MapSizeInPixels.Height;
            var mapImage = new DirectBitmap(imageWidth, imageHeight);

            workReporter.SetTotal(CalculateTotalProgress());

            PlotBase(mapImage);

            var offset = CalculateOcclusionOffset();
            if (generatorOptions.TiledObjects && generatorOptions.Sprites)
            {
                PlotObjects(mapImage, offset);
            }
            else
            {
                if (generatorOptions.TiledObjects)
                {
                    PlotTiledObjects(mapImage, offset);
                }
                if (generatorOptions.Sprites)
                {
                    PlotInternalSprites(mapImage, offset);
                }
            }
            if (generatorOptions.Roofs)
            {
                PlotRoofs(mapImage);
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


            List<IInterlacedOrderObject> interlacedObjects = new List<IInterlacedOrderObject>(Model.TiledObjectInfos);
            interlacedObjects.AddRange(Model.InternalSpriteInfos);
            interlacedObjects.Sort(new IInterlacedOrderObjectComparer());

            foreach (var @object in interlacedObjects)
            {
                if (@object is InternalSpriteInfo spriteInfo)
                    PlotSingleSprite(image, spriteInfo, offset);
                else if (@object is TiledObjectsInfo tiledObjectsInfo)
                    PlotSingleTiledObject(image, tiledObjectsInfo, offset);

                workReporter.ReportProgress(++progressTracker);
            }

            ////debug dots
            //foreach (var @object in interlacedObjects)
            //{
            //    if (@object is InternalSpriteInfo spriteInfo)
            //    {
            //        DrawDot(image, spriteInfo.Position, Color.Yellow, occlusionOffsetX, occlusionOffsetY);
            //        DrawDot(image, new Point(spriteInfo.Position.X, spriteInfo.BottomRightPosition.Y), Color.Magenta, occlusionOffsetX, occlusionOffsetY);
            //    }
            //    else if (@object is TiledObjectsInfo tiledObjectsInfo)
            //    {
            //        DrawDot(image, tiledObjectsInfo.Position, Color.Blue, occlusionOffsetX, occlusionOffsetY);
            //        DrawDot(image, new Point(tiledObjectsInfo.Position.X, tiledObjectsInfo.Position.Y + tiledObjectsInfo.Size * TileSet.TILE_HEIGHT), Color.Red, occlusionOffsetX, occlusionOffsetY);
            //    }
            //}
        }

        private Point CalculateOcclusionOffset()
        {
            int occlusionOffsetX = 0, occlusionOffsetY = 0;
            if (!generatorOptions.Occlusion)
            {
                occlusionOffsetX = Model.MapNonOccludedStart.X;
                occlusionOffsetY = Model.MapNonOccludedStart.Y;
            }
            if (generatorOptions.Cat3Fix)
            {
                occlusionOffsetX -= TileSet.TILE_HORIZONTAL_OFFSET_HALF;
                occlusionOffsetY += TileSet.TILE_HEIGHT_HALF;
            }
            return new Point(occlusionOffsetX, occlusionOffsetY);
        }

        private void PlotSingleSprite(DirectBitmap image, InternalSpriteInfo info, Point offset)
        {
            var sprite = mapContainer.InternalSprites[info.Id];
            int destX = info.Position.X + offset.X;
            int destY = info.Position.Y + offset.Y;
            PlotSpriteOnBitmap(image, sprite.GetFrame(0).RawRgb, destX, destY);
        }
        private void PlotSingleTiledObject(DirectBitmap image, TiledObjectsInfo info, Point offset)
        {
            for (int i = 0; i < info.Size; i++)
            {
                var tile = mapContainer.Btl[info.GetId(i)];
                var x = info.Position.X + offset.X;
                var y = info.Position.Y + (i * TileSet.TILE_HEIGHT) + offset.Y;
                tile.PlotTileOnBitmap(image, x, y);
            }
        }
        private class IInterlacedOrderObjectComparer : IComparer<IInterlacedOrderObject>
        {
            public int Compare(IInterlacedOrderObject a, IInterlacedOrderObject b)
            {
                var position = a.PositionOrder - b.PositionOrder;
                var type = a.TypeOrder - b.TypeOrder;
                return position != 0 ? position : (type != 0 ? type : a.Order - b.Order);
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
                 + (generatorOptions.TiledObjects ? Model.TiledObjectInfos.Count : 0);
        }

        private Point ConvertMapCoordsToImageCoords(int x, int y)
        {
            return new Point(
                   (x + y) * TileSet.TILE_HORIZONTAL_OFFSET_HALF,
                   (-x + y) * TileSet.TILE_HEIGHT_HALF + (Model.MapDiagonalTiles / 2 * TileSet.TILE_HEIGHT_HALF));
        }

        //Debug draw
        //private void DrawDot(DirectBitmap parent, Point point, Color color, Point offset)
        //{
        //    for (int x = point.X - 1; x < point.X + 2; x++)
        //        for (int y = point.Y - 1; y < point.Y + 2; y++)
        //            parent.SetPixel(x + offset.X, y + offset.Y, color);
        //}

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
    }
}
