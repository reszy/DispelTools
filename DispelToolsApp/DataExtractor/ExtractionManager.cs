using DispelTools.DataExtractor.ExtractionStatus;
using DispelTools.DebugTools.Metrics;
using DispelTools.DebugTools.Metrics.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;

namespace DispelTools.DataExtractor
{
    public class ExtractionManager
    {
        public IFileSystem fs;
        private readonly BackgroundWorker backgroundWorker;
        private readonly List<string> filenames;
        private readonly string outputDirectory;
        private int filesAtStartCount;
        private int currentFileInProcess;
        private int errosOccured = 0;
        private bool statusUsed = false;
        private readonly Extractor extractor;

        public enum ExtractorType { MULTI_FILE, DIRECTORY }

        public ExtractionManager(IFileSystem fs, Extractor extractor, List<string> filenames, string outputDirectory, BackgroundWorker backgroundWorker)
        {
            this.fs = fs;
            this.backgroundWorker = backgroundWorker;
            if (backgroundWorker != null)
            {
                backgroundWorker.WorkerReportsProgress = true;
            }
            this.filenames = filenames;
            this.outputDirectory = outputDirectory;
            this.extractor = extractor;
        }

        public ExtractionManager(Extractor extractor, List<string> filenames, string outputDirectory, BackgroundWorker backgroundWorker)
            : this(new FileSystem(), extractor, filenames, outputDirectory, backgroundWorker)
        {
        }

        public void RaportFileCreatedDetail(ExtractionFileProcess process, string filename)
        {
            var progressChanged = ProgressChanged.CreateAsWorker(process.Stream.Position, process.Stream.Length, $"Created file: {filename}");
            progressChanged.CompleteStatus(currentFileInProcess, filesAtStartCount);
            backgroundWorker.ReportProgress(0, progressChanged);
            process.FilesCreated += 1;
        }
        public void ReportNewStatus(StatusNameChanged status) { statusUsed = true; backgroundWorker.ReportProgress(0, status); }

        public void ReportFileCompleted(ProgressChanged progress)
        {
            progress.CompleteStatus(currentFileInProcess, filesAtStartCount);
            backgroundWorker.ReportProgress(0, progress);
        }
        public void ReportDetail(SimpleDetail detail) => backgroundWorker.ReportProgress(0, detail);

        public void Start()
        {
            int createdFilesCount = 0;
            var files = new List<ExtractionFile>();
            try
            {
                files.AddRange(extractor.Initialize(this, filenames, outputDirectory));
                filesAtStartCount = files.Count;
            }
            catch (Exception e)
            {
                ReportFileCompleted(ProgressChanged.FileCompleted($"Error: {e.Message}"));
                errosOccured++;
            }
            for (currentFileInProcess = 0; currentFileInProcess < filesAtStartCount; currentFileInProcess++)
            {
                using (var fileProcess = files[currentFileInProcess].CreateProcess(this))
                {
                    string errorMessage = null;
                    try
                    {
                        extractor.ExtractFile(fileProcess);
                    }
                    catch (Exception e)
                    {
                        FileMetrics.AddMetric(new GeneralFileStatusDto($"ExtractorError.{extractor.GetType().Name}", fileProcess.Filename, $"{e.Message} p: {fileProcess.File.BaseStream.Position}"));
                        errorMessage = $"Error: {e.Message}";
                        errosOccured++;
                    }
                    var resultDetails = fileProcess.ResultDetails;
                    ReportFileCompleted(ProgressChanged.FileCompleted(
                            errorMessage ?? resultDetails.ErrorMessage,
                            $"Finished extracting from file {fileProcess.Filename}",
                            $"Total files created: {resultDetails.FilesCreated}"
                        ));
                    if (resultDetails.ErrorMessage != null && resultDetails.ErrorMessage.Length > 0)
                    {
                        errosOccured++;
                    }
                    createdFilesCount += resultDetails.FilesCreated;
                }
            }
            if (statusUsed)
            {
                ReportNewStatus(StatusNameChanged.Completed());
            }
            ReportDetail(SimpleDetail.NewDetails(
                $"From {filesAtStartCount} files, created {createdFilesCount} files total.",
                $"Errors count: {errosOccured}"));
        }
    }
}

