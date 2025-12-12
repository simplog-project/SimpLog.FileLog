using System.Text.Json.Serialization;

namespace SimpLog.FileLog.Models.AppSettings
{
    public class Log
    {
        [JsonPropertyName("Trace")]
        public LogTypeObject Trace { get; set; } = new();

        [JsonPropertyName("Debug")]
        public LogTypeObject Debug { get; set; } = new();

        [JsonPropertyName("Info")]
        public LogTypeObject Info { get; set; } = new();

        [JsonPropertyName("Notice")]
        public LogTypeObject Notice { get; set; } = new();

        [JsonPropertyName("Warn")]
        public LogTypeObject Warn { get; set; } = new();

        [JsonPropertyName("Error")]
        public LogTypeObject Error { get; set; } = new();

        [JsonPropertyName("Fatal")]
        public LogTypeObject Fatal { get; set; } = new();
    }
}
