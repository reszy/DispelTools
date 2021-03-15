using System.Collections.Generic;
using System.ComponentModel;

namespace DispelTools.DataExtractor.MapImageExtractor
{
    public class MapImageExtractor : Extractor
    {
        private long lastPixelRead = 0;

        public override void ExtractFile(ExtractionFileProcess process)
        {
            //file.BaseStream.Seek(108258, SeekOrigin.Begin); //map1
            //file.BaseStream.Seek(147378, SeekOrigin.Begin); //map2
            //var groupCounter = 0;
            //var imageGroupAhead = file.ReadInt32();
            //file.ReadInt32();

            //while (imageGroupAhead > 0)
            //{

            //    ReadSetOfImages(imageGroupAhead, $"{newFilename}.{groupCounter}");
            //    groupCounter += imageGroupAhead;
            //    while(file.ReadInt32() == 0) { }
            //    do
            //    {
            //        file.ReadBytes(360);
            //    }
            //    while (file.ReadInt32() == 8);
            //    var g1 = file.ReadInt32();
            //    file.ReadInt32();
            //    imageGroupAhead = file.ReadInt32();
            //    var g2 = file.ReadInt32();
            //    if (g1 != 8 && g2 != 0)
            //    {
            //        break;
            //    }
            //}
            //ResultDetails.Add($"Last read {ExceptionMessagePrefix()}");
            //ResultDetails.Add($"Last pixel read at {lastPixelRead}");
        }

        private void ReadSetOfImages(int count, string filePrefix)
        {
            //for (int i = 0; i < count; i++)
            //{
            //    file.ReadBytes(32);
            //    var width = file.ReadInt32();
            //    var height = file.ReadInt32();
            //    var pixelCount = file.ReadInt32();
            //    if (width == 0 || height == 0 || pixelCount == 0)
            //    {
            //        throw new ExtractException($"{ExceptionMessagePrefix()} image size is zero");
            //    }
            //    if (width * height != pixelCount)
            //    {
            //        throw new ExtractException($"{ExceptionMessagePrefix()} size doesn't match width and height");
            //    }
            //    var image = ImageProcessing.ImageProcessor.Process((uint)width, (uint)height, file.ReadBytes(pixelCount * 2));
            //    lastPixelRead = file.BaseStream.Position;
            //    var finalname = $"{outputDirectory}\\{filePrefix}.{i}.png";
            //    image.Save(finalname, ImageFormat.Png);
            //    CreateFileCreatedDetail(finalname);
            //}
        }
    }
}
