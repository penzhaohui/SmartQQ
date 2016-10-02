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
        public static readonly MyQQDAL MyQQDAL = new MyQQDAL();

        public static void LaunchMyQQRobot(SmartQQWrapper smartQQ)
        {
            if (messageService == null)
            {
                messageService = new SmartQQ.MessageService(smartQQ, ProcessHeartbeatHandler);
                messageService.ReceiveMessage(ProcessMessageHandler);
            }
            else
            {
                messageService.SwitchSmartQQ(smartQQ);
            }
        }

        public static void ProcessMessageHandler(QQMessage message)
        {
            System.Console.WriteLine("Receive one message");
            MyQQDAL.AddOneMessage(message.QQAccount, message);
        }

        public static void ProcessHeartbeatHandler(string qqAccount, bool running)
        {
            MyQQDAL.UpdateOnlineStatus(qqAccount, running);
        }
    }
}