using System.Collections.Generic;

namespace DispelTools.DataPatcher
{
    public class PatcherParams
    {
        public PatcherParams() { }
        public PatcherParams(List<string> newList, PatcherParams patcherParams)
        {
            PatchesFilenames = newList;
            TargetFileName = patcherParams.TargetFileName;
            options.KeepBackupFiles = patcherParams.options.KeepBackupFiles;
            options.KeepImageSize = patcherParams.options.KeepImageSize;
        }

        public class PatcherOptions
        {
            public bool KeepBackupFiles { get; set; } = true;
            public bool KeepImageSize { get; set; } = true;
        }

        public enum OptionNames { KeepBackupFiles = 1, KeepImageSize = 2 }
        public static OptionNames NoOptions { get; } = 0;
        public bool HaveFilledRequiredParams => PatchesFilenames != null && PatchesFilenames.Count > 0;

        //Required
        public List<string> PatchesFilenames { get; set; }

        //Optional
        public string TargetFileName { get; set; }
        public PatcherOptions options { get; } = new PatcherOptions();

    }
}
