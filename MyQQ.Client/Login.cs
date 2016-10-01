using Microsoft.Http;
using MyQQ.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyQQ.Client
{
    public partial class Login : Form
    {
        private readonly static string ClientID = GetClientID();
        private static readonly string BASE_IMAGE_SERVICE = "http://localhost:4788/";
                
        // 实例化Timer类，设置间隔时间为10000毫秒；
        System.Timers.Timer LoginStatusTimer = new System.Timers.Timer(10000);

        public Login()
        {
            InitializeComponent();

            GenerateQRCode();
            
            this.LoginStatusTimer.Enabled = true; // 是否执行System.Timers.Timer.Elapsed事件；
            this.LoginStatusTimer.AutoReset = true; // 设置是执行一次（false）还是一直执行(true)；   
            this.LoginStatusTimer.Interval = 10000;
            this.LoginStatusTimer.Elapsed += new System.Timers.ElapsedEventHandler(CheckQRCodeStatus);// 到达时间的时候执行事件；
        }
        
        public void CheckQRCodeStatus(object source, System.Timers.ElapsedEventArgs e)
        {
            // If the timer is paused, skip other request
            if (this.LoginStatusTimer.Enabled == false || String.IsNullOrEmpty(ClientID))
            {
                return;
            }

            String strSvcURI = "http://localhost:4788/" + "login/qrcode/status/" + ClientID;

            HttpClient client = new HttpClient();
            
            HttpResponseMessage responseMsg = client.Get(strSvcURI);
            responseMsg.EnsureStatusIsSuccessful();

            String strJson = responseMsg.Content.ReadAsString();

            ResponseWrapper<LoginEntity> loginStatusResult = JsonConvert.DeserializeObject<ResponseWrapper<LoginEntity>>(strJson);
            if (loginStatusResult.ReturnCode == 1)
            {
                // Pause the timer
                this.LoginStatusTimer.Enabled = false;               
                MessageBox.Show("Login Successfully.");
            }
            else
            {
                if (loginStatusResult.Result != null && loginStatusResult.Result.StatusCode == "65")
                {
                    // Pause the timer
                    this.LoginStatusTimer.Enabled = false;
                    MessageBox.Show(loginStatusResult.Result.StatusText);
                }
            }
        }

        private void btnGenerateQRCode_Click(object sender, EventArgs e)
        {
            // http://www.cnblogs.com/Leo_wl/p/4314797.html
            GenerateQRCode();
        }

        private void GenerateQRCode()
        {
            
            String strSvcURI = String.Format("{0}login/qrcode/{1}", BASE_IMAGE_SERVICE, ClientID);
            HttpClient client = new HttpClient();
            HttpResponseMessage responseMsg = client.Get(strSvcURI);
            responseMsg.EnsureStatusIsSuccessful();
            Stream stream = responseMsg.Content.ReadAsStream();
            this.picLoginQRCode.Image = Image.FromStream(stream);
            
            // Launch Timer
            this.LoginStatusTimer.Enabled = true;
        }

        private async void Login_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(ClientID))
            {
                this.Hide();
                string qqAccount = GetQQAccount();
                MainForm mainForm = new MainForm(ClientID, qqAccount);               
                mainForm.Show();
            }
        }

        private static string GetClientID()
        {
            String strSvcURI = "http://localhost:4788/" + "login/getclientid";

            HttpClient client = new HttpClient();

            HttpResponseMessage responseMsg = client.Get(strSvcURI);
            responseMsg.EnsureStatusIsSuccessful();

            String strJson = responseMsg.Content.ReadAsString();

            ResponseWrapper<string> loginStatusResult = JsonConvert.DeserializeObject<ResponseWrapper<string>>(strJson);
            if (loginStatusResult.ReturnCode == 1)
            {
                return loginStatusResult.Result;
            }

            return "";
        }

        private string GetQQAccount()
        {
            String strSvcURI = "http://localhost:4788/" + "account/getaccount/" + ClientID;

            HttpClient client = new HttpClient();

            HttpResponseMessage responseMsg = client.Get(strSvcURI);
            responseMsg.EnsureStatusIsSuccessful();

            String strJson = responseMsg.Content.ReadAsString();

            ResponseWrapper<string> getQQAccountResult = JsonConvert.DeserializeObject<ResponseWrapper<string>>(strJson);
            if (getQQAccountResult.ReturnCode == 1)
            {
                return getQQAccountResult.Result;
            }

            return "";
        }
    }
}
