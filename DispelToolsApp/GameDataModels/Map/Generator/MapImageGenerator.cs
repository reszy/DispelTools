﻿using DispelTools.Common;
using System.Drawing;

namespace DispelTools.GameDataModels.Map.Generator
{
    public class MapImageGenerator
    {
        private const uint WHITE_COLOR = 0xFF000000;

        private readonly WorkReporter workReporter;
        private readonly MapContainer mapContainer;
        private GeneratorOptions generatorOptions;

        private int progressTracker = 0;
        private MapModel Model => mapContainer?.Model;

        public MapImageGenerator(WorkReporter workReporter, MapContainer mapContainer)
        {
            this.workReporter = workReporter;
            this.mapContainer = mapContainer;
        }
        public DirectBitmap GenerateMap(GeneratorOptions generatorOptions)
        {
            this.generatorOptions = generatorOptions;
            int imageWidth = generatorOptions.Occlusion ? Model.OccludedMapSizeInPixels.Width : Model.MapSizeInPixels.Width;
            int imageHeight = generatorOptions.Occlusion ? Model.OccludedMapSizeInPixels.Height : Model.MapSizeInPixels.Height;
            var mapImage = new DirectBitmap(imageWidth, imageHeight);

            workReporter.SetTotal(CalculateTotalProgress());

            if (generatorOptions.GTL)
            {
                PlotGtlAndCollisions(ref mapImage);
            }
            if (generatorOptions.TiledObjects)
            {
                PlotTiledObjects(ref mapImage);
            }
            if (generatorOptions.Sprites)
            {
                PlotInternalSprites(ref mapImage);
            }
            if (generatorOptions.Roofs)
            {
                PlotRoofs(ref mapImage);
            }
            return mapImage;
        }

        private void PlotGtlAndCollisions(ref DirectBitmap image)
        {
            for (int y = 0; y < Model.TiledMapSize.Height; y++)
            {
                for (int x = 0; x < Model.TiledMapSize.Width; x++)
                {
                    var tile = mapContainer.Gtl[Model.GetGtlId(x, y)];
                    if (generatorOptions.Collisions && Model.GetCollision(x, y))
                    {
                        tile = tile.MixColor(Color.Red, 128);
                    }
                    var mapCoords = ConvertMapCoordsToImageCoords(x, y);
                    if (generatorOptions.Occlusion)
                    {
                        mapCoords.X -= Model.MapNonOccludedStart.X;
                        mapCoords.Y -= Model.MapNonOccludedStart.Y;
                    }
                    tile.PlotTileOnBitmap(ref image, mapCoords.X, mapCoords.Y);
                    workReporter.ReportProgress(++progressTracker);
                }
            }
        }
        private void PlotTiledObjects(ref DirectBitmap image)
        {
            foreach (var btlData in Model.TiledObjectInfos)
            {
                for (int i = 0; i < btlData.Size; i++)
                {
                    var tile = mapContainer.Btl[btlData.GetId(i)];
                    tile.PlotTileOnBitmap(ref image, btlData.Position.X, btlData.Position.Y + (i * TileSet.TILE_HEIGHT));
                }
                workReporter.ReportProgress(++progressTracker);
            }
        }
        private void PlotInternalSprites(ref DirectBitmap image)
        {
            foreach (var spriteData in Model.InternalSpriteInfos)
            {
                var sprite = mapContainer.SpritesImageCache[spriteData.Id];
                int destX = spriteData.Position.X;
                int destY = spriteData.Position.Y;
                if (!generatorOptions.Occlusion)
                {
                    destX += Model.MapNonOccludedStart.X;
                    destY += Model.MapNonOccludedStart.Y;
                }
                PlotSpriteOnBitmap(ref image, sprite, destX, destY);
                workReporter.ReportProgress(++progressTracker);
            }
        }

        private void PlotRoofs(ref DirectBitmap image)
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
                        tile.PlotTileOnBitmap(ref image, mapCoords.X, mapCoords.Y);
                    }
                    workReporter.ReportProgress(++progressTracker);
                }
            }
        }

        private int CalculateTotalProgress()
        {
            return (generatorOptions.GTL ? Model.TiledMapSize.Width * Model.TiledMapSize.Height : 0)
                 + (generatorOptions.Roofs ? Model.TiledMapSize.Width * Model.TiledMapSize.Height : 0)
                 + (generatorOptions.Sprites ? Model.InternalSpriteInfos.Count : 0)
                 + (generatorOptions.TiledObjects ? Model.TiledObjectInfos.Count : 0);
        }

        private Point ConvertMapCoordsToImageCoords(int x, int y)
        {
            return new Point(
                   (x + y) * TileSet.TILE_HORIZONTAL_OFFSET_HALF,
                   (-x + y) * TileSet.TILE_HEIGHT_HALF + (Model.MapDiagonalTiles / 2 * TileSet.TILE_HEIGHT_HALF));
        }

        private void PlotSpriteOnBitmap(ref DirectBitmap parent, DirectBitmap sprite, int destX, int destY)
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
                        if ((uint)color.ToArgb() != WHITE_COLOR)
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