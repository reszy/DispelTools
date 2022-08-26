using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Abstractions;
using System.Text;
using System.Threading.Tasks;
using DispelTools.Common.DataProcessing;

namespace DispelTools.DataPatcher
{
    public abstract class Patcher
    {
        protected IFileSystem fs;
        protected Patcher()
        {
            fs = new FileSystem();
        }
        protected Patcher(IFileSystem fs)
        {
            this.fs = fs;
        }

        public abstract int Count { get; }
        public abstract void PatchFile(PatcherParams.PatcherOptions options, DetailedProgressReporter workReporter);

        public abstract void Initialize(List<string> patches, string targetFile, DetailedProgressReporter workReporter);
    }
}
