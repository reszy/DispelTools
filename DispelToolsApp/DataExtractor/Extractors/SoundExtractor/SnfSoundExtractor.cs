using DispelTools.Common;
using System.IO;

namespace DispelTools.DataExtractor.SoundExtractor
{
    public class SnfSoundExtractor : Extractor
    {
        private static readonly short PCMAudioFormat = 1;
        private static readonly short NumberOfChannels = 1;

        public override void ExtractFile(ExtractionFileProcess process)
        {
            var file = process.File;
            int datasize = file.ReadInt32();
            if (datasize == file.BaseStream.Length - 22)
            {
                file.ReadInt32();
                int sampleRate = file.ReadInt32();
                file.ReadInt32();
                file.ReadInt16();
                short bitsPerSample = file.ReadInt16();
                file.ReadInt16();
                if (!Settings.ExtractorReadOnly)
                {
                    SaveToWaveFormat(process, file, datasize, sampleRate, bitsPerSample);
                }
            }
            else
            {
                throw new ExtractException($"{process.ExceptionMessagePrefix()} expected data size of ({file.BaseStream.Length - 22}) was ({datasize})");
            }
        }

        private void SaveToWaveFormat(ExtractionFileProcess process, BinaryReader reader, int dataSize, int sampleRate, short bitsPerSample)
        {
            var outputFileName = $"{process.Filename}.wav";
            using (var writer = new BinaryWriter(new FileStream($"{process.OutputDirectory}\\{outputFileName}", FileMode.Create)))
            {
                WriteRiff(writer, dataSize);
                WriteFmt(writer, sampleRate, bitsPerSample);
                WriteData(writer, reader, dataSize);
            }
            process.WorkReporter.ReportFileCreated(process, outputFileName);
        }

        private void WriteRiff(BinaryWriter writer, int dataSize)
        {
            writer.Write(new char[4] { 'R', 'I', 'F', 'F' });
            writer.Write(44 + dataSize);
            writer.Write(new char[4] { 'W', 'A', 'V', 'E' });
        }

        private void WriteFmt(BinaryWriter writer, int sampleRate, short bitsPerSample)
        {
            writer.Write(new char[4] { 'f', 'm', 't', ' ' });
            writer.Write(16);
            writer.Write(PCMAudioFormat);
            writer.Write(NumberOfChannels);
            writer.Write(sampleRate);
            writer.Write(sampleRate * NumberOfChannels * (bitsPerSample / 8));
            writer.Write((short)(NumberOfChannels * (bitsPerSample / 8)));
            writer.Write(bitsPerSample);
        }

        private void WriteData(BinaryWriter writer, BinaryReader reader, int dataSize)
        {
            writer.Write(new char[4] { 'd', 'a', 't', 'a' });
            writer.Write(dataSize);
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                writer.Write(reader.ReadBytes(2));
            }
        }
    }
}
