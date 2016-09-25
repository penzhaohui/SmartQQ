using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using SmartQQ;
using MyQQ.Entity;
using MyQQ.Server.Util;
using SmartQQ.model;
using MyQQ.Util;

namespace MyQQ
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class LoginService
    {
        readonly SmartQQ.LoginService loginService = null;
        readonly MyQQDAL myQQDAL = null;

        LoginService()
        {
            loginService = new SmartQQ.LoginService();
            myQQDAL = new MyQQDAL();
        }

        [OperationContract]
        [WebGet(UriTemplate = "/getclientid", ResponseFormat = WebMessageFormat.Json)]
        public ResponseWrapper<string> GetClientId()
        {
            ResponseWrapper<string> response = new ResponseWrapper<string>();
            string clientId = TokenUtil.NewToken();

            MyQQEntity myQQEntity = new MyQQEntity();            
            myQQEntity.ClientID = clientId;
            myQQEntity.IsInitialized = false;

            CacheUtil.Add(clientId, myQQEntity);

            response.ReturnCode = 1;
            response.Message = "Get client id successfully.";
            response.InnerMessage = "Please remember to append the client id value for the next api request.";
            response.Result = clientId;

            return response;
        }

        [OperationContract]
        [WebGet(UriTemplate = "/qrcode/{clientId}", ResponseFormat = WebMessageFormat.Json)]
        public Stream GetQRCodeImage(string clientId)
        {
            if (CacheUtil.Exists(clientId) == false)
            {
                return null;
            }

            return loginService.GetQRCodeStream();
        }

        [OperationContract]
        [WebGet(UriTemplate = "/qrcode/status/{clientId}", ResponseFormat = WebMessageFormat.Json)]
        public ResponseWrapper<LoginStatus> GetQRCodeStatus(string clientId)
        {
            ResponseWrapper<LoginStatus> response = new ResponseWrapper<LoginStatus>();

            if (CacheUtil.Exists(clientId) == false)
            {
                response.ReturnCode = 0;
                response.Message = "Login Failed";
                response.InnerMessage = "You might not forget pass the client id.";
                response.Result = null;

                return response;
            }

            LoginStatus loginStatus = new LoginStatus();
            var result = loginService.CheckQRCodeStatus();

            if (result == null)
            {
                response.ReturnCode = 0;
                response.Message = "Login Failed";
                response.InnerMessage = "You might forget to scan QR code first.";
                response.Result = null;

                return response;
            }

            loginStatus.StatusCode = result.StatusCode;
            loginStatus.StatusText = result.StatusText;           
           
            response.Result = loginStatus;

            switch (result.StatusCode)
            { 
                case "65":
                    response.ReturnCode = 0;
                    response.Message = "Login Failed";
                    response.InnerMessage = "二维码失效";
                    break;
                case "66":
                    response.ReturnCode = 0;
                    response.Message = "Login Failed";
                    response.InnerMessage = "等待扫描";
                    break;
                case "67":
                    response.ReturnCode = 0;
                    response.Message = "Login Failed";
                    response.InnerMessage = "等待确认";
                    break;
                case "0":
                    response.ReturnCode = 1;
                    response.Message = "";
                    response.InnerMessage = "已经确认";

                    ProcessLoginRequest(clientId, result.RedirectUrl);

                    break;
            }

            return response;
        }

        /// <summary>
        /// Process Login Request
        /// </summary>
        /// <param name="token"></param>
        /// <param name="redirectUrl"></param>
        private async void ProcessLoginRequest(string clientId, string redirectUrl)
        {
            var MyQQEntity = (MyQQEntity)CacheUtil.Get(clientId);
            lock (MyQQEntity)
            {
                if (MyQQEntity.IsInitialized)
                {
                    return;
                }
                else
                {
                    MyQQEntity.IsInitialized = true;
                    CacheUtil.Update(clientId, MyQQEntity);
                }
            }

            var smartQQWarapper = loginService.ProcessLoginRequest(redirectUrl);            
            
            SmartQQ.AccountService accountService = new SmartQQ.AccountService(smartQQWarapper);
            
            smartQQWarapper.GroupAccounts = accountService.GetGroupList(true);
            System.Console.WriteLine("Initialize QQ groups successfully.");            

            smartQQWarapper.DiscussionAccounts = accountService.GetDiscussionGroupList(true);
            System.Console.WriteLine("Initialize QQ discussions successfully.");

            smartQQWarapper.FriendAccounts = accountService.GetFriendList(true);
            System.Console.WriteLine("Initialize QQ friends successfully.");

            smartQQWarapper = accountService.GetQQProfile();
            System.Console.WriteLine("Initialize QQ profile successfully.");  

            smartQQWarapper = accountService.GetQQProfile();
            smartQQWarapper.Online = true;

            MyQQEntity.SmartQQ = smartQQWarapper;
            CacheUtil.Update(clientId, MyQQEntity);

            myQQDAL.InitializedSmartQQ(smartQQWarapper);
            System.Console.WriteLine("Initialize MyQQ context successfully.");
        }
    }
}