using Microsoft.Http;
using MyQQ.Client;
using MyQQ.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyQQ
{
    public partial class MainForm : Form
    {
        public string ClientId { get; set; }
        public string QQAccount { get; set; }
        private static readonly string BASE_IMAGE_SERVICE = "http://localhost:4788/";
        
        public MainForm(string clientId, string qqAccount)
        {
            InitializeComponent();

            this.ClientId = clientId;
            this.QQAccount = qqAccount;

            this.Text = this.QQAccount + ", welcome to SmartQQ!";
            //LoadMyFriends(clientId);
            LoadMyGroups(clientId);
            LoadMyDiscussionGroups(clientId);
            LoadQQMessage(clientId);
        }

        private async void LoadQQMessage(string clientId)
        {
            String strSvcURI = BASE_IMAGE_SERVICE + "message/receive/" + clientId;
            strSvcURI += "/" + DateTime.Now.Ticks;

            HttpClient client = new HttpClient();

            HttpResponseMessage responseMsg = client.Get(strSvcURI);
            responseMsg.EnsureStatusIsSuccessful();

            String strJson = responseMsg.Content.ReadAsString();

            ResponseWrapper<List<MessageEntity>> getFriendsResult = JsonConvert.DeserializeObject<ResponseWrapper<List<MessageEntity>>>(strJson);
            if (getFriendsResult.ReturnCode == 1)
            {
                this.txtShowMessageWindow.Text = "";
                string message = "";
                foreach (var entity in getFriendsResult.Result)
                {
                    if (!String.IsNullOrEmpty(entity.GroupName))
                    {
                        message = entity.CreateTime.ToString() + System.Environment.NewLine;
                        message += "----------------------------------" + System.Environment.NewLine;
                        message += entity.GroupName + " - " + entity.MessagerName + "[" + entity.MessagerAccount + "]: " + entity.MessageContent;
                        message += System.Environment.NewLine;
                    }
                    else if (!String.IsNullOrEmpty(entity.DiscussionName))
                    {
                        message = entity.CreateTime.ToString() + System.Environment.NewLine;
                        message += "----------------------------------" + System.Environment.NewLine;
                        message += entity.DiscussionName + " - " + entity.MessagerName + "[" + entity.MessagerAccount + "]: " + entity.MessageContent;
                        message += System.Environment.NewLine;
                    }
                    else
                    {
                        message = entity.CreateTime.ToString() + System.Environment.NewLine;
                        message += "----------------------------------" + System.Environment.NewLine;
                        message += entity.MessagerName + "[" + entity.MessagerAccount + "]: " + entity.MessageContent;
                        message += System.Environment.NewLine;
                    }

                    this.txtShowMessageWindow.Text += message;
                }
            }
            else
            {
                MessageBox.Show(getFriendsResult.Message);
            }
        }

        private async void LoadMyFriends(string clientId)
        {
            String strSvcURI = BASE_IMAGE_SERVICE + "account/getfriends/" + clientId;

            HttpClient client = new HttpClient();

            HttpResponseMessage responseMsg = client.Get(strSvcURI);
            responseMsg.EnsureStatusIsSuccessful();

            String strJson = responseMsg.Content.ReadAsString();

            ResponseWrapper<List<FriendEntity>> getFriendsResult = JsonConvert.DeserializeObject<ResponseWrapper<List<FriendEntity>>>(strJson);
            if (getFriendsResult.ReturnCode == 1)
            {
                this.lstMyFriends.BeginUpdate();
                this.lstMyFriends.Items.Clear();

                foreach (var friend in getFriendsResult.Result)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = friend.Name + "(" + friend.QQNo + ")";
                    this.lstMyFriends.Items.Add(item);
                }
                this.lstMyFriends.EndUpdate();
            }
            else
            {
                MessageBox.Show(getFriendsResult.Message);
            }
        }

        private async void LoadMyGroups(string clientId)
        {
            String strSvcURI = BASE_IMAGE_SERVICE + "account/getgroups/" + clientId;

            HttpClient client = new HttpClient();

            HttpResponseMessage responseMsg = client.Get(strSvcURI);
            responseMsg.EnsureStatusIsSuccessful();

            String strJson = responseMsg.Content.ReadAsString();

            ResponseWrapper<List<GroupEntity>> getGroupsResult = JsonConvert.DeserializeObject<ResponseWrapper<List<GroupEntity>>>(strJson);
            if (getGroupsResult.ReturnCode == 1)
            {
                this.lstMyQQGroups.BeginUpdate();
                this.lstMyQQGroups.Items.Clear();

                foreach (var group in getGroupsResult.Result)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = group.GroupName + "(" + group.GroupID + ")";
                    this.lstMyQQGroups.Items.Add(item);
                }
                this.lstMyQQGroups.EndUpdate();
            }
            else
            {
                MessageBox.Show(getGroupsResult.Message);
            }
        }

        private async void LoadMyDiscussionGroups(string clientId)
        {
            String strSvcURI = BASE_IMAGE_SERVICE + "account/getdiscussiongroups/" + clientId;

            HttpClient client = new HttpClient();

            HttpResponseMessage responseMsg = client.Get(strSvcURI);
            responseMsg.EnsureStatusIsSuccessful();

            String strJson = responseMsg.Content.ReadAsString();

            ResponseWrapper<List<DiscussionGroupEntity>> getDicussionGroupsResult = JsonConvert.DeserializeObject<ResponseWrapper<List<DiscussionGroupEntity>>>(strJson);
            if (getDicussionGroupsResult.ReturnCode == 1)
            {
                this.lstMyDiscussionGroups.BeginUpdate();
                this.lstMyDiscussionGroups.Items.Clear();

                foreach (var group in getDicussionGroupsResult.Result)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = group.GroupName + "(" + group.GroupID + ")";
                    this.lstMyDiscussionGroups.Items.Add(item);
                }
                this.lstMyDiscussionGroups.EndUpdate();
            }
            else
            {
                MessageBox.Show(getDicussionGroupsResult.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadQQMessage(ClientId);
        }
    }
}
