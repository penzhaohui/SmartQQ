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

namespace MyQQ
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class LoginService
    {
        readonly SmartQQ.LoginService loginService = null;

        LoginService()
        {
            loginService = new SmartQQ.LoginService();           
        }

        [OperationContract]
        [WebGet(UriTemplate = "/qrcode", ResponseFormat = WebMessageFormat.Json)]
        public Stream GetQRCodeImage()
        {            return loginService.GetQRCodeStream();
        }

        [OperationContract]
        [WebGet(UriTemplate = "/qrcode/status", ResponseFormat = WebMessageFormat.Json)]
        public ResponseWrapper<LoginStatus> GetQRCodeStatus()
        {
            ResponseWrapper<LoginStatus> response = new ResponseWrapper<LoginStatus>();
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
                    loginStatus.Token = TokenUtil.NewToken();

                    ProcessLoginRequest(loginStatus.Token, result.RedirectUrl);

                    break;
            }

            return response;
        }

        /// <summary>
        /// Process Login Request
        /// </summary>
        /// <param name="token"></param>
        /// <param name="redirectUrl"></param>
        private async void ProcessLoginRequest(string token, string redirectUrl)
        {
            var smartQQWarapper = loginService.ProcessLoginRequest(redirectUrl);            
            
            SmartQQ.AccountService accountService = new SmartQQ.AccountService(smartQQWarapper);
            smartQQWarapper = accountService.GetQQProfile();
            CacheUtil.Add(token, smartQQWarapper);

            smartQQWarapper.GroupAccounts = accountService.GetGroupList(true);
            smartQQWarapper.DiscussionAccounts = accountService.GetDiscussionGroupList(true);

            CacheUtil.Update(token, smartQQWarapper);

            System.Console.WriteLine("Initialize MyQQ context successfully.");
        }
    }
}