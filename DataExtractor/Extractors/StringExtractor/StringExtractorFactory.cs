using System.Collections.Generic;
using System.ComponentModel;

namespace DispelTools.DataExtractor.StringExtractor
{
    public class StringExtractorFactory : IExtractorFactory
    {
        public string ExtractorName => "StringExtractor";

        public string FileFilter => "Any file|*.*";

        public ExtractionManager.ExtractorType type => ExtractionManager.ExtractorType.MULTI_FILE;

        public ExtractionManager CreateExtractorInstance(List<string> filenames, string outputDirectory, BackgroundWorker backgroundWorker) => new ExtractionManager(new StringExtractor(), filenames, outputDirectory, backgroundWorker);
    }
}
