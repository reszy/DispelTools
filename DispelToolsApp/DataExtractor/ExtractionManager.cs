using DispelTools.Common.DataProcessing;
using DispelTools.DataExtractor.ExtractionStatus;
using DispelTools.DebugTools.MetricTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Abstractions;

namespace DispelTools.DataExtractor
{
    public partial class ExtractionManager
    {
        public IFileSystem fs;
        private readonly BackgroundWorker backgroundWorker;
        private readonly List<string> filenames;
        private readonly string outputDirectory;
        private int currentFileInProcess;
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
            var workReporter = new ExtractionWorkReporter(backgroundWorker, preparedFiles.Count);
            workReporter.SetText("Extracting...");
            for (currentFileInProcess = 0; currentFileInProcess < preparedFiles.Count; currentFileInProcess++)
            {
                using (var fileProcess = preparedFiles[currentFileInProcess].CreateProcess(ExtractionParams, workReporter))
                {
                    workReporter.StartNewStage(currentFileInProcess + 1, null);
                    workReporter.ReportFileExtractionStart(fileProcess);
                    try
                    {
                        extractor.ExtractFile(fileProcess);
                    }
                    catch (UnsupportedFileException e)
                    {
                        workReporter.ReportSkip($"Extension {fileProcess.Extension} not supported");
                    }
                    catch (Exception e)
                    {
                        Metrics.List(MetricFile.SpriteFileMetric, $"ExtractorError.{extractor.GetType().Name}.{fileProcess.Filename}", $"{e.Message} p: {fileProcess.File.BaseStream.Position}");
                        workReporter.ReportError(e.Message);
                        System.Diagnostics.Debug.WriteLine(e.ToString());
                    }
                    var resultDetails = fileProcess.ResultDetails;
                    workReporter.ReportFileComplete(fileProcess);
                }
            }
            workReporter.ReportFinishedExtraction(preparedFiles.Count);
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
                return new List<ExtractionFile>();
            }
        }

        public ExtractorValidationResult Validate(out string returnMessage)
        {
            if (extractor is null)
            {
                returnMessage = "Extractor is not initialized";
                return ExtractorValidationResult.Error;
            }
            else
            {
                return extractor.Validate(filenames, out returnMessage);
            }
        }
    }
}

