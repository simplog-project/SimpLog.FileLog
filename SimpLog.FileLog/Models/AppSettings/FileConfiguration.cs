using System;
using System.IO;
using System.Text.Json.Serialization;

namespace SimpLog.FileLog.Models.AppSettings
{
    public class FileConfiguration
    {
        [JsonPropertyName("PathToSaveLogs")]
        public string PathToSaveLogs { get; set; } = Path.GetTempPath();

        [JsonPropertyName("LogFileName")]
        public string LogFileName { get; set; } = $"SimpLog_{DateTime.Now.DayOfYear}.txt";

        [JsonPropertyName("Enable_File_Log")]
        public bool Enable_File_Log { get; set; } = false;
    }
}
