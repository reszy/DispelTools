using DispelTools.Common;
using DispelTools.GameDataModels.Map.Reader;
using DispelTools.GameDataModels.Sprite;

namespace DispelTools.DataExtractor.MapExtractor
{
    public class MapImageExtractor : Extractor
    {
        public override void ExtractFile(ExtractionFileProcess process)
        {
            var mapReader = new MapReader(process.Filename, process.WorkReporter);
            var container = mapReader.ReadMap(process.File, false);
            for (int i = 0; i < container.InternalSprites.Count; i++)
            {
                var sequence = container.InternalSprites[i];
                SaveSequence(process, sequence, i);
            }
        }

        private void SaveSequence(ExtractionFileProcess process, SpriteSequence sequence, int imageNumber)
        {
            string createdFileName = $"{process.Filename}.{imageNumber}";

            if (!Settings.ExtractorReadOnly)
            {
                sequence.SaveAsImage(process.OutputDirectory, createdFileName, process.Options.CreateAnimatedGifs, process.Options.BlackAsTransparent);
                process.WorkReporter.ReportFileCreated(process, createdFileName);
            }
        }
    }
}
