using DispelTools.Common.DataProcessing;

namespace DispelTools.DataPatcher.PatchingStatus
{
    public static class FileCompleted
    {
        public static SimpleDetail Create(int patchesApplied)
        {
            return new SimpleDetail($"Total patches applied: {patchesApplied}", "");
        }
    }
}
