namespace SimpLog.FileLog.Models.AppSettings
{
    public class LogTypeObject
    {
        public bool? Log { get; set; }

        public bool? SendEmail { get; set; }

        public bool? SaveInDatabase { get; set; }
    }
}
