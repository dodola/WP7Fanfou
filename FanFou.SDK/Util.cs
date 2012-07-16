using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using FanFou.SDK.Objects;

namespace FanFou.SDK
{
    /// <summary>
    /// 实用类
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 用于计算时间戳的时间值
        /// </summary>
        private static DateTime UnixTimestamp = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 生成一个时间戳
        /// </summary>
        /// <returns></returns>
        public static long GenerateTimestamp()
        {
            return GenerateTimestamp(DateTime.Now);
        }
        /// <summary>
        /// 生成一个时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long GenerateTimestamp(DateTime time)
        {
            return (long)(time.ToUniversalTime() - UnixTimestamp).TotalSeconds;
        }
        /// <summary>
        /// 将时间戳转换为人性化的时间
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static string ConvertFromTimestamp(long timestamp)
        {
            var timeSince = DateTime.Now.Subtract(DateTime.Parse("1970-01-01 00:00:00").AddSeconds(timestamp + 8 * 60 * 60));
            if (timeSince.TotalMinutes < 1) return "刚刚";
            if (timeSince.TotalMinutes < 2) return "1 分钟前";
            if (timeSince.TotalMinutes < 60) return string.Format("{0} 分钟前", timeSince.Minutes);
            if (timeSince.TotalMinutes < 120) return "1 小时前";
            if (timeSince.TotalHours < 24) return string.Format("{0} 小时前", timeSince.Hours);
            if (timeSince.TotalDays == 1) return "昨天";
            if (timeSince.TotalDays < 7) return string.Format("{0} 天前", timeSince.Days);
            if (timeSince.TotalDays < 14) return "上周";
            if (timeSince.TotalDays < 21) return "2 周前";
            if (timeSince.TotalDays < 28) return "3 周前";
            if (timeSince.TotalDays < 60) return "上个月";
            if (timeSince.TotalDays < 365) return string.Format("{0} 个月前", Math.Round(timeSince.TotalDays / 30));
            if (timeSince.TotalDays < 730) return "去年";
            return string.Format("{0} 年前", Math.Round(timeSince.TotalDays / 365));
        }

        /// <summary>
        /// 随机种子
        /// </summary>
        private static Random RndSeed = new Random();
        /// <summary>
        /// 生成一个随机码
        /// </summary>
        /// <returns></returns>
        public static string GenerateRndNonce()
        {
            return string.Concat(
            Util.RndSeed.Next(1, 99999999).ToString("00000000"),
            Util.RndSeed.Next(1, 99999999).ToString("00000000"),
            Util.RndSeed.Next(1, 99999999).ToString("00000000"),
            Util.RndSeed.Next(1, 99999999).ToString("00000000"));
        }

        /// <summary>
        /// 连接字符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="separator">分隔符</param>
        /// <param name="values">值列表</param>
        /// <returns></returns>
        public static string Join<T>(string separator, IEnumerable<T> values)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (T t in values)
            {
                if (buffer.Length != 0) buffer.Append(separator);
                buffer.Append(t == null ? "" : t.ToString());
            }
            return buffer.ToString();
        }
        /// <summary>
        /// UrlEncode
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UrlEncode(string text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            StringBuilder buffer = new StringBuilder(text.Length);
            byte[] data = Encoding.UTF8.GetBytes(text);
            foreach (byte b in data)
            {
                char c = (char)b;
                if (!(('0'<= c && c <= '9') || ('a'<= c && c <= 'z') || ('A'<= c && c <= 'Z'))
                    && "-_.~".IndexOf(c) == -1)
                {
                    buffer.Append('%' + Convert.ToString(c, 16).ToUpper());
                }
                else
                {
                    buffer.Append(c);
                }
            }
            return buffer.ToString();
        }
        /// <summary>
        /// 32位MD5加密字符数据
        /// </summary>
        /// <param name="value">要加密的字符数据</param>
        /// <returns></returns>
        public static string MD5(string value)
        {
            return MD5(value, Encoding.UTF8);
        }
        /// <summary>
        /// MD5加密字符
        /// </summary>
        /// <param name="value">要加密的字符数据</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string MD5(string value, Encoding encoding)
        {
            if (string.IsNullOrEmpty(value)) return "";

            var md5 = new MD5CryptoServiceProvider();

            byte[] output = md5.ComputeHash(encoding.GetBytes(value));

            md5.Clear();

            var code = new StringBuilder();
            for (int i = 0; i < output.Length; i++)
            {
                code.Append(output[i].ToString("x2"));
            }
            return code.ToString();
        }


        public static NameValuePairCollection GetQueryParameters(string parameters)
        {
            if (parameters.StartsWith("?"))
            {
                parameters = parameters.Remove(0, 1);
            }

            var result = new NameValuePairCollection();

            if (!string.IsNullOrEmpty(parameters))
            {
                string[] p = parameters.Split('&');
                foreach (string s in p)
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        if (s.IndexOf('=') > -1)
                        {
                            string[] temp = s.Split('=');
                            result.Add(temp[0], temp[1]);
                        }
                        else
                        {
                            result.Add(s, string.Empty);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(string json)
        {
            if (!string.IsNullOrEmpty(json))
            {
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
                var ser = new DataContractJsonSerializer(typeof (T));
                return (T) ser.ReadObject(ms);
            }
            return default(T);
        }
    }
}
