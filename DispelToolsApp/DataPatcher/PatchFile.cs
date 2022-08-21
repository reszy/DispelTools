using DispelTools.Common;

namespace DispelTools.DataPatcher
{
    public class PatchFile
    {
        private readonly string destinationFile;
        private readonly string patchFile;

        public PatchFile(string patchFile, string destinationFile)
        {
            this.patchFile = patchFile;
            this.destinationFile = destinationFile;
        }

        public string DestinationFile { get => destinationFile; }
        public string PatchFileName { get => patchFile; }
        public override string ToString() => $"[file: '{PatchFileName}', out: '{DestinationFile}']";
    }
}