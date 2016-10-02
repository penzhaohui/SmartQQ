using MyQQ.Entity;
using MyQQ.Server.Util;
using MyQQ.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;

namespace MyQQ
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class MessageService
    {
        readonly MyQQDAL myQQDAL = null;

        MessageService()
        {
            myQQDAL = new MyQQDAL();
        }

        [OperationContract]
        [WebGet(UriTemplate = "/receive/{clientId}/{ticks}", ResponseFormat = WebMessageFormat.Json)]
        public ResponseWrapper<List<MessageEntity>> ReceiveMessage(string clientId, string ticks)
        {
            ResponseWrapper<List<MessageEntity>> response = new ResponseWrapper<List<MessageEntity>>();

            if (CacheUtil.Exists(clientId) == false)
            {
                response.ReturnCode = 0;
                response.Message = "Login Failed";
                response.InnerMessage = "The client id is expired or does not exists.";
                response.Result = null;

                return response;
            }

            DateTime endTime = new DateTime(long.Parse(ticks));
            DateTime startTime = endTime.AddHours(-12);            

            var MyQQEntity = (MyQQEntity)CacheUtil.Get(clientId);
            string qqAccount = MyQQEntity.QQAccount;

            List<MessageEntity> messages = myQQDAL.GetMessageList(qqAccount, startTime, endTime);

            response.ReturnCode = 1;
            response.Message = "";
            response.InnerMessage = "";
            response.Result = messages;

            return response;
        }

        [OperationContract]
        [WebGet(UriTemplate = "/send/{clientId}", ResponseFormat = WebMessageFormat.Json)]
        public void SendMessage(string clientId)
        {
        }
    }
}