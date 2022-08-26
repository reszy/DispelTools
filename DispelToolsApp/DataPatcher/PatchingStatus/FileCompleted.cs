using DispelTools.Common.DataProcessing;

namespace DispelTools.DataPatcher.PatchingStatus
{
    public static class FileCompleted
    {
        public static SimpleDetail Create(string patchedFile, int patchesApplied)
        {
            return new SimpleDetail($"Finished patching file {patchedFile}", $"Total patches applied: {patchesApplied}");
        }
    }
}
