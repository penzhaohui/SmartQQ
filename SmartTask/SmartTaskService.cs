//#undef DEBUG
#define DEBUG

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmartTask
{
    public partial class SmartTaskService : ServiceBase
    {
        TaskManagementService _taskManager = null;
        private readonly string _displayName = "";

        public SmartTaskService()
        {        

            InitializeComponent();

            _displayName = ConfigurationManager.AppSettings["ServiceDisplayName"].Trim(); 
            ServiceName = ConfigurationManager.AppSettings["ServiceName"].Trim();
            CanPauseAndContinue = true;
            
            Logger.Debug("服务类初始化完成");
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            try
            {
                if (_taskManager == null)
                    _taskManager = new TaskManagementService();
                Logger.InfoFormat("try run the {0}.", _displayName);
                _taskManager.Start();
                Logger.InfoFormat("{0} is runing.", _displayName);
            }
            catch (Exception ex)
            {
                Logger.FatalFormat("启动服务时发生异常", ex);
                throw;
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        protected override void OnStop()
        {
            Logger.InfoFormat("try stop the {0}.", _displayName);
            _taskManager.Stop();
            Logger.InfoFormat("{0} is stoped.", _displayName);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        protected override void OnPause()
        {
            Logger.InfoFormat("try pause the {0}", _displayName);
            _taskManager.Pause();
            Logger.InfoFormat("{0} is pauseed\n", _displayName);
        }

        /// <summary>
        /// 继续执行
        /// </summary>
        protected override void OnContinue()
        {
            Logger.InfoFormat("try continue the {0}", _displayName);
            _taskManager.Resume();
            Logger.InfoFormat("{0} is continued\n", _displayName);
        }

        /// <summary>
        /// 应用程序入口
        /// </summary>
        public static void Main(string[] args)
        {
            Logger.DebugFormat("args:[{0}]", string.Join(" ", args));

            try
            {
                var newMutexCreated = false;
                var mutexName = "SmartTask";

                try
                {
                    var obj = new Mutex(false, mutexName, out newMutexCreated);
                }
                catch (Exception ex)
                {
                    Logger.ErrorFormat("创建互斥体[mutexName = {0}]异常，程序退出", mutexName);
                    Environment.Exit(1);
                }

                if (newMutexCreated)
                {
                    Logger.DebugFormat("创建互斥体[mutexName = {0}]成功，开始创建服务", mutexName);

                    //无参数时直接运行服务
                    if ((!Environment.UserInteractive))
                    {
                        Logger.Debug("RunAsService");
                        RunAsService();
                        return;
                    }

                    if (args != null && args.Length > 0)
                    {
                        if (args[0].Equals("-i", StringComparison.OrdinalIgnoreCase))
                        {
                            Logger.Info("Install the service...");
                            SelfInstaller.InstallMe();
                            return;
                        }
                        if (args[0].Equals("-u", StringComparison.OrdinalIgnoreCase))
                        {
                            Logger.Info("Uninstall the service...");
                            SelfInstaller.UninstallMe();
                            return;
                        }
                        if (args[0].Equals("-t", StringComparison.OrdinalIgnoreCase) ||
                            args[0].Equals("-c", StringComparison.OrdinalIgnoreCase))
                        {
                            Logger.InfoFormat("Run as Console.[{0}]", Assembly.GetExecutingAssembly().Location);
                            RunAsConsole(args);
                            return;
                        }
                        const string tip =
                            "Invalid argument! note:\r\n -i is install the service.;\r\n -u is uninstall the service.;\r\n -t or -c is run the service on console.";
                        Logger.Info(tip);
                        Console.WriteLine(tip);
                        Console.ReadLine();
                    }
                    else
                    {
#if DEBUG
                        Logger.InfoFormat("Run as Console.[{0}]", Assembly.GetExecutingAssembly().Location);
                        RunAsConsole(args);
#endif
                    }
                }
                else
                {
                    Logger.Error("有一个实例正在运行，如要调试，请先停止其它正在运行的实例如WindowsService，程序退出。");
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("启动服务异常", ex);
            }
            finally
            {
            }
        }

        private static void RunAsConsole(string[] args)
        {
            var service = new SmartTaskService();
            service.OnStart(null);
            Console.ReadLine();
        }

        private static void RunAsService()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new SmartTaskService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
