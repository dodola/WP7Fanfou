using System;
using System.IO;
using System.Net;
using System.Text;
using MetroFanfou.SDK.Objects;

namespace MetroFanfou.SDK.Http
{
    public class XAuth : BaseXAuthRequest
    {
        public const string AccessTokenUrl = "http://fanfou.com/oauth/access_token";
        public const string VerifyUrl = "http://api.fanfou.com/account/verify_credentials.xml";

        /// <summary>
        /// 授权过程中最后一次发生的错误
        /// </summary>
        public Exception LastError { get; private set; }

        public String AccessString { get; set; }
        private string postData;

        private Action<string> innerCallback;

        public void GetAccessToken(string username, string password, Action<string> callback)
        {
            innerCallback = callback;
            string authorizationHeader = GetXAuthorizationHeader(AccessTokenUrl, "post", username, password);

            postData = string.Format("x_auth_mode={0}&x_auth_password={1}&x_auth_username={2}", "client_auth", password, UrlEncode(username));

            var request = (HttpWebRequest)System.Net.WebRequest.Create(AccessTokenUrl);
            request.Headers["Authorization"] = authorizationHeader;

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);



        }

        private void GetRequestStreamCallback(IAsyncResult ar)
        {
            HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
            System.IO.Stream postStream = request.EndGetRequestStream(ar);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            postStream.Write(byteArray, 0, postData.Length);
            postStream.Close();
            request.BeginGetResponse(new AsyncCallback(GetResponseCallBack), request);

        }

        private void GetResponseCallBack(IAsyncResult ar)
        {
            HttpWebRequest request = (HttpWebRequest)ar.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(ar);
            Stream streamResponse = response.GetResponseStream();
            StreamReader streamRead = new StreamReader(streamResponse);
            AccessString = streamRead.ReadToEnd();
            // Close the stream object
            streamResponse.Close();
            streamRead.Close();
            innerCallback(AccessString);

        }

        /// <summary>
        /// 获取AccessToken结束,返回用户名称给回调方法
        /// </summary>
        /// <param name="response"></param>
        /// <param name="callback"></param>
        private void EndAccessToken(string response, Action<string> callback)
        {
            if (!string.IsNullOrEmpty(response))
            {
                var responseData = Util.GetQueryParameters(response);
                this.Token = responseData["oauth_token"];
                this.TokenSecret = responseData["oauth_token_secret"];
                if (callback != null)
                {
                    callback(responseData["name"]);
                }
            }
            else
            {
                if (callback != null)
                {
                    callback("");
                }
            }
        }

        //public string GetAccessToken(string username, string password)
        //{
        //    string authorizationHeader = GetXAuthorizationHeader(AccessTokenUrl, "post", username, password);

        //    string postData = String.Format("x_auth_mode={0}&x_auth_password={1}&x_auth_username={2}", "client_auth", password, UrlEncode(username));

        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uploadApiUrl);
        //    request.Headers.Add("Authorization", authorizationHeader);

        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = postData.Length;

        //    using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
        //    {
        //        sw.Write(postData);
        //        sw.Flush();
        //    }
        //    try
        //    {
        //        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        //        {
        //            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")))
        //            {
        //                string result = sr.ReadToEnd();
        //                return result;
        //            }
        //        }
        //    }
        //    catch (WebException ex)
        //    {
        //        using (HttpWebResponse response = ex.Response as HttpWebResponse)
        //        {
        //            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("gbk")))
        //            {
        //                string result = sr.ReadToEnd();
        //                return result;
        //            }
        //        }
        //    }
        //}
    }
}