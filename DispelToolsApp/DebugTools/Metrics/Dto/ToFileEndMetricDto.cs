namespace DispelTools.DebugTools.Metrics.Dto
{
    public class ToFileEndMetricDto : IMetricDto
    {
        private readonly long lengthInBytes;
        private readonly string fileType;

        public ToFileEndMetricDto(long lengthInBytes, string fileType)
        {
            this.lengthInBytes = lengthInBytes;
            this.fileType = fileType;
        }

        public string MetricName => "ToFileEnd." + fileType;

        public MetricType MetricType => MetricType.COUNT;

        public object MetricValue => lengthInBytes;
    }
}
