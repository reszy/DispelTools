using DispelTools.Common;
using DispelTools.Common.DataProcessing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace DispelTools.DataPatcher
{
    internal class PatcherManager
    {
        private readonly IFileSystem fs;
        private readonly IPatcherFactory patcherFactory;
        private readonly BackgroundWorker backgroundWorker;
        private readonly Patcher patcher;
        private readonly PatcherParams.OptionNames settings;

        private readonly List<string> patchFiles;
        private string targetFile;
        private string targetFileDirectory;

        private int errosOccured = 0;

        public int FilesToPatchCount => patcher.Count;

        public PatcherManager(IFileSystem fs, IPatcherFactory patcherFactory, BackgroundWorker backgroundWorker)
        {
            this.fs = fs;
            this.patcherFactory = patcherFactory;
            patchFiles = new List<string>();
            this.backgroundWorker = backgroundWorker;
            if (backgroundWorker != null)
            {
                backgroundWorker.WorkerReportsProgress = true;
            }
            patcher = patcherFactory.CreateInstance();
        }

        public PatcherManager(IPatcherFactory patcherFactory, BackgroundWorker backgroundWorker)
            : this(new FileSystem(), patcherFactory, backgroundWorker)
        {
        }

        private string GuessPatchedFile(string patchFileName)
        {
            patchFileName = fs.Path.GetFileName(patchFileName);
            patchFileName = patchFileName.Substring(0, fs.Path.GetFileNameWithoutExtension(patchFileName).IndexOf('.'));
            string searchPattern = patchFileName + '*';

            IEnumerable<string> fileCandidates = fs.Directory.EnumerateFiles(Settings.GameRootDir, searchPattern, SearchOption.AllDirectories)
                .Where(name => !name.EndsWith("bak"));
            if (fileCandidates.Count() == 1)
            {
                return fileCandidates.First();
            }
            else
            {
                return null;
            }
        }

        public string GetPatchMaping()
        {
            var sb = new StringBuilder();
            var targetFilename = fs.Path.GetFileName(targetFile);
            foreach (var patch in patchFiles)
            {
                sb.Append(fs.Path.GetFileName(patch));
                sb.Append(" -> ");
                sb.Append(targetFilename);
                sb.AppendLine();
            }
            return sb.ToString();
        }

        internal void Start()
        {
            var workReporter = new WorkReporter(backgroundWorker, patcher.Count);
            patcher.PatchFile(settings, workReporter);
        }

        public void SetPatchFiles(string[] patchFileNames)
        {
            patchFiles.Clear();
            patchFiles.AddRange(patchFileNames);

            targetFile = GuessPatchedFile(patchFiles[0]) ?? targetFile;
        }

        public bool IsReady()
        {
            return !string.IsNullOrEmpty(targetFile) && patchFiles.Count > 0;
        }

        public string TargetFileDirectory => targetFileDirectory;
        public void SetTargetFile(string path)
        {
            targetFile = path;
            targetFileDirectory = fs.Path.GetDirectoryName(targetFile);
        }

        public void Prepare(PatcherParams patcherParams)
        {
            try
            {
                patcher.Initialize(patcherParams.PatchesFilenames, targetFile);
            }
            catch (Exception e)
            {
                backgroundWorker.ReportProgress(0, $"Error: {e.Message}");
                errosOccured++;
            }
        }

        public void MapToGameFiles(PatcherParams patcherParams)
        {

        }
    }
}
