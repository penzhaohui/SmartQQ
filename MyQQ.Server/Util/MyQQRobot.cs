using SmartQQ.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyQQ.Util
{
    public class MyQQRobot
    {
        public static SmartQQ.MessageService messageService = null;

        public static void LaunchMyQQRobot(SmartQQWrapper smartQQ)
        {
            if (messageService == null)
            {
                messageService = new SmartQQ.MessageService(smartQQ);
                messageService.ReceiveMessage(ProcessMessageHandler);
            }
            else
            {
                messageService.SwitchSmartQQ(smartQQ);
            }
        }

        public static void ProcessMessageHandler(QQMessage message)
        { 
        }
    }
}