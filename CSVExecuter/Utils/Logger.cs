using System.Collections.Concurrent;
using System.Text;

namespace Utils.Utils
{
    /// <summary>
    /// ロガークラス
    /// </summary>
    public class Logger
    {
        readonly static int UpdateCycleMilliseconds = 100;    //ログ書き込みサイクル時間(ms)
        readonly static ConcurrentDictionary<string, Logger> Manager = new ConcurrentDictionary<string, Logger>();
        readonly static object MyLock = new object();
        readonly ConcurrentQueue<string> Queues = new ConcurrentQueue<string>();
        readonly string Filename;
        readonly static Task LoggerTask = Task.Run(async () =>
        {
            while (true)
            {
                foreach (var logger in Manager.Values)
                    await logger.FlushAsync();
                await Task.Delay(UpdateCycleMilliseconds);
            }
        });

        /// <summary>
        /// ログ書き込み
        /// </summary>
        /// <param name="task"></param>
        /// <param name="message"></param>
        public static void WriteLine(string task, string message)
        {
            lock (MyLock)
            {
                if (!Manager.TryGetValue(task, out Logger? logger))
                    logger = Manager[task] = new Logger(task);
                logger.WriteLine(message);
            }
        }

        /// <summary>
        /// ログ書き込み
        /// </summary>
        /// <param name="task"></param>
        /// <param name="ex"></param>
        public static void WriteLine(string task, Exception ex)
        {
            lock (MyLock)
            {
                if (!Manager.TryGetValue(task, out Logger? logger))
                    logger = Manager[task] = new Logger(task);
                logger.WriteLine(ex);
            }
        }

        /// <summary>
        /// 書き込み
        /// </summary>
        /// <returns></returns>
        async Task FlushAsync()
        {
            if (Queues.IsEmpty)
                return;

            //書き込み
            using (var writer = new StreamWriter(Filename, true, Encoding.UTF8) { AutoFlush = false })
            {
                while (Queues.TryDequeue(out string? message))
                    await writer.WriteLineAsync(message);
                await writer.FlushAsync();
            }
        }

        /// <summary>
        /// コンストラクタ処理
        /// </summary>
        /// <param name="task"></param>
        Logger(string task)
        {
            Filename = PathManager.GetBaseFullPath($"Log_{task}.txt");
        }

        /// <summary>
        /// ログ書き込み
        /// </summary>
        /// <param name="message"></param>
        void WriteLine(string? message)
        {
            if (!string.IsNullOrWhiteSpace(message))
                Queues.Enqueue($"{DateTime.Now:HH:mm:ss.fff},{message}");
        }

        /// <summary>
        /// 例外ログ書き込み
        /// </summary>
        /// <param name="_ex"></param>
        void DoWriteLine(Exception? _ex)
        {
            if (_ex == null) return;
            WriteLine($"{_ex.GetType()},{_ex.Message}");
            WriteLine(_ex.StackTrace);
            WriteLine("- - - - - - - - - -");
            var _ex1 = _ex.InnerException;
            if (_ex1 != null)
                DoWriteLine(_ex);
            if (_ex is AggregateException _ex2)
            {
                foreach (var _ex3 in _ex2.InnerExceptions)
                    DoWriteLine(_ex3);
            }
            WriteLine("--------------------");
        }

        /// <summary>
        /// 例外ログ書き込み
        /// </summary>
        /// <param name="ex"></param>
        void WriteLine(Exception ex)
        {
            WriteLine("----------ここから----------");
            DoWriteLine(ex);
            WriteLine("----------ここまで----------");
        }
    }

}
