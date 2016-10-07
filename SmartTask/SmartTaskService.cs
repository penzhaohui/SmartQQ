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
                Logger.Info("try run the {0}.", _displayName);
                _taskManager.Start();
                Logger.Info("{0} is runing.", _displayName);
            }
            catch (Exception ex)
            {
                Logger.Fatal("启动服务时发生异常", ex);
                throw;
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        protected override void OnStop()
        {
            Logger.Info("try stop the {0}.", _displayName);
            _taskManager.Stop();
            Logger.Info("{0} is stoped.", _displayName);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        protected override void OnPause()
        {
            Logger.Info("try pause the {0}", _displayName);
            _taskManager.Pause();
            Logger.Info("{0} is pauseed\n", _displayName);
        }

        /// <summary>
        /// 继续执行
        /// </summary>
        protected override void OnContinue()
        {
            Logger.Info("try continue the {0}", _displayName);
            _taskManager.Resume();
            Logger.Info("{0} is continued\n", _displayName);
        }       

        /// <summary>
        /// 应用程序入口
        /// </summary>
        public static void Main(string[] args)
        {
            Logger.Debug("args:[{0}]", string.Join(" ", args));

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
                    Logger.Error("创建互斥体[mutexName = {0}]异常，程序退出", mutexName);                   
                    Environment.Exit(1);
                }

                if (newMutexCreated)
                {
                    Logger.Debug("创建互斥体[mutexName = {0}]成功，开始创建服务", mutexName);

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
                            Logger.Info("Run as Console.[{0}]", Assembly.GetExecutingAssembly().Location);
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
                        Logger.Info("Run as Console.[{0}]", Assembly.GetExecutingAssembly().Location);
                        RunAsConsole(args);
#endif
                    }
                }
                else
                {
                    Logger.Error("有一个实例正在运行，如要调试，请先停止其它正在运行的实例如WindowsService，程序退出。");

                    System.Console.Write("Smart Task Console is running in the background. Do you want to show it?(Y/N)");
                    char answer = Convert.ToChar(System.Console.Read());
                    if ('Y' == answer || 'y' == answer)
                    {
                        ConsoleHelper.showConsole("Welcome to Smart Task Console@V1.0");
                    }
                    else
                    {
                        Environment.Exit(1);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("启动服务异常", ex);
            }
            finally
            {
            }
        }

        private static void RunAsConsole(string[] args)
        {
            string ConsileTile = ConfigurationManager.AppSettings["ConsoleTitle"];
            Console.Title = ConsileTile;

            string strProcessName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if (System.Diagnostics.Process.GetProcessesByName(strProcessName).Length > 1)
            {
                ConsoleHelper.showConsole(ConsileTile);
                return;
            }

            var service = new SmartTaskService();
            service.OnStart(null);

            ShowCommandHelpText();

            while (true)
            {
                System.Console.WriteLine("Please select one command:");
                string command = Console.ReadLine();
                string[] commandArgs = command.ToLower().Split(' ');
                // Command line parsing
                Arguments CommandLine = new Arguments(commandArgs);
                if (CommandLine["start"] != null)
                {
                    service.OnStart(null);
                }
                else if (CommandLine["stop"] != null)
                {
                    service.OnStop();
                }
                else if (CommandLine["pause"] != null)
                {
                    service.OnPause();
                }
                else if (CommandLine["resume"] != null)
                {
                    service.OnContinue();
                }
                else if (CommandLine["clear"] != null)
                {
                    Console.Clear();
                    ShowCommandHelpText();
                } 
                else if (CommandLine["hide"] != null)
                {
                    ConsoleHelper.hideConsole(ConsileTile);
                }
                else if (CommandLine["log"] != null)
                {
                    Logger.ShowLogWindow();
                }                
                else if (CommandLine["exit"] != null)
                {
                    System.Environment.Exit(-1); 
                }                
                else if (CommandLine["help"] != null)
                {
                    ShowCommandHelpText();
                }
                else
                {
                    System.Console.WriteLine("Invalid comment, please enter -help for some help");
                }
            }
        }

        /// <summary>
        /// 显示控制台命令行帮助信息
        /// </summary>
        private static void ShowCommandHelpText()
        {
            System.Console.WriteLine("*********************************************");
            System.Console.WriteLine("*  Welcome to Smart Task Console Window     *");
            System.Console.WriteLine("*  -help   Show help text                   *");
            System.Console.WriteLine("*  -start  Start service                    *");
            System.Console.WriteLine("*  -stop   Stop service                     *");
            System.Console.WriteLine("*  -pause  Start service                    *");
            System.Console.WriteLine("*  -resume Resume service                   *");
            System.Console.WriteLine("*  -clear  Clean the console window         *");
            System.Console.WriteLine("*  -hide   Hile the console window          *");
            System.Console.WriteLine("*  -exit   Exit this application            *");
            System.Console.WriteLine("*  -log    Show the log window              *");
            System.Console.WriteLine("*  ------------------------------------     *");
            System.Console.WriteLine("*  For example:                             *");
            System.Console.WriteLine("*  Please select one command: -stop         *");
            System.Console.WriteLine("*********************************************");
            System.Console.WriteLine("\n\n");
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
