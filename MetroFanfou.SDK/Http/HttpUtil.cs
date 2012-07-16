using System.Net;

namespace MetroFanfou.SDK.Http
{
    /// <summary>
    /// 
    /// </summary>
    static class HttpUtil
    {
        /// <summary>
        /// 建立请求
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static HttpWebRequest CreateRequest(string method, string url, int timeout)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AllowReadStreamBuffering = false;
            request.Method = method;
            return request;
        }
    }
}
