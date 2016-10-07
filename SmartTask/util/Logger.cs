using System;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace SmartTask
{
    /// <summary>
    /// 日志类
    /// http://blog.csdn.net/linjf520/article/details/17261685
    /// </summary>
    public class Logger
    {
        private const string PATH = "Log";
        private const string FILE_NAME = "Logs.txt";
        private const string FULL_NAME = PATH + "/" + FILE_NAME;
        public static readonly object Locker = new object();
        private static StreamWriter WRITER;
        private static string GUID;

        private static string ContinueWriteCaches;
        private static readonly Stopwatch Continue_WriteSw;
        private static int ContinueTime = 300;      // 300毫秒以后，连续写操作，都统一到一块操作  
        private static int ContinueCountMax = 100; // 当连续写操作次数上限到指定的数值后，都写一次操作，之后的重新再计算  
        private static int ContinueCount = 0;
        public static int AllWriteCount = 0;

        /// <summary>
        /// Log Out Window
        /// </summary>
        public static LogWindow LogWin { get; set; }

        /// <summary>
        /// Show Log Output Window
        /// </summary>
        public static void ShowLogWindow()
        {
            LogWindow win = new LogWindow();            
            Thread logThread = new Thread(new ThreadStart(delegate(){
                win.TopMost = true;
                LogWin = win;
                win.ShowDialog();
                }));
            logThread.Name = "OpenLogWindowThread";
            logThread.IsBackground = true;
            logThread.Start();
        }

        static Logger()
        {
            Continue_WriteSw = new Stopwatch();
        }

        private static string ProjectFullName
        {
            get
            {
                if (string.IsNullOrEmpty(GUID))
                {
                    GUID = Guid.NewGuid().ToString();
                }

                return PATH + "/" + "TEMPLATE_" + GUID + "_" + FILE_NAME;
            }
        }

        private static void Write(string msg)
        {
            if (string.IsNullOrEmpty(msg)) return;

            System.Diagnostics.Debug.WriteLine(msg);

            if (LogWin != null)
            {
                LogWin.Output(msg);
            }

            lock (Locker)
            {
                if (Continue_WriteSw.IsRunning && Continue_WriteSw.ElapsedMilliseconds < ContinueTime)
                {
                    if (ContinueWriteCaches == null)
                    {
                        ContinueWriteCaches = msg;
                    }
                    else
                    {
                        ContinueWriteCaches += msg + "\r\n";
                    }

                    ContinueCount++;

                    if (ContinueCount > ContinueCountMax)
                    {
                        _Write();
                    }

                    return;
                }

                if (!Continue_WriteSw.IsRunning) Continue_WriteSw.Start();
                ContinueWriteCaches = msg;

                new Task(() =>
                {
                    Thread.Sleep(ContinueTime);
                    //_Write();  
                }).Start();
            }
        }

        private static void _Write()
        {
            if (ContinueWriteCaches != null)
            {
                if (!File.Exists(ProjectFullName))
                {
                    if (!Directory.Exists(PATH))
                        Directory.CreateDirectory(PATH);
                    //File.Create(ProjectFullName);  
                }

                WRITER = new StreamWriter(ProjectFullName, true, Encoding.UTF8);
                WRITER.WriteLine(ContinueWriteCaches);
                WRITER.Flush();
                WRITER.Close();
            }
            Continue_WriteSw.Stop();
            Continue_WriteSw.Reset();
            ContinueWriteCaches = null;
            ContinueCount = 0;

            Interlocked.Increment(ref AllWriteCount);
        }

        public static void Debug(string msg)
        {
            msg = string.Format("[{0} {1}] : {2}", "Debug", DateTime.Now.ToString(), msg);
            Write(msg);
        }

        public static void Debug(string format, params object[] args)
        {
            string msg = String.Format(format, args);
            Debug(msg);
        }

        public static void Info(string msg)
        {
            msg = string.Format("[{0} {1}] : {2}", "Info", DateTime.Now.ToString(), msg);
            Write(msg);
        }

        public static void Info(string format, params object[] args)
        {
            string msg = String.Format(format, args); ;
            Info(msg);
        }

        public static void Warn(string msg)
        {
            msg = string.Format("[{0} {1}] : {2}", "Fatal", DateTime.Now.ToString(), msg);
            Write(msg);
        }

        public static void Warn(string format, params object[] args)
        {
            string msg = String.Format(format, args);
            Warn(msg);
        }

        public static void Error(string msg)
        {
            msg = string.Format("[{0} {1}] : {2}", "Error", DateTime.Now.ToString(), msg);
            Write(msg);
        }

        public static void Error(string format, params object[] args)
        {
            string msg = String.Format(format, args);
            Error(msg);
        }

        public static void Fatal(string msg)
        {
            msg = string.Format("[{0} {1}] : {2}", "Fatal", DateTime.Now.ToString(), msg);
            Write(msg);
        }

        public static void Fatal(string format, params object[] args)
        {
            string msg = String.Format(format, args);
            Fatal(msg);
        }
    }
}

