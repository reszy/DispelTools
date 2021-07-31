using DispelTools.Common;
using System.ComponentModel;

namespace DispelTools.DataExtractor.ExtractionStatus
{
    public class ExtractionWorkReporter : WorkReporter
    {
        public ExtractionWorkReporter(BackgroundWorker backgroundWorker, int stageCount = 1) : base(backgroundWorker, stageCount) { }

        public void PrepareWorkerForProcess(ExtractionFileProcess process) => totalInStage = (int)process.Stream.Length; //all files are below 2GB
        public void ReportFileCreated(ExtractionFileProcess process, string filename)
        {
            ReportProgress((int)process.Stream.Position);
            ReportDetails(SimpleDetail.NewDetails($"Created file: {filename}"));
            process.FilesCreated += 1;
        }
        public void SetText(string text) => backgroundWorker.ReportProgress(currentProgress, text);
        public void ReportDetails(SimpleDetail detail) => backgroundWorker.ReportProgress(currentProgress, detail);
    }
}
