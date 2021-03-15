namespace DispelTools.DataExtractor.ExtractionStatus
{
    public class ProgressChanged
    {
        private int filePositionPercentage;
        private int fileInProcess;
        private int filesInTotal;

        public string Action { get; private set; }
        public int FilesInTotal { get => filesInTotal; private set => filesInTotal = value; }
        public int CurrentProgress => fileInProcess * 100 + FilePositionPercentage;
        public int ProgressTotal => filesInTotal * 100;
        public int FilePositionPercentage { get => filePositionPercentage; set => filePositionPercentage = value; }

        public static ProgressChanged FileCompleted(string error, params string[] results)
        {
            string action = "";
            if (error != null)
            {
                action += error + "\r\n";
            }
            foreach (string s in results)
            {
                action += s + "\r\n";
            }
            return new ProgressChanged()
            {
                Action = action,
                FilePositionPercentage = 100
            };
        }
        public static ProgressChanged CreateAsWorker(long filePosition, long fileLength, string action = null)
        {
            return new ProgressChanged
            {
                Action = action,
                FilePositionPercentage = (int)(100 * (double)filePosition / fileLength)
            };
        }

        public void CompleteStatus(int fileNumber, int filesTotal)
        {
            fileInProcess = fileNumber;
            filesInTotal = filesTotal;
        }
    }
}
