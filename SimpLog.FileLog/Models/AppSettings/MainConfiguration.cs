using System.IO;

namespace SimpLog.FileLog.Models.AppSettings
{
    public class MainConfiguration
    {
        public string WhyLogIsNotWorkingPath { get; set; } = Path.GetTempPath();

        public string WhyLogIsNotWorkingFileName { get; set; } = "SimpLog_Error.txt";

        public bool Disable_Log { get; set; } = false;
    }
}
