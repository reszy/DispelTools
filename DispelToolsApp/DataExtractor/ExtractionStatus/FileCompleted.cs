using System.Collections.Generic;

namespace DispelTools.DataExtractor.ExtractionStatus
{
    public static class FileCompleted
    {
        public static SimpleDetail Create(string error, string filename, int filesCreated)
        {
            var details = new List<string>();
            if (error != null)
            {
                details.Add(error);
            }
            details.Add($"Finished extracting from file {filename}");
            details.Add($"Total files created: { filesCreated}");
            return SimpleDetail.NewDetails(details.ToArray());
        }
    }
}
