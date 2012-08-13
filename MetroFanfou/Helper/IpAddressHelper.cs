/////////////////////////////////////////////////////////
// Author   : huangzhiming
// Date     : 11/23/2011 4:43:30 PM
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Text;
using System.Text.RegularExpressions;
using FanFou.SDK.Http;

namespace MetroFanfou.Helper
{
    /// <summary>
    /// IP地址帮助类
    /// </summary>
    public class IpAddressHelper
    {
        private const string IpaddressApi = "http://fw.qq.com/ipaddress";

        /// <summary>
        ///  获取IP地址，并回调
        /// </summary>
        /// <param name="callback"></param>
        public void GetIpAddress(Action<string> callback)
        {
            var request = new AsyncHttpRequest(IpaddressApi, Encoding.UTF8);
            request.Get(EndGetResponseData, callback);
        }

        private void EndGetResponseData(string rs, Action<string> callback)
        {
            if (!string.IsNullOrEmpty(rs) && callback != null)
            {
                callback(ExtractIpaddress(rs));
            }
        }

        /// <summary>
        /// 提取IP
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ExtractIpaddress(string input)
        {
            var reg = new Regex(@"(?<First>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Second>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Third>2[0-4]\d|25[0-5]|[01]?\d\d?)\.(?<Fourth>2[0-4]\d|25[0-5]|[01]?\d\d?)");
            var rs = reg.Match(input);
            return rs.Success ? rs.Value : "";
        }
    }
}