﻿using DispelTools.Common.DataProcessing;
using System.ComponentModel;

namespace DispelTools.DataExtractor.ExtractionStatus
{
    public class ExtractionWorkReporter : DetailedProgressReporter
    {
        public int FilesCreatedCount { get; private set; }
        public ExtractionWorkReporter(BackgroundWorker backgroundWorker, int stageCount = 1) : base(backgroundWorker, stageCount) { }

        public void PrepareWorkerForProcess(ExtractionFileProcess process) => PrepareWorker((int)process.Stream.Length); //all files are below 2GB
        public void ReportFileCreated(ExtractionFileProcess process, string filename)
        {
            ReportProgress((int)process.Stream.Position);
            ReportDetails(new SimpleDetail($"Created file: {filename}"));
            FilesCreatedCount++;
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
                $"From {fromFilesTotal} files, created {FilesCreatedCount} files total.",
                $"Errors count: {ErrorsCount}, Skips: {SkipCount}"
                ));
        }
    }
}
