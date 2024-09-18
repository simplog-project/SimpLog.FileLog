using SimpLog.FileLog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using static SimpLog.FileLog.Models.Constants;

namespace SimpLog.FileLog.Services.FileServices
{
    public class FileService
    {
        /// <summary>
        /// FullPath + FileName is the key and value is what should be saved into the log
        /// </summary>
        public static Dictionary<string, StringBuilder> Logs = new Dictionary<string, StringBuilder>();

        public static Models.AppSettings.Configuration configuration = ConfigurationServices.ConfigService.BindConfigObject();

        public readonly bool? _Enable_File_Log = (configuration.File_Configuration.Enable_File_Log == null) ? true : Convert.ToBoolean(configuration.File_Configuration.Enable_File_Log);


        public readonly bool? _Trace_File   = (configuration.LogType.Trace.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Trace.Log);
        public readonly bool? _Debug_File   = (configuration.LogType.Debug.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Debug.Log);
        public readonly bool? _Info_File    = (configuration.LogType.Info.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Info.Log);
        public readonly bool? _Notice_File  = (configuration.LogType.Notice.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Notice.Log);
        public readonly bool? _Warn_File    = (configuration.LogType.Warn.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Warn.Log);
        public readonly bool? _Error_File   = (configuration.LogType.Error.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Error.Log);
        public readonly bool? _Fatal_File   = (configuration.LogType.Fatal.Log == null) ? true : Convert.ToBoolean(configuration.LogType.Fatal.Log);

        /// <summary>
        /// Pass message and check if it is for execution
        /// </summary>
        /// <param name="bufferMessage"></param>
        /// <returns></returns>
        public async Task BufferMessage(string message, LogType bufferMessageType, string path_to_save_log, string log_file_name)
        {
            await AppendMessage(message, bufferMessageType, path_to_save_log, log_file_name);

            //await SaveMessageIntoLogFile();
        }

        /// <summary>
        /// Add message to StringBuilder Message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <returns></returns>
        public async Task AppendMessage(string message, LogType messageType, string path_to_save_log, string log_file_name)
        {
            //  String builder where the message will be saved
            StringBuilder Message = new StringBuilder();

            //  If there was previous information for this file to be saved, take it!
            if (Logs.ContainsKey(path_to_save_log + "\\" + Path.GetFileNameWithoutExtension(log_file_name) + FileFormat))
                Message = Logs[path_to_save_log + "\\" + Path.GetFileNameWithoutExtension(log_file_name) + FileFormat];

            Message.Append(
                DateTime.UtcNow.ToString(DateFormat) +
                Separator +
                MessageType(messageType) +
                Separator +
                message +
                Environment.NewLine);

            Logs[path_to_save_log + "\\" + Path.GetFileNameWithoutExtension(log_file_name) + FileFormat] = Message;
        }

        /// <summary>
        /// Log rotation implemented
        /// </summary>
        /// <returns></returns>
        public async Task FileRenameIfNeeded(string filePath_final, string fileName_final)
        {
            string  fullFilePathName    = filePath_final + PathSeparator + fileName_final + FileFormat;
            int     i                   = 1;
            bool    ready               = false;

            if (!File.Exists(fullFilePathName) || new FileInfo(fullFilePathName).Length < 7485760)
                return;

            do
            {
                if (File.Exists(filePath_final + PathSeparator + fileName_final + i + FileFormat))
                    continue;

                File.Move(fullFilePathName, filePath_final + PathSeparator + fileName_final + i + FileFormat);

                ready = true;
            } while(!ready);
        }

        /// <summary>
        /// Save log message from buffer memory.
        /// </summary>
        /// <returns></returns>
        public async Task SaveMessageIntoLogFile()
        {
            if (Logs.Count <= 0) return;

            foreach (var log in Logs)
            {
                //if (log.Value.Length < 95)
                //    continue;

                await FileRenameIfNeeded(
                    Path.GetDirectoryName(log.Key),
                    Path.GetFileNameWithoutExtension(log.Key));

                if (File.Exists(log.Key))
                {
                    // Edit the file with a larger buffer. 65k.
                    using (StreamWriter sw = new StreamWriter(new FileStream(Path.GetDirectoryName(log.Key) + PathSeparator + Path.GetFileNameWithoutExtension(log.Key) + FileFormat, FileMode.Append)))
                    {
                        sw.Write(log.Value);
                    }
                }
                else
                    await File.WriteAllTextAsync(log.Key, log.Value.ToString());

                Logs.Remove(log.Key);
            }
        }

        /// <summary>
        /// Save the log without putting into a buffer. If a user wants every log to be saved on a different place.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <returns></returns>
        public async Task ImediateSaveMessageIntoLogFile(string message, LogType messageType, string? path_to_save_log, string? log_file_name)
        {
            await FileRenameIfNeeded(path_to_save_log, log_file_name);

            message = DateTime.UtcNow.ToString(DateFormat) + Separator + MessageType(messageType) + Separator + message + Environment.NewLine;

            if (File.Exists(path_to_save_log + PathSeparator + log_file_name + FileFormat))
            {
                // Edit the file with a larger buffer. 65k.
                using (StreamWriter sw = new StreamWriter(new FileStream(path_to_save_log + PathSeparator + log_file_name + FileFormat, FileMode.Append)))
                {
                    sw.Write(message);
                }
            }
            else
                await File.WriteAllTextAsync(path_to_save_log + PathSeparator + log_file_name + FileFormat, message);
        }

