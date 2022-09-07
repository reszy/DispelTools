using DispelTools.Common.DataProcessing;
using System.ComponentModel;

namespace DispelTools.DataExtractor.ExtractionStatus
{
    public class ExtractionWorkReporter : DetailedProgressReporter
    {
        public int FilesCreatedCount { get; private set; }
        public ExtractionWorkReporter(BackgroundWorker backgroundWorker, int stageCount = 1) : base(backgroundWorker, stageCount) { }

        public void PrepareWorkerForProcess(ExtractionFileProcess process)
        {
            PrepareWorker((int)process.Stream.Length); //all files are below 2GB
        }

        public void ReportFileExtractionStart(ExtractionFileProcess process)
        {
            PrepareWorkerForProcess(process);
            ReportProgress(0);
            ReportDetails(new SimpleDetail($"Extracting from file: {process.Filename + process.Extension}"));
        }

        public void ReportFileCreated(ExtractionFileProcess process, string createdFilename, bool reportProgress = true)
        {
            if (reportProgress)
            {
                ReportProgress((int)process.Stream.Position);
            }
            ReportDetails(new SimpleDetail($"Created file: {createdFilename}"));
            process.FilesCreated++;
            FilesCreatedCount++;
        }

        public void ReportFileComplete(ExtractionFileProcess process)
        {
            ReportFinishedStage(new SimpleDetail($"Total files created: {process.FilesCreated}", ""));
        }

        /// <summary>
        /// Prints:
        /// From {fromFilesTotal} files, created {FilesCreatedCount} files total.
        /// Errors count: {ErrorsCount}, Skips: {SkipCount}
        /// </summary>
        /// <param name="fromFilesTotal">Count of files selected for extraction</param>
        public void ReportFinishedExtraction(int fromFilesTotal)
        {
            ReportDetails(new SimpleDetail(
                $"Finished extracting {fromFilesTotal} files",
                $"Created files: {FilesCreatedCount}, Errors: {ErrorsCount}, Skips: {SkipCount}"
                ));
        }
    }
}
