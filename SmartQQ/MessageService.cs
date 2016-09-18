using SmartQQ.model;
using SmartQQ.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartQQ
{
    public class MessageService
    {
        public delegate void ProcessMessageHandler(QQMessage message);
        public delegate void SendMessageHandler(QQMessage message);

        private ProcessMessageHandler processMessageAction;
        public MessageService(ProcessMessageHandler processMessageAction)
        {
            this.processMessageAction = processMessageAction;
            Task.Run(() => RequestMessage());
        }

        private void RequestMessage()
        {
            //try
            //{
            //    string url = "http://d1.web2.qq.com/channel/poll2";
            //    string HeartPackdata = "{\"ptwebqq\":\"#{ptwebqq}\",\"clientid\":53999199,\"psessionid\":\"#{psessionid}\",\"key\":\"\"}";
            //    HeartPackdata = HeartPackdata.Replace("#{ptwebqq}", ptwebqq).Replace("#{psessionid}", psessionid);
            //    HeartPackdata = "r=" + HttpUtility.UrlEncode(HeartPackdata);
            //    HTTP.Post_Async_Action action = Message_Get;
            //    HTTP.Post_Async(url, HeartPackdata, action);
            //}
            //catch (Exception) { Message_Request(); }
        }
    }
}
