using SimpLog.FileLog.Models;
using SimpLog.FileLog.Services.FileServices;
using System.Threading.Tasks;

namespace SimpLog.FileLog.Services.SimpLogServices
{
    public class SimpLog
    {
        private FileService _fileService = new FileService();

        /// <summary>
        /// If there is no configuration set up in appsettings.json, log is enabled. If there is disabled from the
        /// configuration, take it in mind here.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="saveType"></param>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <returns></returns>
        public async Task Trace(string message, FileSaveType? saveType = FileSaveType.Standart, string? path_to_save_log = null, string? log_file_name = null)
            => await _fileService.Save(message, LogType.Trace, saveType, path_to_save_log, log_file_name);

        public async Task Debug(string message, FileSaveType? saveType = FileSaveType.Standart, string? path_to_save_log = null, string? log_file_name = null)
            => await _fileService.Save(message, LogType.Debug, saveType, path_to_save_log, log_file_name);

        public async Task Info(string message, FileSaveType? saveType = FileSaveType.Standart, string? path_to_save_log = null, string? log_file_name = null)
            => await _fileService.Save(message, LogType.Debug, saveType, path_to_save_log, log_file_name);
        
        public async Task Notice(string message, FileSaveType? saveType = FileSaveType.Standart, string? path_to_save_log = null, string? log_file_name = null)
            => await _fileService.Save(message, LogType.Debug, saveType, path_to_save_log, log_file_name);

        public async Task Warn(string message, FileSaveType? saveType = FileSaveType.Standart, string? path_to_save_log = null, string? log_file_name = null)
            => await _fileService.Save(message, LogType.Debug, saveType, path_to_save_log, log_file_name);

        public async Task Error(string message, FileSaveType? saveType = FileSaveType.Standart, string? path_to_save_log = null, string? log_file_name = null)
            => await _fileService.Save(message, LogType.Debug, saveType, path_to_save_log, log_file_name);

        public async Task Fatal(string message, FileSaveType? saveType = FileSaveType.Standart, string? path_to_save_log = null, string? log_file_name = null)
            => await _fileService.Save(message, LogType.Debug, saveType, path_to_save_log, log_file_name);
    }
}
