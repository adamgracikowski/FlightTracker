using System.Text;

namespace ProjOb_24L_01180781.DataSource.Ftre
{
    public class LogManager : IDisposable
    {
        public static LogManager GetInstance()
        {
            lock (_instanceLock)
            {
                return _instance ??= new();
            }
        }
        public void Write(string logEntry)
        {
            var logTask = Task.Run(() =>
            {
                lock (_logStream)
                {
                    var logTime = DateTime.Now;
                    _logStream.WriteLine($"{logTime:HH:mm:ss} | {logEntry}");
                    _logStream.Flush();
                }
            });

            lock (_logTasksLock)
                _logTasks.Add(logTask);
        }
        public void Dispose()
        {
            try
            {
                lock (_logTasksLock)
                    Task.WaitAll([.. _logTasks]);
            }
            catch (AggregateException)
            {
                throw;
            }
            finally
            {
                _logStream.Dispose();
            }
        }

        private LogManager()
        {
            _dateTime = DateTime.Now;

            var logsDirectory = TryCreateLogsDirectory();
            _logFilename = GetLogFilename(logsDirectory);
            _logStream = new(_logFilename, true, Encoding.UTF8);
        }
        private static string TryCreateLogsDirectory()
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var logsDirectory = Path.Combine(baseDirectory, "logs");
            if (!Directory.Exists(logsDirectory))
            {
                Directory.CreateDirectory(logsDirectory);
                Console.WriteLine($"{Path.DirectorySeparatorChar}logs created.");
            }
            return logsDirectory;
        }
        private string GetLogFilename(string logsDirectory)
        {
            long counter = 0;
            var filename = $"{_dateTime.ToString(_dateFormat)}-{counter}.txt";
            while (File.Exists(Path.Combine(logsDirectory, filename)))
                filename = $"{_dateTime.ToString(_dateFormat)}-{++counter}.txt";
            return Path.Combine(logsDirectory, filename);
        }

        private static LogManager? _instance;
        private string _logFilename { get; set; }
        private DateTime _dateTime { get; set; }
        private StreamWriter _logStream { get; set; }
        private static List<Task> _logTasks { get; set; } = [];
        private static readonly object _instanceLock = new();
        private static readonly object _logTasksLock = new();
        private static readonly string _dateFormat = "yyyy-MM-dd";
    }
}
