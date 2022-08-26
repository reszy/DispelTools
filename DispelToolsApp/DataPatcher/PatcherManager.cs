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

        private readonly Dictionary<string, List<string>> mappedPatches;
        private readonly List<string> unmappedPatches;
        private PatcherParams patcherParams;

        public int FilesToPatchCount => mappedPatches.Count;

        public PatcherManager(IFileSystem fs, IPatcherFactory patcherFactory, BackgroundWorker backgroundWorker)
        {
            this.fs = fs;
            this.patcherFactory = patcherFactory;
            mappedPatches = new Dictionary<string, List<string>>();
            unmappedPatches = new List<string>();
            this.backgroundWorker = backgroundWorker;
            if (backgroundWorker != null)
            {
                backgroundWorker.WorkerReportsProgress = true;
            }
        }

        public PatcherManager(IPatcherFactory patcherFactory, BackgroundWorker backgroundWorker)
            : this(new FileSystem(), patcherFactory, backgroundWorker)
        {
        }

        public string GetPatchMaping()
        {
            var sb = new StringBuilder();
            foreach (var unmapped in unmappedPatches)
            {
                sb.Append(fs.Path.GetFileName(unmapped));
                sb.Append(" ->");
                sb.AppendLine();
            }
            foreach (var patchTarget in mappedPatches)
            {
                foreach (var patch in patchTarget.Value)
                {
                    sb.Append(fs.Path.GetFileName(patch));
                    sb.Append(" -> ");
                    sb.Append(patchTarget.Key);
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        internal void Start()
        {
            var workReporter = new DetailedProgressReporter(backgroundWorker, mappedPatches.Count);
            workReporter.SetText("Patching...");

            int targetCounter = 0;
            foreach (var targetMapping in mappedPatches)
            {
                var filename = fs.Path.GetFileName(targetMapping.Key);
                targetCounter++;
                workReporter.StartNewStage(targetCounter, $"Patching {filename}");
                var patcher = patcherFactory.CreateInstance();
                try
                {
                    patcher.Initialize(targetMapping.Value, targetMapping.Key, workReporter);
                    patcher.PatchFile(patcherParams.options, workReporter);
                }
                catch (Exception e)
                {
                    workReporter.ReportError(e.Message);
                }
                workReporter.ReportFinishedStage();
            }
            workReporter.ReportDetails(SimpleDetail.NewDetails($"Finished patching {targetCounter} files.", $"Errors count: {workReporter.ErrorsCount}"));
        }

        public void SetParams(PatcherParams patcherParams)
        {
            this.patcherParams = patcherParams;
            if (string.IsNullOrEmpty(patcherParams.TargetFileName))
            {
                MapToGameFiles();
            }
            else
            {
                mappedPatches.Clear();
                unmappedPatches.Clear();
                mappedPatches[patcherParams.TargetFileName] = patcherParams.PatchesFilenames;
            }
        }

        public void MapToGameFiles()
        {
            mappedPatches.Clear();
            unmappedPatches.Clear();

            var targetCache = new Dictionary<string, string>();
            foreach (var patch in patcherParams.PatchesFilenames)
            {
                var patchFileName = fs.Path.GetFileName(patch);
                patchFileName = patchFileName.Substring(0, fs.Path.GetFileNameWithoutExtension(patchFileName).IndexOf('.'));

                if (targetCache.ContainsKey(patchFileName))
                {
                    mappedPatches[targetCache[patchFileName]].Add(patch);
                }
                else
                {
                    string searchPattern = patchFileName + patcherFactory.OutputFileExtension;
                    IEnumerable<string> fileCandidates = fs.Directory.EnumerateFiles(Settings.GameRootDir, searchPattern, SearchOption.AllDirectories)
                    .Where(name => !name.EndsWith("bak"));
                    if (fileCandidates.Count() == 1)
                    {
                        var target = fileCandidates.First();
                        targetCache[patchFileName] = target;

                        mappedPatches[target] = new List<string>
                        {
                            patch
                        };
                    }
                    else
                    {
                        unmappedPatches.Add(patch);
                    }
                }
            }
        }
    }
}
