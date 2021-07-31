using DispelTools.DataExtractor.ExtractionStatus;
using DispelTools.DebugTools.MetricTools;
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
        private int currentFileInProcess;
        private int errosOccured = 0;
        private readonly Extractor extractor;

        private List<ExtractionFile> preparedFiles;

        public ExtractionParams ExtractionParams { get; private set; }

        public enum ExtractorType { MULTI_FILE, DIRECTORY }

        public ExtractionManager(IFileSystem fs, Extractor extractor, ExtractionParams extractionParams, BackgroundWorker backgroundWorker)
        {
            this.fs = fs;
            this.backgroundWorker = backgroundWorker;
            if (backgroundWorker != null)
            {
                backgroundWorker.WorkerReportsProgress = true;
            }
            this.filenames = extractionParams.Filename;
            this.outputDirectory = extractionParams.OutputDirectory;
            this.extractor = extractor;
            ExtractionParams = extractionParams;
        }

        public ExtractionManager(Extractor extractor, ExtractionParams extractionParams, BackgroundWorker backgroundWorker)
            : this(new FileSystem(), extractor, extractionParams, backgroundWorker)
        {
        }

        public int Prepare()
        {
            preparedFiles = PrepareListOfFilesToExtract();
            return preparedFiles.Count;
        }

        public void Start()
        {
            int createdFilesCount = 0;

            var workReporter = new ExtractionWorkReporter(backgroundWorker, preparedFiles.Count);
            workReporter.SetText("Extracting...");
            for (currentFileInProcess = 0; currentFileInProcess < preparedFiles.Count; currentFileInProcess++)
            {
                using (var fileProcess = preparedFiles[currentFileInProcess].CreateProcess(ExtractionParams, workReporter))
                {
                    string errorMessage = null;
                    workReporter.StartNewStage(currentFileInProcess + 1, null);
                    workReporter.PrepareWorkerForProcess(fileProcess);
                    try
                    {
                        extractor.ExtractFile(fileProcess);
                    }
                    catch (Exception e)
                    {
                        Metrics.List(MetricFile.SpriteFileMetric, $"ExtractorError.{extractor.GetType().Name}.{fileProcess.Filename}", $"{e.Message} p: {fileProcess.File.BaseStream.Position}");
                        errorMessage = $"Error: {e.Message}";
                        errosOccured++;
                    }
                    var resultDetails = fileProcess.ResultDetails;
                    workReporter.ReportDetails(FileCompleted.Create(errorMessage ?? resultDetails.ErrorMessage, fileProcess.Filename, fileProcess.FilesCreated));
                    if (resultDetails.ErrorMessage != null && resultDetails.ErrorMessage.Length > 0)
                    {
                        errosOccured++;
                    }
                    createdFilesCount += resultDetails.FilesCreated;
                }
            }
            workReporter.ReportDetails(SimpleDetail.NewDetails(
                $"From {preparedFiles.Count} files, created {createdFilesCount} files total.",
                $"Errors count: {errosOccured}"));
        }

        private List<ExtractionFile> PrepareListOfFilesToExtract()
        {
            try
            {
                return extractor.Initialize(filenames, outputDirectory);
            }
            catch (Exception e)
            {
                backgroundWorker.ReportProgress(0, $"Error: {e.Message}");
                errosOccured++;
                return new List<ExtractionFile>();
            }
        }
    }
}

