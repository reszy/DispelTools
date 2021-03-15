using System.Collections.Generic;
using System.ComponentModel;

namespace DispelTools.DataExtractor.SoundExtractor
{
    public class SnfSoundExtractorFactory : IExtractorFactory
    {
        public string ExtractorName => "SoundExtractor (SNF)";

        public string FileFilter => "sound (*.snf)|*.SNF;*.snf";

        public ExtractionManager.ExtractorType type => ExtractionManager.ExtractorType.MULTI_FILE;

        public ExtractionManager CreateExtractorInstance(List<string> filenames, string outputDirectory, BackgroundWorker backgroundWorker) => new ExtractionManager(new SnfSoundExtractor(), filenames, outputDirectory, backgroundWorker);
    }
}
