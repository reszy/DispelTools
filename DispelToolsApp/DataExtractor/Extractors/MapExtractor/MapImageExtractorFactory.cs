using System.Collections.Generic;
using System.ComponentModel;

namespace DispelTools.DataExtractor.MapExtractor
{
    public class MapImageExtractorFactory : IExtractorFactory
    {
        public string ExtractorName => "MapImageExtractor";

        public string FileFilter => "sprite (*.map,*.btl,*.gtl)|*.map;*.btl;*.gtl";

        public ExtractionManager.ExtractorType Type => ExtractionManager.ExtractorType.MULTI_FILE;

        public ExtractionParams.OptionNames AcceptedOptions => ExtractionParams.OptionNames.AnimatedGifs;

        public Extractor CreateInstance() => new MapImageExtractor();
    }
}
