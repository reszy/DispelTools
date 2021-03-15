using System.Collections.Generic;
using System.Linq;

namespace DispelTools.DataExtractor
{
    public abstract class Extractor
    {
        public abstract void ExtractFile(ExtractionFileProcess process);

        public virtual List<ExtractionFile> Initialize(ExtractionManager extractionManager, List<string> filenames, string outputDirectory) => filenames.Select((filename) => new ExtractionFile(filename, outputDirectory)).ToList();
    }
}
