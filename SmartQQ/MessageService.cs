using Newtonsoft.Json;
using SmartQQ.json;
using SmartQQ.model;
using SmartQQ.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SmartQQ
{
    public class MessageService
    {
        private static Random rand = new Random();

        public delegate void ProcessMessageHandler(QQMessage message);
        public delegate void SendMessageHandler(bool success);
        public delegate void HeartbeatMonitor(string qqAccount, bool running);
        
        private SmartQQWrapper smartQQ = null;
        private ProcessMessageHandler ProcessMessageAction;
        private SendMessageHandler SendMessageAction;
        private HeartbeatMonitor HeartbeatMonitorAction;

        public MessageService(SmartQQWrapper smartQQ, HeartbeatMonitor callbackFunc = null)
        {
            this.smartQQ = smartQQ;
            this.HeartbeatMonitorAction = callbackFunc;
        }

        private MessageStatus status = MessageStatus.Initialized;
        public MessageStatus Status
        {
            get {
                return status;
            }
        }

        public void SwitchSmartQQ(SmartQQWrapper smartQQ)
        {
            this.smartQQ = smartQQ;            
        }        

        public void ReceiveMessage(ProcessMessageHandler callbackFunc = null)
        {
            status += (int) MessageStatus.GetMessage;

            this.ProcessMessageAction = callbackFunc;
            Task.Run(() => PollMessage());
        }

        public void SendMessage(SendMessageHandler callbackFunc)
        {
            status += (int)MessageStatus.SendMessage;

            this.SendMessageAction = callbackFunc;
        }

        #region Poll QQ Message

        private void PollMessage()
        {
            try
            {
                string url = "http://d1.web2.qq.com/channel/poll2";
                string packData = "{\"ptwebqq\":\"#{ptwebqq}\",\"clientid\":53999199,\"psessionid\":\"#{psessionid}\",\"key\":\"\"}";
                packData = packData.Replace("#{ptwebqq}", smartQQ.PTWebQQ).Replace("#{psessionid}", smartQQ.PSessionId);
                packData = "r=" + HttpUtility.UrlEncode(packData);
                HTTP.Post_Async_Action action = ReceiveMessage;
                HTTP.Post_Async(url, packData, action);
            }
            catch (Exception)
            {
                PollMessage();
            }
        }

        private bool processMessageFlag = true;
        private bool ProcessMessageFlag 
        {
            get {
                return processMessageFlag;
            }
            set {
                processMessageFlag = value;

                if (value == true)
                {
                    if (status != MessageStatus.GetMessage)
                    {
                        status += (int)MessageStatus.GetMessage;
                    }
                }

                if (HeartbeatMonitorAction != null)
                {
                    HeartbeatMonitorAction(this.smartQQ.QQAccount, processMessageFlag);
                }
            }
        }

        private void ReceiveMessage(string data)
        {
            Task.Run(() => PollMessage());
            if (ProcessMessageFlag)
            {
                ProcessMessage(data);
            }
        }

        private void ProcessMessage(string data)
        {
            Message poll = (Message)JsonConvert.DeserializeObject(data, typeof(Message));
            if (poll.retcode != 0)
            {
                ProcessMessageError(poll);
            }
            else if (poll.result != null && poll.result.Count > 0)
            {
                QQMessage message = new QQMessage();                

                for (int i = 0; i < poll.result.Count; i++)
                {
                    switch (poll.result[i].poll_type)
                    {
                        case "kick_message":
                            ProcessMessageFlag = false;
                            //MessageBox.Show(poll.result[i].value.reason);
                            break;
                        case "message":
                            message = ProcessPrivateMessage(poll.result[i].value);
                            break;
                        case "group_message":
                            message = ProcessGroupMessage(poll.result[i].value);
                            break;
                        case "discu_message":
                            message = ProcessDisscussMessage(poll.result[i].value);
                            break;
                        default:                            
                            break;
                    }
                }

                if (ProcessMessageAction != null)
                {
                    message.QQAccount = this.smartQQ.QQAccount;
                    ProcessMessageAction(message);
                }
            }
        }

        /// <summary>
        /// 私聊消息处理
        /// </summary>
        /// <param name="value">poll包中的value</param>
        private QQMessage ProcessPrivateMessage(Message.Result.Value value)
        {
            QQMessage message = new QQMessage();

            message.AccountType = AccountType.Private;
            message.FriendID = value.from_uin;
            message.MessageType = MessageType.Receive;
            message.MessageContent = GetMessageText(value.content);

            this.SendMessage(0, message.FriendID, "收到！QQ机器人正在处理，请稍等...");

            return message;
        }

        /// <summary>
        /// 群聊消息处理
        /// </summary>
        /// <param name="value">poll包中的value</param>
        private QQMessage ProcessGroupMessage(Message.Result.Value value)
        {
            QQMessage message = new QQMessage();
            message.AccountType = AccountType.Group;
            message.GroupID = value.group_code;
            message.FriendID = value.send_uin;
            message.MessageType = MessageType.Receive;
            message.MessageContent = GetMessageText(value.content);

            this.SendMessage(1, message.GroupID, "收到！QQ机器人正在处理，请稍等...");

            return message;
        }

        /// <summary>
        /// 讨论组消息处理
        /// </summary>
        /// <param name="value">poll包中的value</param>
        private QQMessage ProcessDisscussMessage(Message.Result.Value value)
        {
            QQMessage message = new QQMessage();
            message.AccountType = AccountType.Discussion;
            message.DiscussionID = value.did;
            message.FriendID = value.send_uin;
            message.MessageType = MessageType.Receive;
            message.MessageContent = GetMessageText(value.content);

            this.SendMessage(2, message.DiscussionID, "收到！QQ机器人正在处理，请稍等...");

            return message;
        }

        /// <summary>
        /// 错误信息处理
        /// </summary>
        /// <param name="poll">poll包</param>
        private int Count103 = 0;
        private void ProcessMessageError(Message poll)
        {
            /*
               http://blog.csdn.net/mingzznet/article/details/9879977
               返回103、121，代表连接不成功，需要重新登录；
               返回102，代表连接正常，此时服务器暂无信息；
               返回0，代表服务器有信息传递过来：包括群信、群成员给你的发信，QQ好友给你的发信。
             */
            int TempCount103 = Count103;
            Count103 = 0;
            if (poll.retcode == 102)
            {
                return;
            }
            else if (poll.retcode == 103)
            {
                Count103 = TempCount103 + 1;
                if (Count103 > 20)
                {
                    ProcessMessageFlag = false;
                }

                return;
            }
            else if (poll.retcode == 116)
            {
                //ptwebqq = poll.p;
                return;
            }
            else if (poll.retcode == 108 || poll.retcode == 114)
            {
                ProcessMessageFlag = false;
                return;
            }
            else if (poll.retcode == 120 || poll.retcode == 121)
            {
                ProcessMessageFlag = false;
                return;
            }
            else if (poll.retcode == 100006 || poll.retcode == 100003)
            {
                ProcessMessageFlag = false;
                return;
            }
        }

        /// <summary>
        /// 处理poll包中的消息数组
        /// </summary>
        /// <param name="content">消息数组</param>
        /// <returns></returns>
        private string GetMessageText(List<object> content)
        {
            string message = "";
            for (int i = 1; i < content.Count; i++)
            {
                if (content[i].ToString().Contains("[\"cface\","))
                {
                    continue;
                }
                else if (content[i].ToString().Contains("\"face\","))
                {
                    message += ("{..[face" + content[i].ToString().Replace("\"face\",", "").Replace("]", "").Replace("[", "").Replace(" ", "").Replace("\r", "").Replace("\n", "") + "]..}");
                }
                else
                {
                    message += content[i].ToString();
                }
            }

            message = message.Replace("\\\\n", Environment.NewLine).Replace("＆", "&");

            return message;
        }

        #endregion

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="type">接受者类型：0，用户；1，群；2，讨论组</param>
        /// <param name="id">用户：uid；群：qid；讨论组：did</param>
        /// <param name="messageToSend">要发送的消息</param>
        /// <returns></returns>
        public bool SendMessage(int type, string id, string messageToSend)
        {
            if (messageToSend.Equals("") || id.Equals(""))
            {
                return false;
            }

            string[] tmp = messageToSend.Split("{}".ToCharArray());
            messageToSend = "";
            for (int i = 0; i < tmp.Length; i++)
            {
                if (!tmp[i].Trim().StartsWith("..[face") || !tmp[i].Trim().EndsWith("].."))
                {
                    messageToSend += "\\\"" + tmp[i] + "\\\",";
                }
                else
                {
                    messageToSend += tmp[i].Replace("..[face", "[\\\"face\\\",").Replace("]..", "],");
                }
            }

            messageToSend = messageToSend.Remove(messageToSend.LastIndexOf(','));
            messageToSend = messageToSend.Replace("\r\n", "\n").Replace("\n\r", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);

            try
            {
                string to_groupuin_did, url;
                switch (type)
                {
                    case 0:
                        to_groupuin_did = "to";
                        url = "http://d1.web2.qq.com/channel/send_buddy_msg2";
                        break;
                    case 1:
                        to_groupuin_did = "group_uin";
                        url = "http://d1.web2.qq.com/channel/send_qun_msg2";
                        break;
                    case 2:
                        to_groupuin_did = "did";
                        url = "http://d1.web2.qq.com/channel/send_discu_msg2";
                        break;
                    default:
                        return false;
                }

                string postData = "{\"#{type}\":#{id},\"content\":\"[#{msg},[\\\"font\\\",{\\\"name\\\":\\\"宋体\\\",\\\"size\\\":10,\\\"style\\\":[0,0,0],\\\"color\\\":\\\"000000\\\"}]]\",\"face\":#{face},\"clientid\":53999199,\"msg_id\":#{msg_id},\"psessionid\":\"#{psessionid}\"}";
                postData = "r=" + HttpUtility.UrlEncode(postData.Replace("#{type}", to_groupuin_did).Replace("#{id}", id).Replace("#{msg}", messageToSend).Replace("#{face}", this.smartQQ.Face.ToString()).Replace("#{msg_id}", rand.Next(10000000, 99999999).ToString()).Replace("#{psessionid}", this.smartQQ.PSessionId));

                string dat = HTTP.Post(url, postData, "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2");

                return dat.Equals("{\"errCode\":0,\"msg\":\"send ok\"}");
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
