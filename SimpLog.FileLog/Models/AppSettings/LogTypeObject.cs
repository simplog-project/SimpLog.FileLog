using System.Text.Json.Serialization;

namespace SimpLog.FileLog.Models.AppSettings
{
    public class LogTypeObject
    {
        [JsonPropertyName("Log")]
        public bool Log { get; set; } = false;

        [JsonPropertyName("SendEmail")]
        public bool SendEmail { get; set; } = false;

        [JsonPropertyName("SaveInDatabase")]
        public bool SaveInDatabase { get; set; } = false;
    }
}
