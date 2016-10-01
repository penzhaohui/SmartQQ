// *   This program is free software: you can redistribute it and/or modify
// *   it under the terms of the GNU General Public License as published by
// *   the Free Software Foundation, either version 3 of the License, or
// *   (at your option) any later version.
// *
// *   This program is distributed in the hope that it will be useful,
// *   but WITHOUT ANY WARRANTY; without even the implied warranty of
// *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// *   GNU General Public License for more details.
// *
// *   You should have received a copy of the GNU General Public License
// *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
// *
// * @author     Xianglong He
// * @copyright  Copyright (c) 2015 Xianglong He. (http://tec.hxlxz.com)
// * @license    http://www.gnu.org/licenses/     GPL v3
// * @version    1.0
// * @discribe   RuiRuiQQRobot服务端
// * 本软件作者是何相龙，使用GPL v3许可证进行授权。

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace SmartQQ.util
{
    public class HTTP
    {
        //网络通信相关
        public static CookieContainer cookies = new CookieContainer();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="referer"></param>
        /// <param name="timeout"></param>
        /// <param name="encode"></param>
        /// <param name="NoProxy"></param>
        /// <returns></returns>
        public static string Get(string url, string referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2", int timeout = 100000, Encoding encode = null, bool NoProxy = false)
        {
            string dat;
            HttpWebResponse res = null;
            HttpWebRequest req;

            try
            {
                req = (HttpWebRequest)WebRequest.Create(url);
                req.CookieContainer = cookies;
                req.AllowAutoRedirect = false;
                req.Timeout = timeout;
                req.Referer = referer;
                if (NoProxy)
                    req.Proxy = null;
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0;%20WOW64; rv:47.0) Gecko/20100101 Firefox/47.0";
                res = (HttpWebResponse)req.GetResponse();

                cookies.Add(res.Cookies);              
            }
            //catch (HttpException)
            //{
            //    return "";
            //}
            catch (WebException)
            {
                return "";
            }

            StreamReader reader;
            reader = new StreamReader(res.GetResponseStream(), encode == null ? Encoding.UTF8 : encode);
            dat = reader.ReadToEnd();

            res.Close();
            req.Abort();
           
            return dat;
        }

        //http://www.itokit.com/2012/0721/74607.html
        public static string Post(string url, string data, string Referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2", int timeout = 100000, Encoding encode = null)
        {
            string dat = "";
            HttpWebRequest req;
            try
            {
                req = WebRequest.Create(url) as HttpWebRequest;
                req.CookieContainer = cookies;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Method = "POST";
                req.Proxy = null;
                req.Timeout = timeout;
                req.UserAgent = "Mozilla/5.0 (Windows NT 10.0;%20WOW64; rv:47.0) Gecko/20100101 Firefox/47.0";
                req.ProtocolVersion = HttpVersion.Version10;
                req.Referer = Referer;

                byte[] mybyte = Encoding.Default.GetBytes(data);
                req.ContentLength = mybyte.Length;

                Stream stream = req.GetRequestStream();
                stream.Write(mybyte, 0, mybyte.Length);

                HttpWebResponse res = req.GetResponse() as HttpWebResponse;                                        
                
                cookies.Add(res.Cookies);

                stream.Close();

                StreamReader SR = new StreamReader(res.GetResponseStream(), encode == null ? Encoding.UTF8 : encode);
                dat = SR.ReadToEnd();
                res.Close();
                req.Abort();
            }
            //catch (HttpException)
            //{
            //    return "";
            //}
            catch (WebException)
            {
                return "";
            }
            
            return dat;
        }

        public delegate void Post_Async_Action(string data);
        private class Post_Async_Data
        {
            public HttpWebRequest req;
            public Post_Async_Action post_Async_Action;
        }
        public static void Post_Async(string url, string PostData, Post_Async_Action action, string Referer = "http://d1.web2.qq.com/proxy.html?v=20151105001&callback=1&id=2", int timeout = 100000)
        {
            HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
            req.CookieContainer = cookies;
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            req.Referer = Referer;
            req.UserAgent = "Mozilla/5.0 (Windows NT 10.0;%20WOW64; rv:47.0) Gecko/20100101 Firefox/47.0";
            req.Proxy = null;
            req.ProtocolVersion = HttpVersion.Version10;
            req.ContinueTimeout = timeout;

            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(PostData);
            Stream stream = req.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            Post_Async_Data dat = new Post_Async_Data();
            dat.req = req;
            dat.post_Async_Action = action;
            req.BeginGetResponse(new AsyncCallback(Post_Async_ResponesProceed), dat);
        }

        private static void Post_Async_ResponesProceed(IAsyncResult ar)
        {
            StreamReader reader = null;
            Post_Async_Data dat = ar.AsyncState as Post_Async_Data;
            HttpWebRequest req = dat.req;
            HttpWebResponse res = req.GetResponse() as HttpWebResponse;
            reader = new StreamReader(res.GetResponseStream());
            string temp = reader.ReadToEnd();
            res.Close();
            req.Abort();
            dat.post_Async_Action(temp);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="type">1，10位；2，13位。</param>
        /// <returns></returns>
        public static string AID_TimeStamp(int type = 1)
        {
            if (type == 1)
            {
                DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return ((DateTime.UtcNow.Ticks - dt1970.Ticks) / 10000).ToString();
            }
            else if (type == 2)
            {
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return Convert.ToInt64(ts.TotalSeconds).ToString();
            }
            else return "ERROR";
        }

        /// <summary>
        /// 根据QQ号和ptwebqq值获取hash值，用于获取好友列表和群列表
        /// </summary>
        /// <param name="QQNum">QQ号</param>
        /// <param name="ptwebqq">ptwebqq</param>
        /// <returns>hash值</returns>
        public static string AID_Hash(string QQNum, string ptwebqq)
        {
            int[] N = new int[4];
            long QQNum_Long = long.Parse(QQNum);
            for (int T = 0; T < ptwebqq.Length; T++)
            {
                N[T % 4] ^= ptwebqq.ToCharArray()[T];
            }
            string[] U = { "EC", "OK" };
            long[] V = new long[4];
            V[0] = QQNum_Long >> 24 & 255 ^ U[0].ToCharArray()[0];
            V[1] = QQNum_Long >> 16 & 255 ^ U[0].ToCharArray()[1];
            V[2] = QQNum_Long >> 8 & 255 ^ U[1].ToCharArray()[0];
            V[3] = QQNum_Long & 255 ^ U[1].ToCharArray()[1];

            long[] U1 = new long[8];

            for (int T = 0; T < 8; T++)
            {
                U1[T] = T % 2 == 0 ? N[T >> 1] : V[T >> 1];
            }

            string[] N1 = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
            string V1 = "";

            for (int i = 0; i < U1.Length; i++)
            {
                V1 += N1[(int)((U1[i] >> 4) & 15)];
                V1 += N1[(int)(U1[i] & 15)];
            }
            return V1;
        }
    }
}
