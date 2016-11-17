using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ.model
{
    public enum MessageStatus
    {
        Initialized = 0,
        GetMessage = 1,
        SendMessage = 2,
        GetAndSendMessage = 3
    }
}
