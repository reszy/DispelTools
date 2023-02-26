using System;
using System.ComponentModel;

namespace DispelTools.Common.DataProcessing
{
    public class WorkReporter
    {
        private const int STAGE_MAX = 1000;

        protected readonly BackgroundWorker backgroundWorker;
        protected readonly int stageCount;
        protected int currentStage = 0;
        protected int totalInStage;
        protected int currentProgress;

        public int Max => stageCount * STAGE_MAX;
        public int StagesLeft => stageCount - currentStage;

        /// <summary>
        /// Allows to decide what to do during process.
        /// Return true if continue or false to abort
        /// </summary>
        public Func<WorkerWarning, bool>? DecideOnWarning { get; set; }

        public WorkReporter(BackgroundWorker backgroundWorker, int stageCount = 1)
        {
            if (stageCount < 1)
            {
                throw new ArgumentException("stage count is below 1");
            }
            this.backgroundWorker = backgroundWorker ?? throw new ArgumentNullException(nameof(backgroundWorker));
            this.stageCount = stageCount;
            this.backgroundWorker.WorkerReportsProgress = true;
        }
        public void SetTotal(int total) => totalInStage = total;
        public void StartNewStage(int stage, string text)
        {
            if (stage < 1 || stage > stageCount)
            {
                throw new ArgumentOutOfRangeException(nameof(stage), "stage count out of range");
            }
            currentStage = stage;
            currentProgress = (currentStage - 1) * STAGE_MAX;
            backgroundWorker.ReportProgress(currentProgress, text);

        }
        public void ReportProgress(int progress)
        {
            currentProgress = (currentStage - 1) * STAGE_MAX + (int)((double)progress / totalInStage * STAGE_MAX);
            backgroundWorker.ReportProgress(currentProgress);
        }
        public void SetText(string text) => backgroundWorker.ReportProgress(currentProgress, text);

        public void ReportWarning(string message) => backgroundWorker.ReportProgress(currentProgress, new WorkerWarning(message));

        public bool AskUserIfCanProceed(WorkerWarning warning) => DecideOnWarning?.Invoke(warning) ?? false;

        public record WorkerWarning(string Message);
    }
}
