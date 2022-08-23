using DispelTools.Common.DataProcessing;
using System.Collections.Generic;

namespace DispelTools.DataPatcher.PatchingStatus
{
    public static class FileCompleted
    {
        public static SimpleDetail Create(string error, string patchedFile, int patchesApplied)
        {
            var details = new List<string>();
            if (error != null)
            {
                details.Add(error);
            }
            details.Add($"Finished patching file {patchedFile}");
            details.Add($"Total patches applied: {patchesApplied}");
            return SimpleDetail.NewDetails(details.ToArray());
        }
    }
}
