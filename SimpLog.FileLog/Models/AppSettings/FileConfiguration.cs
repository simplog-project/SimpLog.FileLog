namespace SimpLog.FileLog.Models.AppSettings
{
    internal class FileConfiguration
    {
        public string? PathToSaveLogs { get; set; }

        public string? LogFileName { get; set; }

        public bool? Enable_File_Log { get; set; }
    }
}
