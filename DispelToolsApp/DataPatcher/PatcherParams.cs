using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.DataPatcher
{
    public class PatcherParams
    {
        public PatcherParams() { }
        public PatcherParams(List<string> newList, PatcherParams patcherParams)
        {
            PatchesFilenames = newList;
            TargetFileName = patcherParams.TargetFileName;
            KeepBackupFiles = patcherParams.KeepBackupFiles;
            KeepImageSize = patcherParams.KeepImageSize;
        }

        public enum OptionNames { KeepBackupFiles = 1, KeepImageSize = 2 }
        public static OptionNames NoOptions { get; } = 0;
        //Required
        public List<string> PatchesFilenames { get; set; }
        public string TargetFileName { get; set; }
        //Optional
        public bool KeepBackupFiles { get; set; } = true;
        public bool KeepImageSize { get; set; } = true;

        public bool HaveFilledRequiredParams => PatchesFilenames != null && PatchesFilenames.Count > 0;
    }
}
