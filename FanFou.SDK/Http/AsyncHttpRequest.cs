using System;
using System.IO;
using System.Net;
using System.Text;
using FanFou.SDK.Objects;

namespace FanFou.SDK.Http
{
    /// <summary>
    /// 异步的HTTP请求
    /// </summary>
    public class AsyncHttpRequest
    {
        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Charset = Encoding.UTF8;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="url"></param>
        public AsyncHttpRequest(string url)
            : this(url, null, Encoding.UTF8)
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="url"></param>
        /// <param name="charset"></param>
        public AsyncHttpRequest(string url, Encoding charset)
            : this(url, null, charset)
        {
        }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="url"></param>
        /// <param name="func"></param>
        /// <param name="charset"></param>
        public AsyncHttpRequest(string url, Action<string> func, Encoding charset)
        {
            Url = url;
            Timeout = 30000;
            Charset = charset;
            OAuthCallback = func;
        }

        /// <summary>
        /// 超时,单位:毫秒
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// 需要请求的地址
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// 查询参数
        /// </summary>
        public Parameters Parameters { get; set; }

        public String AuthHeader { get; set; }

        /// <summary>
        /// OAuthEndAction方法
        /// </summary>
        public Action<string, Action<string>> OAuthEndAction { get; set; }

        /// <summary>
        /// 回应方法
        /// </summary>
        public Action<string> OAuthCallback { get; set; }

        /// <summary>
        /// 将要发送的文件
        /// </summary>
        public Files Files { get; set; }

        #region 方法动作

        /// <summary>
        /// GET请求
        /// </summary>
        /// <returns></returns>
        private void Get()
        {
            string queryString = Parameters == null ? "" : Parameters.BuildQueryString(true);
            string url = Url;
            if (!string.IsNullOrEmpty(queryString))
            {
                url = string.Concat(url, url.IndexOf('?') == -1 ? '?' : '&', queryString);
            }
            HttpWebRequest request = HttpUtil.CreateRequest("GET", url, Timeout);
            request.BeginGetResponse(AsyncResponseCallback, request);
        }


        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="responseEnd"></param>
        /// <param name="callback"></param>
        public void Get(Action<string, Action<string>> responseEnd, Action<string> callback)
        {
            OAuthCallback = callback;
            OAuthEndAction = responseEnd;
            Get();
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="responseEnd"></param>
        /// <param name="callback"></param>
        public void Post(Action<string, Action<string>> responseEnd, Action<string> callback)
        {
            OAuthCallback = callback;
            OAuthEndAction = responseEnd;
            Post("application/x-www-form-urlencoded");
        }

        /// <summary>
        /// Post请求,带图片
        /// </summary>
        /// <param name="responseEnd"></param>
        /// <param name="files"></param>
        /// <param name="callback"></param>
        public void PostFile(Action<string, Action<string>> responseEnd, Files files, Action<string> callback)
        {
            OAuthCallback = callback;
            OAuthEndAction = responseEnd;
            Files = files;

            HttpWebRequest request = HttpUtil.CreateRequest("POST", Url, Timeout);
            request.Headers["Authorization"] = AuthHeader;
            if (Parameters != null && Parameters.Items.Count != 0)
            {
                request.BeginGetRequestStream(GetPostFileRequestStreamCallback, request);
            }
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <returns></returns>
        private void Post(string contentType)
        {
            HttpWebRequest request = HttpUtil.CreateRequest("POST", Url, Timeout);
            request.ContentType = contentType;
            request.AllowReadStreamBuffering = true;

            if (Parameters != null && Parameters.Items.Count != 0)
            {
                request.BeginGetRequestStream(GetRequestStreamCallback, request);
            }
        }

        #endregion

        /// <summary>
        /// 异步回调
        /// </summary>
        /// <param name="asynchronousResult"></param>
        public void AsyncResponseCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                var request = (HttpWebRequest)asynchronousResult.AsyncState;
                var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
                Stream streamResponse = response.GetResponseStream();
                var streamRead = new StreamReader(streamResponse);
                string responseString = streamRead.ReadToEnd();
                streamResponse.Close();
                streamRead.Close();
                response.Close();
                if (OAuthEndAction != null)
                {
                    OAuthEndAction(responseString, OAuthCallback);
                }
            }
            catch (Exception e)
            {
                if (OAuthEndAction != null)
                {
                    OAuthEndAction("", OAuthCallback);
                }
            }
        }

        /// <summary>
        /// 获取POST流，并发送数据，调用回调
        /// </summary>
        /// <param name="asynchronousResult"></param>
        public void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                var request = (HttpWebRequest)asynchronousResult.AsyncState;
                Stream stream = request.EndGetRequestStream(asynchronousResult);
                string queryString = Parameters.BuildQueryString(true);
                byte[] data = Charset.GetBytes(queryString);
                stream.Write(data, 0, data.Length);
                stream.Close();
                request.BeginGetResponse(AsyncResponseCallback, request);
            }
            catch (Exception ex)
            {
                if (OAuthEndAction != null)
                {
                    OAuthEndAction("", OAuthCallback);
                }
            }
        }

        /// <summary>
        /// 获取POSTFILE流，并发送数据，调用回调
        /// </summary>
        /// <param name="asynchronousResult"></param>
        public void GetPostFileRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream stream = request.EndGetRequestStream(asynchronousResult);

            #region 生成流

            Files files = Files;

            string boundary = string.Concat("-----------------------------", Util.GenerateRndNonce());
            request.ContentType = string.Concat("multipart/form-data; boundary=", boundary);

            using (var ms = new MemoryStream())
            {
                byte[] boundaryData = Charset.GetBytes("\r\n--" + boundary + "\r\n");
                if (Parameters != null && Parameters.Items.Count != 0)
                {
                    //写入参数
                    const string parameterData = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                    foreach (var p in Parameters.Items)
                    {
                        string item = string.Format(parameterData, p.Key, p.Value);
                        byte[] data = Charset.GetBytes(item);
                        ms.Write(boundaryData, 0, boundaryData.Length);
                        ms.Write(data, 0, data.Length);
                    }
                }

                if (files != null)
                {
                    //写入文件数据
                    const string fileData =
                        "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                    foreach (var p in files.Items)
                    {
                        if (p.Value != null)
                        {
                            string item = string.Format(fileData, p.Key, p.Value.FileName, p.Value.ContentType);
                            byte[] data = Charset.GetBytes(item);
                            ms.Write(boundaryData, 0, boundaryData.Length);
                            ms.Write(data, 0, data.Length);

                            p.Value.WriteTo(ms);
                        }
                    }
                }

                //写入结束线
                boundaryData = Charset.GetBytes("\r\n--" + boundary + "--\r\n");
                ms.Write(boundaryData, 0, boundaryData.Length);
                ms.WriteTo(stream);
                stream.Close();
            }

            #endregion

            request.BeginGetResponse(AsyncResponseCallback, request);
        }
    }
}