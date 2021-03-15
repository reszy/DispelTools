using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DispelTools.DataExtractor.ExtractionManager;

namespace DispelTools.DataExtractor
{
    public interface IExtractorFactory
    {
        string ExtractorName { get; }
        string FileFilter { get; }

        ExtractorType type { get; }

        ExtractionManager CreateExtractorInstance(List<string> filename, string outputDirectory, System.ComponentModel.BackgroundWorker backgroundWorker);
    }
}
