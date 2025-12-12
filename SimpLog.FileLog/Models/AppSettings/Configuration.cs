using System.Text.Json.Serialization;

namespace SimpLog.FileLog.Models.AppSettings
{
    public class Configuration
    {
        [JsonPropertyName("Main_Configuration")]
        public MainConfiguration? Main_Configuration { get; set; } = new ();

        [JsonPropertyName("File_Configuration")]
        public FileConfiguration? File_Configuration { get; set; } = new ();

        [JsonPropertyName("LogType")]
        public Log? LogType { get; set; } = new();
    }
}
