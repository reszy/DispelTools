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
        public int ErrorsCount { get; private set; }
        public DetailedProgressReporter(BackgroundWorker backgroundWorker, int stageCount = 1) : base(backgroundWorker, stageCount) { }

        protected void PrepareWorker(int totalStageProgress) => totalInStage = totalStageProgress;
        public void ReportDetails(SimpleDetail detail) => backgroundWorker.ReportProgress(currentProgress, detail);

        public void ReportFinishedStage(SimpleDetail completedDetail)
        {
            ReportProgress(totalInStage);
            ReportDetails(completedDetail);
        }

        public void ReportError(string message, string secondaryMessage = null)
        {
            ErrorsCount++;
            backgroundWorker.ReportProgress(currentProgress, new ErrorDetail(message, secondaryMessage));
        }

        /// <summary>
        /// Sends text as userState(Not like details). Used to set text on progress bar not for details panel.
        /// </summary>
        /// <param name="text">Text to put on bar</param>
        public void SetText(string text) => backgroundWorker.ReportProgress(currentProgress, text);
    }
}
