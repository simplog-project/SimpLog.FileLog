using SimpLog.FileLog.Services.FileServices;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SimpLog.FileLog
{
    public static class ServicesExtensions
    {
        private static CancellationTokenSource _cts;
        private static Task _backgroundTask;
        private static readonly object _lock = new();

        /// <summary>
        /// Starts SimpLog background file logging.
        /// Safe to call once during app startup.
        /// </summary>
        public static void SimpLog()
        {
            lock (_lock)
            {
                if (_backgroundTask != null)
                    return; // already running

                _cts = new CancellationTokenSource();
                _backgroundTask = Task.Run(() => BufferSave(_cts.Token));
            }
        }

        /// <summary>
        /// Optional: stop logging gracefully.
        /// </summary>
        public static async Task StopAsync()
        {
            lock (_lock)
            {
                if (_cts == null)
                    return;

                _cts.Cancel();
            }

            try
            {
                await _backgroundTask;
            }
            catch (OperationCanceledException) { }
        }


        private static async Task BufferSave(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                //  If the configuration file was not set
                if (!File.Exists(Environment.CurrentDirectory + "\\simplog.json"))
                    await Task.Delay(TimeSpan.FromSeconds(10));

                await new FileService().SaveMessageIntoLogFile();

                await Task.Delay(TimeSpan.FromSeconds(5));
            }
        }

    }
}
