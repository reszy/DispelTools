using DispelTools.Common;
using DispelTools.DebugTools.MetricTools;
using DispelTools.ImageProcessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.GameDataModels.Sprite
{
    public interface ISpriteSequenceProcessor
    {
        void Process(SpriteSequence sequence, int imageNumber);
    }

    public abstract class OpenedSpriteFile : ISpriteSequenceProcessor
    {
        protected OpenedSpriteFile(BinaryReader file, string filename, ColorManagement.ColorMode colorMode)
        {
            File = file;
            Filename = filename;
            ColorMode = colorMode;
        }

        public BinaryReader File { get; }
        public Stream Stream { get => File.BaseStream; }
        public string Filename { get; }
        public ColorManagement.ColorMode ColorMode { get; }

        public abstract void Process(SpriteSequence sequence, int imageNumber);
    }
    public static class SpriteFileReader
    {
        public static void ProcessThroughFile(OpenedSpriteFile file)
        {
            var loader = new SpriteLoader(file.File, file.Filename, file.ColorMode);
            file.File.Skip(268);

            int sequenceCounter = 0;
            while (file.Stream.Position < file.Stream.Length)
            {
                if (SeekNextSequence(file))
                {
                    var sequence = loader.LoadSequence();

                    file.Process(sequence, sequenceCounter);
                    sequenceCounter++;
                }
                else
                {
                    return;
                }
            }
            Metrics.Count(MetricFile.SpriteFileMetric, "ToFileEnd", file.Stream.Length - file.Stream.Position);
        }

        private static bool SeekNextSequence(OpenedSpriteFile file)
        {
            long oldPosition = file.Stream.Position;
            bool validSpriteSequence = false;
            int numberOfSkips = 0;
            int dataChunkSize = 1;
            int readIntSize = dataChunkSize + 14;
            while (!validSpriteSequence)
            {
                if (file.Stream.Position + (readIntSize * 4) >= file.Stream.Length) { break; }
                int[] ints = file.File.ReadInts(readIntSize);
                file.Stream.Seek(-((readIntSize - dataChunkSize) * 4), SeekOrigin.Current);
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
            Metrics.Count(MetricFile.SpriteFileMetric, file.Filename, "numberOfSkips", numberOfSkips);
            file.File.SetPosition(oldPosition + sikpByteSize);
            return validSpriteSequence;
        }
    }
}
