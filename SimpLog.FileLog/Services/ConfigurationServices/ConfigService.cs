using SimpLog.FileLog.Models.AppSettings;
using System;
using System.IO;
using System.Text.Json;

namespace SimpLog.FileLog.Services.ConfigurationServices
{
    internal static class ConfigService
    {
        #region Main Configuration Variable

        public static readonly string? _WhyLogIsNotWorkingPath;
        
        #endregion Main Configuration Variable

        #region Log File Configuration Variable

        static readonly string? _PathToSaveLogs = null;
        static readonly string? _LogFileName = null;
        static readonly bool? _Enable_File_Log = null;

        #endregion Log File Configuration Variable

        #region Log Type Configuration Variable
        
        static readonly bool? _TraceLog;
        static readonly bool? _TraceSendEmail        = true;
        static readonly bool? _TraceSaveInDatabase   = false;

        static readonly bool? _DebugLog              = false;
        static readonly bool? _DebugSendEmail        = false;
        static readonly bool? _DebugSaveInDatabase   = false;
        
        static readonly bool? _InfoLog               = false;
        static readonly bool? _InfoSendEmail         = false;
        static readonly bool? _InfoSaveInDatabase    = false;
        
        static readonly bool? _NoticeLog             = false;
        static readonly bool? _NoticeSendEmail       = false;
        static readonly bool? _NoticeSaveInDatabase  = false;
        
        static readonly bool? _WarnLog               = false;
        static readonly bool? _WarnSendEmail         = false;
        static readonly bool? _WarnSaveInDatabase    = false;
        
        static readonly bool? _ErrorLog              = false;
        static readonly bool? _ErrorSendEmail        = false;
        static readonly bool? _ErrorSaveInDatabase   = false;
        
        static readonly bool? _FatalLog              = false;
        static readonly bool? _FatalSendEmail        = false;
        static readonly bool? _FatalSaveInDatabase   = false;

        #endregion Log Type Configuration Variable

        static ConfigService()
        {
            Configuration? simpLogConfig;

            //  If there is not found a configuration file
            if (!File.Exists(Environment.CurrentDirectory + "\\simplog.json"))
            {
                simpLogConfig = new Configuration()
                {
                    Main_Configuration = new MainConfiguration()
                    {
                        WhyLogIsNotWorkingFileName = null,
                        WhyLogIsNotWorkingPath = null,
                        Disable_Log = null
                    },
                    File_Configuration = new FileConfiguration()
                    {
                        Enable_File_Log = null,
                        LogFileName = null,
                        PathToSaveLogs = null,
                    },
                    LogType = new Log()
                    {
                        Debug = new LogTypeObject()
                        {
                            Log = null
                        },
                        Error = new LogTypeObject()
                        {
                            Log = null
                        },
                        Fatal = new LogTypeObject()
                        {
                            Log = null
                        },
                        Info = new LogTypeObject()
                        {
                            Log = null
                        },
                        Notice = new LogTypeObject()
                        {
                            Log = null
                        },
                        Trace = new LogTypeObject()
                        {
                            Log = null
                        },
                        Warn = new LogTypeObject()
                        {
                            Log = null
                        }
                    }
                };
            }
            else
                simpLogConfig = JsonSerializer.Deserialize<Configuration>(File.ReadAllText(Environment.CurrentDirectory + "\\simplog.json"));

            #region Main Configuration Get From Json

            _WhyLogIsNotWorkingPath = simpLogConfig.Main_Configuration.WhyLogIsNotWorkingPath;

            //            _WhyLogIsNotWorkingPath = (simpLogConfig.Main_Configuration.WhyLogIsNotWorkingPath == null) ? string.Empty : simpLogConfig.Main_Configuration.WhyLogIsNotWorkingPath.ToString();

            #endregion Main Configuration Get From Json

            #region Log File Configuration Get From Json

            _PathToSaveLogs = simpLogConfig.File_Configuration.PathToSaveLogs;
            _LogFileName = simpLogConfig.File_Configuration.LogFileName;
            _Enable_File_Log = simpLogConfig.File_Configuration.Enable_File_Log;

            #endregion Log File Configuration Get From Json

            #region Log Type Configuration Get From Json

            //  Checks if the configuration exists at all
            _TraceLog = simpLogConfig.LogType.Trace.Log;
            _TraceSendEmail = simpLogConfig.LogType.Trace.SendEmail;
            _TraceSaveInDatabase = simpLogConfig.LogType.Trace.SaveInDatabase;

            _DebugLog = simpLogConfig.LogType.Debug.Log;
            _DebugSendEmail = simpLogConfig.LogType.Debug.SendEmail;
            _DebugSaveInDatabase = simpLogConfig.LogType.Debug.SaveInDatabase;

            _InfoLog = simpLogConfig.LogType.Info.Log;
            _InfoSendEmail = simpLogConfig.LogType.Info.SendEmail;
            _InfoSaveInDatabase = simpLogConfig.LogType.Info.SaveInDatabase;

            _NoticeLog = simpLogConfig.LogType.Notice.Log;
            _NoticeSendEmail = simpLogConfig.LogType.Notice.SendEmail;
            _NoticeSaveInDatabase = simpLogConfig.LogType.Notice.SaveInDatabase;

            _WarnLog = simpLogConfig.LogType.Warn.Log;
            _WarnSendEmail = simpLogConfig.LogType.Warn.SendEmail;
            _WarnSaveInDatabase = simpLogConfig.LogType.Warn.SaveInDatabase;

            _ErrorLog = simpLogConfig.LogType.Error.Log;
            _ErrorSendEmail = simpLogConfig.LogType.Error.SendEmail;
            _ErrorSaveInDatabase = simpLogConfig.LogType.Error.SaveInDatabase;

            _FatalLog = simpLogConfig.LogType.Fatal.Log;
            _FatalSendEmail = simpLogConfig.LogType.Fatal.SendEmail;
            _FatalSaveInDatabase = simpLogConfig.LogType.Fatal.SaveInDatabase;

            #endregion Log Type Configuration Get From Json
        }

        /// <summary>
        /// Check if the path exists
        /// </summary>
        /// <param name="path_to_save_log"></param>
        /// <returns></returns>
        public static bool PathCheck(string? path_to_save_log)
        {
            if (!string.IsNullOrEmpty(path_to_save_log) && Directory.Exists(path_to_save_log))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Put into an object the configuration from appsettings.json file
        /// </summary>
        /// <returns></returns>
        public static Configuration BindConfigObject()
        {
            return new Configuration()
            {
                Main_Configuration      = new MainConfiguration() 
                {
                    WhyLogIsNotWorkingPath = _WhyLogIsNotWorkingPath
                },
                File_Configuration       = new FileConfiguration()
                {
                    PathToSaveLogs  = _PathToSaveLogs,
                    LogFileName     = _LogFileName,
                    Enable_File_Log = _Enable_File_Log
                },
                LogType                 = new Log()
                {
                    Trace   = new LogTypeObject()
                    {
                        Log             = _TraceLog
                    },
                    Debug   = new LogTypeObject()
                    {
                        Log             = _DebugLog
                    },
                    Info    = new LogTypeObject()
                    {
                        Log             = _InfoLog
                    },
                    Notice  = new LogTypeObject()
                    {
                        Log             = _NoticeLog
                    },
                    Warn    = new LogTypeObject()
                    {
                        Log             = _WarnLog
                    },
                    Error   = new LogTypeObject()
                    {
                        Log             = _ErrorLog
                    },
                    Fatal   = new LogTypeObject()
                    {
                        Log             = _FatalLog
                    },
                }
            };
        }
    }
}
