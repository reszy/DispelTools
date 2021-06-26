using DispelTools.Common;
using DispelTools.ImageProcessing.Sprite;
using System;
using System.IO;

namespace DispelTools.DataExtractor.MapExtractor
{
    public class MapImageExtractor : Extractor
    {
        public override void ExtractFile(ExtractionFileProcess process)
        {
            SeekForSprites(process);
            var file = process.File;
            int spritesCount = file.ReadInt32();

            var spriteLoader = new SpriteLoader(file, process.Filename);
            for (int i = 0; i < spritesCount; i++)
            {
                int imageStamp = file.ReadInt32();
                int imageOffset = imageStamp == 6 ? 1904 : (imageStamp == 9 ? 2996 : throw new NotImplementedException($"Unexpected imageStamp {imageStamp}"));
                file.Skip(264);
                LoadAndSaveSequence(process, spriteLoader, i);
                file.Skip(imageOffset);
            }
        }

        private void SeekForSprites(ExtractionFileProcess process)
        {
            var file = process.File;
            process.Stream.Seek(8, SeekOrigin.Begin);//Set position after map dimensions
            int multiplier = file.ReadInt32();
            int size = file.ReadInt32();
            file.BaseStream.Seek(8, SeekOrigin.Begin);
            file.Skip(multiplier * size * 4);//skip unknown data

            size = file.ReadInt32();
            file.Skip(size * 2);
        }
        private void LoadAndSaveSequence(ExtractionFileProcess process, SpriteLoader loader, int imageNumber)
        {
            var sequence = loader.LoadSequence();
            string createdFileName = $"{process.Filename}.{imageNumber}";

            if (!Settings.ExtractorReadOnly)
            {
                sequence.SaveAsImage(process.OutputDirectory, createdFileName);
                process.Extractor.RaportFileCreatedDetail(process, createdFileName);
            }
            sequence.Dispose();
        }
    }
}
