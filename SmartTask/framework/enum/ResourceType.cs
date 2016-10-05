using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTask
{
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// 未知的
        /// </summary>
        Unknow = 0,

        /// <summary>
        /// 本地资源
        /// </summary>
        Local,

        /// <summary>
        /// Http型源型
        /// </summary>
        Http,

        /// <summary>
        /// Ftp协议资源
        /// </summary>
        Ftp,

        /// <summary>
        /// MsSqlServer数据库
        /// </summary>
        SqlServer,

        /// <summary>
        /// Web服务
        /// </summary>
        WebService,

        /// <summary>
        /// Email服务器
        /// </summary>
        EmailService,
    }
}
