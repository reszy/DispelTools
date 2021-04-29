namespace DispelTools.DataExtractor.ExtractionStatus
{
    public class StatusNameChanged
    {
        public string ExtraStatusName { get; set; } = null;
        public static StatusNameChanged NewStatus(string status)
        {
            return new StatusNameChanged
            {
                ExtraStatusName = status
            };
        }
        public static StatusNameChanged NewStatusInProgress(string status)
        {
            return new StatusNameChanged
            {
                ExtraStatusName = status + "..."
            };
        }
        public static StatusNameChanged Completed()
        {
            return new StatusNameChanged
            {
                ExtraStatusName = "Completed"
            };
        }
    }
}
