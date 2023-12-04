namespace SimpLog.FileLog.Models.AppSettings
{
    internal class Configuration
    {
        public MainConfiguration? Main_Configuration { get; set; }

        public FileConfiguration? File_Configuration { get; set; }

        public Log? LogType { get; set; }
    }
}
