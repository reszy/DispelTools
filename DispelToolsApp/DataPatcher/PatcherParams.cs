using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.DataPatcher
{
    public class PatcherParams
    {
        public enum OptionNames { KeepBackupFiles = 1, KeepImageSize = 2 }
        public static OptionNames NoOptions { get; } = 0;
        //Required
        public List<string> PatchesFilenames { get; set; }
        public List<string> TargetFileNames { get; set; }
        //Optional
        public bool KeepBackupFiles { get; set; } = true;
        public bool KeepImageSize { get; set; } = true;

        public bool HaveFilledRequiredParams => PatchesFilenames != null && TargetFileNames != null && PatchesFilenames.Count > 0 && TargetFileNames.Count > 0;
    }
}
