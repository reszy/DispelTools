using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;

namespace DispelTools.DataExtractor
{
    public abstract class Extractor
    {
        protected IFileSystem fs;
        protected Extractor()
        {
            fs = new FileSystem();
        }
        protected Extractor(IFileSystem fs)
        {
            this.fs = fs;
        }

        public abstract void ExtractFile(ExtractionFileProcess process);

        public virtual List<ExtractionFile> Initialize(List<string> filenames, string outputDirectory) => filenames.Select((filename) => new ExtractionFile(filename, outputDirectory)).ToList();
    }
}
