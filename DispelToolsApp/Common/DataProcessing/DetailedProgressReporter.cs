using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispelTools.Common.DataProcessing
{
    public class DetailedProgressReporter : WorkReporter
    {
        public DetailedProgressReporter(BackgroundWorker backgroundWorker, int stageCount = 1) : base(backgroundWorker, stageCount) { }

        protected void PrepareWorker(int totalStageProgress) => totalInStage = totalStageProgress;
        public void ReportDetails(SimpleDetail detail) => backgroundWorker.ReportProgress(currentProgress, detail);

        public void ReportFinishedStage(SimpleDetail completedDetail)
        {
            ReportProgress(totalInStage);
            ReportDetails(completedDetail);
        }
        public void SetText(string text) => backgroundWorker.ReportProgress(currentProgress, text);
    }
}
