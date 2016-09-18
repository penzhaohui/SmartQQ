using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.model
{
    public class QRStatus
    {
        /// <summary>
        /// Status Code
        /// 
        /// 65 - 二维码失效
        /// 66 - 二维码失效
        /// 67 - 等待确认
        /// 0  - 已经确认
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// QR Code Statys
        /// </summary>
        public string StatusText { get; set; }

        /// <summary>
        /// Redirect Url for login
        /// </summary>
        public string RedirectUrl { get; set; }
    }
}
