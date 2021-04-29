using System.Collections.Generic;
using System.ComponentModel;

namespace DispelTools.DataExtractor.RgbConverter
{
    internal class RgbConverterFactory : IExtractorFactory
    {
        public string ExtractorName => "RgbConverter";

        public string FileFilter => "PNG file|*.png";

        public ExtractionManager.ExtractorType type => ExtractionManager.ExtractorType.MULTI_FILE;

        public ExtractionManager CreateExtractorInstance(List<string> filenames, string outputDirectory, BackgroundWorker backgroundWorker) => new ExtractionManager(new Rgb24Converter(), filenames, outputDirectory, backgroundWorker);
    }
}