        /// <summary>
        /// Converts message type from enum to string.
        /// </summary>
        /// <param name="logType"></param>
        /// <returns></returns>
        public string MessageType(LogType logType)
        {
            switch (logType)
            {
                case LogType.Trace:
                    return LogType_Trace;
                case LogType.Debug:
                    return LogType_Debug;
                case LogType.Info:
                    return LogType_Info;
                case LogType.Notice:
                    return LogType_Notice;
                case LogType.Warn:
                    return LogType_Warn;
                case LogType.Error:
                    return LogType_Error;
                case LogType.Fatal:
                    return LogType_Fatal;
                default:
                    return LogType_NoType;
            }
        }

        /// <summary>
        /// Distributes what type of save is it configured. File, Email of Database.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="saveType"></param>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <returns></returns>
        public async Task Save(
            string message, 
            LogType logType, 
            FileSaveType? saveType      = FileSaveType.Standart, 
            string? path_to_save_log    = null,
            string? log_file_name       = null)
        {
            try
            {
                //  Save into a file
                if (ShouldSaveInFile(saveType, logType))
                    await SaveIntoFile(path_to_save_log, log_file_name, saveType, message, logType);
            }
            catch(Exception ex)
            {
                await SaveSimpLogError(ex.Message);
                //Dispose();
            }
        }

        /// <summary>
        /// SimpLog error if the library is not working
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SaveSimpLogError(string message)
        {
            //  Check if it has been disabled from appsettings.json file.
            if(configuration.Main_Configuration.Disable_Log is not null && 
                configuration.Main_Configuration.Disable_Log is true)
                return;

            string path     = string.Empty;
            string fileName = string.Empty;

            //  Checks the path in case of an error
            if (configuration.Main_Configuration.WhyLogIsNotWorkingPath is not null && Directory.Exists(configuration.Main_Configuration.WhyLogIsNotWorkingPath))
                path = configuration.Main_Configuration.WhyLogIsNotWorkingPath.ToString();
            else
                path = Path.GetTempPath();

            //  Create directory if not exists
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //  Checks file name
            if (configuration.Main_Configuration.WhyLogIsNotWorkingFileName is not null)
                fileName = configuration.Main_Configuration.WhyLogIsNotWorkingFileName.ToString();
            else
                fileName = "SimpLogError";

            await ImediateSaveMessageIntoLogFile(message, LogType.Error, path, fileName);
        }

        /// <summary>
        /// Checks all configurations should be saved log into a file at all
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        public bool ShouldSaveInFile(FileSaveType? saveType, LogType logType)
        {
            //  disabled from custom controller that should not be saved into a log file
            if(saveType.Equals(FileSaveType.DontSave) || (_Enable_File_Log is not null && _Enable_File_Log is false)) 
                return false;

            switch(logType)
            {
                case LogType.Trace:
                    {
                        if (_Trace_File is not null && _Trace_File is false)
                            return false;
                        break;
                    }
                case LogType.Debug:
                    {
                        if (_Debug_File is not null && _Debug_File is false)
                            return false;
                        break;
                    }
                case LogType.Info:
                    {
                        if (_Info_File is not null && _Info_File is false)
                            return false;
                        break;
                    }
                case LogType.Notice:
                    {
                        if (_Notice_File is not null && _Notice_File is false)
                            return false;
                        break;
                    }
                case LogType.Warn:
                    {
                        if (_Warn_File is not null && _Warn_File is false)
                            return false;
                        break;
                    }
                case LogType.Error:
                    {
                        if (_Error_File is not null && _Error_File is false)
                            return false;
                        break;
                    }
                case LogType.Fatal:
                    {
                        if (_Fatal_File is not null && _Fatal_File is false)
                            return false;
                        break;
                    }
            }

            return true;
        }

        /// <summary>
        /// Main logic for saving into a Log File
        /// </summary>
        /// <param name="path_to_save_log"></param>
        /// <param name="log_file_name"></param>
        /// <param name="saveType"></param>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <returns></returns>
        public async Task SaveIntoFile(string? path_to_save_log, string? log_file_name, FileSaveType? saveType, string message, LogType logType)
        {
            //  Checks if there should be saved a log into a file.
            if (saveType == FileSaveType.DontSave)
                return;

            //  Gets the right path from three different options. First is custom path in the function. If there is no, it is
            //  searching into the appsettings.json file for a path. If again there is no, then it is using by default in 
            //  local user Temp directory.
            if(string.IsNullOrEmpty(path_to_save_log) && !string.IsNullOrEmpty(configuration.File_Configuration.PathToSaveLogs))
                path_to_save_log = configuration.File_Configuration.PathToSaveLogs;
            else if(string.IsNullOrEmpty(path_to_save_log) && string.IsNullOrEmpty(configuration.File_Configuration.PathToSaveLogs))
                path_to_save_log = Path.GetTempPath();

            //  Checks the path of the folder. If the path is false but it is given somewhere then use default one
            if(!ConfigurationServices.ConfigService.PathCheck(path_to_save_log))
                path_to_save_log = Path.GetTempPath();

            //  Gets file name. If there is setup one in the function name it is with the advantage. If not then takes
            //  from appsettings.json file. If the both are empty it creates default with format DayOfYear_SimpLog of the current date name.
            if (string.IsNullOrEmpty(log_file_name) && !string.IsNullOrEmpty(configuration.File_Configuration.LogFileName))
                log_file_name = configuration.File_Configuration.LogFileName;
            else if(string.IsNullOrEmpty(log_file_name) && string.IsNullOrEmpty(configuration.File_Configuration.LogFileName))
                log_file_name = "SimpLog_" + DateTime.Now.DayOfYear.ToString();
            //  else log_file_name is the value

            //  Checks the type of saving into a file
            if (saveType.Equals(FileSaveType.BufferMemory))
                await BufferMessage(message, logType, path_to_save_log, log_file_name);
            else
                await ImediateSaveMessageIntoLogFile(message, logType, path_to_save_log, log_file_name);
        }
    }
}
