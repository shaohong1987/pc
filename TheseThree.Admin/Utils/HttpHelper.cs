using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Channels;
using System.Text;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using TheseThree.Admin.Models.Entities;

namespace TheseThree.Admin.Utils
{
    public class HttpHelper
    {
        public static void SendSms(List<RegSms> regSmses)
        {
            var str = "[";
            foreach (var regSmse in regSmses)
            {
                str += "{";
                str += "\"name\":\"" + regSmse.Name + "\",";
                str += "\"hosname\":\"" + regSmse.HosName + "\",";
                str += "\"phone\":\"" + regSmse.Phone + "\"";
                str += "},";
            }
            str = str.Substring(0, str.Length - 1) + "]";
            Post(str);
        }

        public static void SendSmsKs(List<SmsKs> list)
        {
            var str = "[";
            foreach (var regSmse in list)
            {
                str += "{";
                str += "\"phone\":\"" + regSmse.Phone + "\",";
                str += "\"neirong\":\"" + regSmse.NeiRong + "\",";
                str += "\"didian\":\"" + regSmse.DiDian + "\",";
                str += "\"shijian\":\"" + regSmse.ShiJian + "\"";
                str += "},";
            }
            str = str.Substring(0, str.Length - 1) + "]";
            PostKs(str);
        }

        public static void SendSmsPx(List<SmsPx> list)
        {
            var str = "[";
            foreach (var regSmse in list)
            {
                str += "{";
                str += "\"phone\":\"" + regSmse.Phone + "\",";
                str += "\"zhuti\":\"" + regSmse.ZhuTi + "\",";
                str += "\"didian\":\"" + regSmse.DiDian + "\",";
                str += "\"shijian\":\"" + regSmse.ShiJian + "\"";
                str += "},";
            }
            str = str.Substring(0, str.Length - 1) + "]";
            PostPx(str);
        }

        private static void Post(string jsonParas)
        {
            const string url = "http://api.js00000000.com:8880/OrderService/postJsonsendsms";
            var request = (HttpWebRequest) WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var paraUrlCoded = System.Web.HttpUtility.UrlEncode("userphone");
            paraUrlCoded += "=" + System.Web.HttpUtility.UrlEncode(jsonParas);
            var payload = Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer;
            try
            {
                writer = request.GetRequestStream();
            }
            catch (Exception)
            {
                writer = null;
            }
            if (writer != null)
            {
                writer.Write(payload, 0, payload.Length);
                writer.Close();
            }
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse) request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }

            if (response != null)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //可以在此实现记录
                }
                else
                {
                    //可以在此实现记录
                }
            }
        }
        private static void PostKs(string jsonParas) 
        {
            const string url = "http://api.js00000000.com:8880/OrderService/postJsonsendsmsks";
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var paraUrlCoded = System.Web.HttpUtility.UrlEncode("userphone");
            paraUrlCoded += "=" + System.Web.HttpUtility.UrlEncode(jsonParas);
            var payload = Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer;
            try
            {
                writer = request.GetRequestStream();
            }
            catch (Exception)
            {
                writer = null;
            }
            if (writer != null)
            {
                writer.Write(payload, 0, payload.Length);
                writer.Close();
            }
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }

            if (response != null)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //可以在此实现记录
                }
                else
                {
                    //可以在此实现记录
                }
            }
        }
        private static void PostPx(string jsonParas)
        {
            const string url = "http://api.js00000000.com:8880/OrderService/postJsonsendsmspx";
            var request = (HttpWebRequest)WebRequest.Create(url); 
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var paraUrlCoded = System.Web.HttpUtility.UrlEncode("userphone");
            paraUrlCoded += "=" + System.Web.HttpUtility.UrlEncode(jsonParas);
            var payload = Encoding.UTF8.GetBytes(paraUrlCoded);
            request.ContentLength = payload.Length;
            Stream writer;
            try
            {
                writer = request.GetRequestStream();
            }
            catch (Exception)
            {
                writer = null;
            }
            if (writer != null)
            {
                writer.Write(payload, 0, payload.Length);
                writer.Close();
            }
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }

            if (response != null)
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //可以在此实现记录
                }
                else
                {
                    //可以在此实现记录
                }
            }
        }
        public static void IosPush(List<string> myDeviceTokens, string content)
        {
            //可以将推送模板搞到这边实现
            Push(myDeviceTokens,content);
        }
        private static void Push(IEnumerable<string> myDeviceTokens,string content)
        {
            var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Production,
            "c:\\backup\\final_iphone.p12", "lw65866447");
            var apnsBroker = new ApnsServiceBroker(config);
            apnsBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {
                aggregateEx.Handle(ex =>
                {
                    if (ex is ApnsNotificationException)
                    {
                        var notificationException = (ApnsNotificationException) ex;
                        var apnsNotification = notificationException.Notification;
                        var statusCode = notificationException.ErrorStatusCode;
                        //失败原因
                    }
                    else
                    {	
                       //未知原因失败
                    }
                    return true;
                });
            };
            apnsBroker.OnNotificationSucceeded += (notification) =>
            {
              //成功了  
            };
            apnsBroker.Start();
            foreach (var deviceToken in myDeviceTokens)
            {
                apnsBroker.QueueNotification(new ApnsNotification
                {
                    DeviceToken = deviceToken,
                    Payload = JObject.Parse(content)
                });
            }
            apnsBroker.Stop();
        }
    }
}