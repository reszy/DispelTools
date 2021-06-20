namespace DispelTools.DebugTools.Metrics.Dto
{
    public class SpriteGapMetricDto : IMetricDto
    {
        private readonly int gapLengthInBytes;

        public SpriteGapMetricDto(int gapLengthInBytes)
        {
            this.gapLengthInBytes = gapLengthInBytes;
        }

        public string MetricName => "SpriteFile.SpriteGap";

        public MetricType MetricType => MetricType.COUNT;

        public object MetricValue => gapLengthInBytes;
    }
}
