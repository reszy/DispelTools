using System.Collections.Generic;
using System.ComponentModel;

namespace DispelTools.DataExtractor.AllExtractor
{
    public class AllFilesExtractorFactory : IExtractorFactory
    {
        public string ExtractorName => "AllExtractor";

        public string FileFilter => "";

        public ExtractionManager.ExtractorType type => ExtractionManager.ExtractorType.DIRECTORY;

        public ExtractionManager CreateExtractorInstance(List<string> filenames, string outputDirectory, BackgroundWorker backgroundWorker) => new ExtractionManager(new AllFilesExtractor(), filenames, outputDirectory, backgroundWorker);
    }
}
