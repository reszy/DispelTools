using DispelTools.Common;
using DispelTools.DebugTools.Metrics;
using DispelTools.DebugTools.Metrics.Dto;
using DispelTools.ImageProcessing.Sprite;
using System;

namespace DispelTools.DataExtractor.ImageExtractor
{
    public class SprImageExtractor : Extractor
    {
        public override void ExtractFile(ExtractionFileProcess process)
        {
            var file = process.File;
            var loader = new SpriteLoader(process.File, process.Filename);

            int imageStamp = file.ReadInt32();
            FileMetrics.AddMetric(new GeneralFileMetricDto("imageStamp", process.Filename, imageStamp));
            int imageOffset = imageStamp == 6 ? 1904 : (imageStamp == 9 ? 2996 : throw new NotImplementedException($"Unexpected imageStamp {imageStamp}"));
            file.Skip(264);

            int sequenceCounter = 0;
            while (process.Stream.Position + imageOffset + 264 < process.Stream.Length)
            {
                SeekNextSequence(process);
                LoadAndSaveSequence(process, loader, sequenceCounter);
                sequenceCounter++;
            }
            FileMetrics.AddMetric(new ToFileEndMetricDto(process.Stream.Length - process.Stream.Position, "Sprite"));
        }

        private void LoadAndSaveSequence(ExtractionFileProcess process, SpriteLoader loader, int imageNumber)
        {
            var sequence = loader.LoadSequence();
            string createdFileName = $"{process.Filename}.{imageNumber}";
            sequence.SaveAsImage(process.OutputDirectory, createdFileName);
            process.Extractor.RaportFileCreatedDetail(process, createdFileName);
        }

        private void SeekNextSequence(ExtractionFileProcess process)
        {
            long oldPosition = process.Stream.Position;
            int value = 0;
            int skipSize = 0;
            while (value == 0)
            {
                value = process.File.ReadInt32();
                process.File.Skip(8);
                if (value == 0)
                {
                    skipSize += 3;
                }
            }

            if (skipSize == 3)
            {
                skipSize = 0;
            }
            FileMetrics.AddMetric(new SpriteGapMetricDto(skipSize * 4));
            FileMetrics.AddMetric(new GeneralFileMetricDto("intSkipSize", process.Filename, skipSize));
            process.File.SetPosition(oldPosition + (skipSize * 4));
        }

        /// OLD METHOD
        //public override void ExtractFile(ExtractionFileProcess process)
        //{
        //    var file = process.File;
        //    int i = 0;
        //    while (file.BaseStream.Position + (100) <= file.BaseStream.Length)
        //    {
        //        int x = file.ReadInt32();
        //        long nextCheckPosition = file.BaseStream.Position - 2;
        //        int y = file.ReadInt32();
        //        int area = file.ReadInt32();
        //        int calculatedArea = x * y;
        //        if (calculatedArea == area && area > 1 && x < 40000 && y < 40000 && x > 1 && y > 1 && file.BaseStream.Position + (area * 2) < file.BaseStream.Length)
        //        {
        //            string createdFileName = ReadImage(process, file, x, y, area, $"{process.Filename}.{i++}");
        //            //file.BaseStream.Seek(nextCheckPosition, SeekOrigin.Begin); don't need that, condition is enough to find all
        //            process.Extractor.RaportFileCreatedDetail(process, createdFileName);
        //        }
        //        else
        //        {
        //            file.BaseStream.Seek(nextCheckPosition, SeekOrigin.Begin);
        //        }
        //    }
        //}
        //
        //private string ReadImage(ExtractionFileProcess process, BinaryReader file, int width, int height, int pixelCount, string filePrefix)
        //{
        //    var image = ImageProcessing.ImageLoader.Load((uint)width, (uint)height, file.ReadBytes(pixelCount * 2));
        //    string finalName = $"{process.OutputDirectory}\\{filePrefix}.png";
        //    image.Save(finalName, ImageFormat.Png);
        //    return finalName;
        //}

        /// OLDEST METHOD
        //protected override void ExtractProcess()
        //{
        //    var header = file.ReadBytes(272);
        //    var groupCounter = 0;
        //    file.ReadInt32();
        //    var imageGroupAhead = file.ReadInt32();
        //    file.ReadInt32();
        //
        //    while (imageGroupAhead > 0)
        //    {
        //
        //        ReadSetOfImages(imageGroupAhead, $"{newFilename}.{groupCounter}");
        //        groupCounter += imageGroupAhead;
        //
        //        var g1 = file.ReadInt32();
        //        imageGroupAhead = file.ReadInt32();
        //        var g2 = file.ReadInt32();
        //        if (g1 != 0 && g2 != 0)
        //        {
        //            break;
        //        }
        //    }
        //}
        //
        //private void ReadSetOfImages(int count, string filePrefix)
        //{
        //    for (int i = 0; i < count; i++)
        //    {
        //        file.ReadBytes(32);
        //        var width = file.ReadInt32();
        //        var height = file.ReadInt32();
        //        var pixelCount = file.ReadInt32();
        //        if (width * height != pixelCount)
        //        {
        //            throw new ExtractException($"{ExceptionMessagePrefix()} size doesn't match width and height");
        //        }
        //        var image = ImageProcessing.ImageProcessor.Process((uint)width, (uint)height, file.ReadBytes(pixelCount * 2));
        //        var finalName = $"{outputDirectory}\\{filePrefix}.{i}.png";
        //        image.Save(finalName, ImageFormat.Png);
        //        CreateFileCreatedDetail(finalName);
        //    }
        //}
    }
}
