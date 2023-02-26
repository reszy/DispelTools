using DispelTools.Common.DataProcessing;
using System.IO.Abstractions;

namespace DispelTools.DataEditor.Export
{
    public abstract class Exporter
    {
        protected readonly IFileSystem fs;
        protected readonly WorkReporter workReporter;

        public Exporter(IFileSystem fs, WorkReporter workReporter)
        {
            this.fs = fs;
            this.workReporter = workReporter;
        }
        public abstract void Export(string path);
    }
}
