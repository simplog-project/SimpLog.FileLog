using System.Text.Json.Serialization;

namespace SimpLog.FileLog.Models.AppSettings
{
    public class Configuration
    {
        public MainConfiguration? Main_Configuration { get; set; } 

        public FileConfiguration? File_Configuration { get; set; }

        public Log? LogType { get; set; }
    }
}
