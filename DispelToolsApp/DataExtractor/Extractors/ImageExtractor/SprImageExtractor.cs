using DispelTools.Common;
using DispelTools.DebugTools.MetricTools;
using DispelTools.GameDataModels.Sprite;
using System.IO;

namespace DispelTools.DataExtractor.ImageExtractor
{
    public class SprImageExtractor : Extractor
    {
        public override void ExtractFile(ExtractionFileProcess process)
        {
            var file = process.File;
            var loader = new SpriteLoader(process.File, process.Filename, process.Options.ColorMode);
            file.Skip(268);

            int sequenceCounter = 0;
            while (process.Stream.Position < process.Stream.Length)
            {
                if (SeekNextSequence(process))
                {
                    LoadAndSaveSequence(process, loader, sequenceCounter);
                    sequenceCounter++;
                }
                else
                {
                    return;
                }
            }
            Metrics.Count(MetricFile.SpriteFileMetric, "ToFileEnd", process.Stream.Length - process.Stream.Position);
        }

        private void LoadAndSaveSequence(ExtractionFileProcess process, SpriteLoader loader, int imageNumber)
        {
            var sequence = loader.LoadSequence();
            string createdFileName = $"{process.Filename}.{imageNumber}";

            if (!Settings.ExtractorReadOnly)
            {
                sequence.SaveAsImage(process.OutputDirectory, createdFileName, process.Options.CreateAnimatedGifs, process.Options.BlackAsTransparent);
                process.WorkReporter.ReportFileCreated(process, createdFileName);
            }
        }

        private bool SeekNextSequence(ExtractionFileProcess process)
        {
            long oldPosition = process.Stream.Position;
            bool validSpriteSequence = false;
            int numberOfSkips = 0;
            int dataChunkSize = 1;
            int readIntSize = dataChunkSize + 14;
            while (!validSpriteSequence)
            {
                if (process.Stream.Position + (readIntSize * 4) >= process.Stream.Length) { break; }
                int[] ints = process.File.ReadInts(readIntSize);
                process.Stream.Seek(-((readIntSize - dataChunkSize) * 4), SeekOrigin.Current);
                if ((ints[0] == 0 && ints[1] > 0 && ints[1] < 255 && ints[2] == 0 && ints[11] > 0 && ints[12] > 0 && ints[11] * ints[12] == ints[13]) ||
                    (ints[0] == 8 && ints[1] == 0 && ints[2] > 0 && ints[2] < 255 && ints[3] == 0 && ints[12] > 0 && ints[13] > 0 && ints[12] * ints[13] == ints[14]))
                {
                    validSpriteSequence = true;
                }
                else
                {
                    numberOfSkips++;
                }
            }

            if (numberOfSkips == 1)
            {
                numberOfSkips = 0;
            }
            int sikpByteSize = numberOfSkips * dataChunkSize * 4;
            Metrics.Count(MetricFile.SpriteFileMetric, "SpriteGap", sikpByteSize.ToString());
            Metrics.Count(MetricFile.SpriteFileMetric, process.Filename, "numberOfSkips", numberOfSkips);
            process.File.SetPosition(oldPosition + sikpByteSize);
            return validSpriteSequence;
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
        //            Metrics.Gauge(MetricFile.SpriteOffsetMetric, $"file.{process.Filename}", file.BaseStream.Position);
        //            Metrics.AddMetric(MetricFile.SpriteFileMetric, new GeneralFileCounterDto("spriteFrameCount", process.Filename));
        //            Metrics.Count(MetricFile.SpriteFileMetric, "allFrames");
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

        //private string ReadImage(ExtractionFileProcess process, BinaryReader file, int width, int height, int pixelCount, string filePrefix)
        //{
        //    file.ReadBytes(pixelCount * 2);//for metrics only use
        //    //var image = ImageProcessing.ImageLoader.Load((uint)width, (uint)height, file.ReadBytes(pixelCount * 2));
        //    string finalName = $"{process.OutputDirectory}\\{filePrefix}.png";
        //    //image.Save(finalName, ImageFormat.Png);
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
