using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.DataExtractor.ImageExtractor
{
    public class SprImageExtractorFactory : IExtractorFactory
    {
        public string ExtractorName => "ImageExtractor (SPR)";

        public string FileFilter => "sprite (*.SPR)|*.SPR;*.spr|All files (*.*)|*.*";

        public ExtractionManager.ExtractorType type => ExtractionManager.ExtractorType.MULTI_FILE;

        public ExtractionManager CreateExtractorInstance(List<string> filenames, string outputDirectory, BackgroundWorker backgroundWorker) => new ExtractionManager(new SprImageExtractor(), filenames, outputDirectory, backgroundWorker);
    }
}
