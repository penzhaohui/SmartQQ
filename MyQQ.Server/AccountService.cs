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
    public class AccountService
    {
        readonly MyQQDAL myQQDAL = null;

        AccountService()
        {
            myQQDAL = new MyQQDAL();
        }

        [OperationContract]
        [WebGet(UriTemplate = "/getaccount/{clientId}", ResponseFormat = WebMessageFormat.Json)]
        public ResponseWrapper<string> GetAccount(string clientId)
        {
            ResponseWrapper<string> response = new ResponseWrapper<string>();

            if (CacheUtil.Exists(clientId) == false)
            {
                response.ReturnCode = 0;
                response.Message = "Login Failed";
                response.InnerMessage = "The client id is expired or does not exists.";
                response.Result = null;

                return response;
            }

            var MyQQEntity = (MyQQEntity)CacheUtil.Get(clientId);

            response.ReturnCode = 1;
            response.Message = "";
            response.InnerMessage = "";
            response.Result = MyQQEntity.QQAccount;

            return response;
        }

        [OperationContract]
        [WebGet(UriTemplate = "/getfriends/{clientId}", ResponseFormat = WebMessageFormat.Json)]
        public ResponseWrapper<List<FriendEntity>> GetFriendList(string clientId)
        {
            ResponseWrapper<List<FriendEntity>> response = new ResponseWrapper<List<FriendEntity>>();
            
            if (CacheUtil.Exists(clientId) == false)
            {
                response.ReturnCode = 0;
                response.Message = "Login Failed";
                response.InnerMessage = "The client id is expired or does not exists.";
                response.Result = null;

                return response;
            }

            var MyQQEntity = (MyQQEntity)CacheUtil.Get(clientId);

            List<FriendEntity> friends = myQQDAL.GetFrisendsByQQAccount(MyQQEntity.QQAccount);

            response.ReturnCode = 1;
            response.Message = "";
            response.InnerMessage = "";
            response.Result = friends;

            return response;
        }

        [OperationContract]
        [WebGet(UriTemplate = "/getgroups/{clientId}", ResponseFormat = WebMessageFormat.Json)]
        public ResponseWrapper<List<GroupEntity>> GetGroupList(string clientId)
        {
            ResponseWrapper<List<GroupEntity>> response = new ResponseWrapper<List<GroupEntity>>();

            if (CacheUtil.Exists(clientId) == false)
            {
                response.ReturnCode = 0;
                response.Message = "Login Failed";
                response.InnerMessage = "The client id is expired or does not exists.";
                response.Result = null;

                return response;
            }

            var MyQQEntity = (MyQQEntity)CacheUtil.Get(clientId);

            List<GroupEntity> groups = myQQDAL.GetQQGroupsByQQAccount(MyQQEntity.QQAccount);

            response.ReturnCode = 1;
            response.Message = "";
            response.InnerMessage = "";
            response.Result = groups;

            return response;
        }

        [OperationContract]
        [WebGet(UriTemplate = "/getgroupmembers/{clientId}/{groupId}", ResponseFormat = WebMessageFormat.Json)]
        public ResponseWrapper<List<GroupMemberEntity>> GetGroupMemberList(string clientId, string groupId)
        {
            ResponseWrapper<List<GroupMemberEntity>> response = new ResponseWrapper<List<GroupMemberEntity>>();

            if (CacheUtil.Exists(clientId) == false)
            {
                response.ReturnCode = 0;
                response.Message = "Login Failed";
                response.InnerMessage = "The client id is expired or does not exists.";
                response.Result = null;

                return response;
            }

            List<GroupMemberEntity> members = myQQDAL.GetQQGroupMembersByGroupId(groupId);

            response.ReturnCode = 1;
            response.Message = "";
            response.InnerMessage = "";
            response.Result = members;

            return response;
        }

        [OperationContract]
        [WebGet(UriTemplate = "/getdiscussiongroups/{clientId}", ResponseFormat = WebMessageFormat.Json)]
        public ResponseWrapper<List<DiscussionGroupEntity>> GetDiscussionList(string clientId)
        {
            ResponseWrapper<List<DiscussionGroupEntity>> response = new ResponseWrapper<List<DiscussionGroupEntity>>();

            if (CacheUtil.Exists(clientId) == false)
            {
                response.ReturnCode = 0;
                response.Message = "Login Failed";
                response.InnerMessage = "The client id is expired or does not exists.";
                response.Result = null;

                return response;
            }

            var MyQQEntity = (MyQQEntity)CacheUtil.Get(clientId);

            List<DiscussionGroupEntity> groups = myQQDAL.GetQQDicusstionGroupsByQQAccount(MyQQEntity.QQAccount);

            response.ReturnCode = 1;
            response.Message = "";
            response.InnerMessage = "";
            response.Result = groups;

            return response;
        }

        [OperationContract]
        [WebGet(UriTemplate = "/getdiscussiongroupmembers/{clientId}/{groupId}", ResponseFormat = WebMessageFormat.Json)]
        public ResponseWrapper<List<DicussionGroupMemberEntity>> GetDiscussionMemberList(string clientId, string groupId)
        {
            ResponseWrapper<List<DicussionGroupMemberEntity>> response = new ResponseWrapper<List<DicussionGroupMemberEntity>>();

            if (CacheUtil.Exists(clientId) == false)
            {
                response.ReturnCode = 0;
                response.Message = "Login Failed";
                response.InnerMessage = "The client id is expired or does not exists.";
                response.Result = null;

                return response;
            }

            List<DicussionGroupMemberEntity> members = myQQDAL.GetQQDicusstionGroupMembersByGroupId(groupId);

            response.ReturnCode = 1;
            response.Message = "";
            response.InnerMessage = "";
            response.Result = members;

            return response;
        }
    }
}