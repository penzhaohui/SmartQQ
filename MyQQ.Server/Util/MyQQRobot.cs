using SmartQQ.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TulingRobot;

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
            /*
            string answer = TulingRobot.Answer(message.MessageContent);
            if (message.AccountType == AccountType.Private)
            {
                messageService.SendMessage(0, message.FriendID, answer);
            }
            else if (message.AccountType == AccountType.Group)
            {
                messageService.SendMessage(1, message.GroupID, answer);
            }
            else if (message.AccountType == AccountType.Discussion)
            {
                messageService.SendMessage(2, message.DiscussionID, answer);
            }
            */

            RobotPool robotPool = RobotPool.NewInstance();
            IRobot robot = null;
            string answer = "";            
            if (message.AccountType == AccountType.Private)
            {
                robot = robotPool.ApplyOneAvailableRobot(RobotType.Private, message.FriendID);
                answer = robot.Consult(message.MessageContent);
                messageService.SendMessage(0, message.FriendID, answer);
            }
            else if (message.AccountType == AccountType.Group
                || message.AccountType == AccountType.Discussion)
            {
                robot = robotPool.ApplyOneAvailableRobot(RobotType.Public, message.QQAccount);
                answer = robot.Consult(message.MessageContent);

                if (message.AccountType == AccountType.Group)
                {
                    messageService.SendMessage(1, message.GroupID, answer);
                }
                else
                {
                    messageService.SendMessage(2, message.DiscussionID, answer);
                }
            }

            System.Console.WriteLine("Receive one message");
            //Task.Run(() => MyQQDAL.AddOneMessage(message.QQAccount, message));
            MyQQDAL.AddOneMessage(message.QQAccount, message);
        }

        public static void ProcessHeartbeatHandler(string qqAccount, bool running)
        {
            MyQQDAL.UpdateOnlineStatus(qqAccount, running);
        }
    }
}