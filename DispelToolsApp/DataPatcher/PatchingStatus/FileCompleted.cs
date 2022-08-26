using DispelTools.Common.DataProcessing;
using System.Collections.Generic;

namespace DispelTools.DataPatcher.PatchingStatus
{
    public static class FileCompleted
    {
        public static SimpleDetail Create(string patchedFile, int patchesApplied)
        {
            return SimpleDetail.NewDetails($"Finished patching file {patchedFile}", $"Total patches applied: {patchesApplied}");
        }
    }
}
