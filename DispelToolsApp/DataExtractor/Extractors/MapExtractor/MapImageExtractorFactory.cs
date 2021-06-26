using System.Collections.Generic;
using System.ComponentModel;

namespace DispelTools.DataExtractor.MapExtractor
{
    public class MapImageExtractorFactory : IExtractorFactory
    {
        public string ExtractorName => "MapImageExtractor";

        public string FileFilter => "sprite (*.map,*.btl,*.gtl)|*.map;*.btl;*.gtl";

        public ExtractionManager.ExtractorType type => ExtractionManager.ExtractorType.MULTI_FILE;

        public ExtractionManager CreateExtractorInstance(List<string> filenames, string outputDirectory, BackgroundWorker backgroundWorker) => new ExtractionManager(new MapImageExtractor(), filenames, outputDirectory, backgroundWorker);
    }
}
