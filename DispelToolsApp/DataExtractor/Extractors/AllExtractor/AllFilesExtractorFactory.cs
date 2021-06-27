using System.Collections.Generic;
using System.ComponentModel;

namespace DispelTools.DataExtractor.AllExtractor
{
    public class AllFilesExtractorFactory : IExtractorFactory
    {
        public string ExtractorName => "AllExtractor";

        public string FileFilter => "";

        public ExtractionManager.ExtractorType Type => ExtractionManager.ExtractorType.DIRECTORY;

        public ExtractionParams.OptionNames AcceptedOptions => ExtractionParams.OptionNames.AnimatedGifs;

        public Extractor CreateInstance() => new AllFilesExtractor();
    }
}
