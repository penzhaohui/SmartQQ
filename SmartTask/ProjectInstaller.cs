using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace SmartTask
{
    /// <summary>
    /// 服务安装，安装后立即启动
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private static readonly string codeBase = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().CodeBase);
        protected string ConfigServiceName = codeBase;
        protected string ConfigDescription = null;
        protected string DisplayName = codeBase;

        /// <summary>
        /// 安装
        /// </summary>
        public ProjectInstaller()
        {
            InitializeComponent();
            this.Committed += ServiceInstallerCommitted;
      

            var serviceName = ConfigurationManager.AppSettings["ServiceName"].Trim();
            var displayName = ConfigurationManager.AppSettings["ServiceDisplayName"].Trim();
            var desc = ConfigurationManager.AppSettings["ServiceDescription"].Trim();
            if (!string.IsNullOrEmpty(serviceName)) ConfigServiceName = serviceName;
            if (!string.IsNullOrEmpty(displayName)) DisplayName = displayName;
            if (!string.IsNullOrEmpty(desc)) ConfigDescription = desc;

            this.serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            // 自动启动服务，手动的话，每次开机都要手动启动。
            this.serviceInstaller.StartType = ServiceStartMode.Automatic;
            this.serviceInstaller.DisplayName = DisplayName;
            this.serviceInstaller.DisplayName = DisplayName;
            this.serviceInstaller.ServiceName = ConfigServiceName;
            this.serviceInstaller.Description = ConfigDescription;
        }

        /// <summary>
        /// 服务安装完成后自动启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServiceInstallerCommitted(object sender, InstallEventArgs e)
        {
            var controller = new ServiceController(ConfigServiceName);
            controller.Start();
        }
    }
}
