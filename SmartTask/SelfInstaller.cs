using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartTask
{
    /// <summary>
    /// 服务自安装
    /// </summary>
    public class SelfInstaller
    {
        private static readonly string exePath = Assembly.GetExecutingAssembly().Location;

        /// <summary>
        /// Install service
        /// </summary>
        /// <returns></returns>
        public static bool InstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { exePath });
            }
            catch(Exception ex)
            {
                Logger.Error("Failed to install the windows service, ", ex.Message);
                if (ex.InnerException != null)
                {
                    Logger.Error("Inner exception message {0}", ex.InnerException.Message);
                }

                return false;
            }
            return true;
        }

        /// <summary>
        /// Uninstall service
        /// </summary>
        /// <returns></returns>
        public static bool UninstallMe()
        {
            try
            {
                ManagedInstallerClass.InstallHelper(new[] { "/u", exePath });
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
