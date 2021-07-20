using System;

namespace DispelTools.Viewers.MapViewer
{
    public partial class MapReader
    {
        public class WorkReporter
        {
            public class ProgressReportArgs : EventArgs
            {
                public int Max { get; set; }
                public int Progress { get; set; }
            }
            public void ReportProgress(int progress, int max) => ReportWork?.Invoke(this, new ProgressReportArgs() { Progress = progress, Max = max });
            public delegate void ReportWorkHandler(object sender, ProgressReportArgs e);
            public event ReportWorkHandler ReportWork;
        }
    }
}
