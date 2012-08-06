/////////////////////////////////////////////////////////
// Author   : huangzhiming
// Date     : 11/28/2011 9:55:38 AM
// Usage    :
/////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MetroFanfou.Helper
{
    /// <summary>
    /// 微博内容的帮助类，获取话题，帐号等
    /// </summary>
    public class TweetHelper
    {
        #region 私有变量

        /// <summary>
        /// 帐号正则
        /// </summary>
        public const string AccountReg = @"@(.*?\s+)?";
        /// <summary>
        /// 话题正则
        /// </summary>
        public const string HuatiReg = @"\#([^\#|.]+)\#";
        /// <summary>
        /// Url正则
        /// </summary>
        public const string UrlReg = @"http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?";

        #endregion

        #region 私有方法

        private static List<string> GetMatchs(string content, string reg)
        {
            var rs = new List<string>();

            if (!string.IsNullOrEmpty(content))
            {
                var regex = new Regex(reg);
                var matches = regex.Matches(content);
                foreach (Match m in matches)
                {
                    if (!string.IsNullOrWhiteSpace(m.Value))
                    {
                        rs.Add(m.Value);
                    }
                }
            }

            return rs;
        }

        #endregion

        public TweetHelper() { }

        public TweetHelper(string content)
        {
            Accounts = GetMatchs(content, AccountReg);
            HuaTi = GetMatchs(content, HuatiReg);
            Urls = GetMatchs(content, UrlReg);
        }
        /// <summary>
        /// 匹配的字符串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="reg"></param>
        /// <returns></returns>
        public List<string> MatchString(string content, string reg)
        {
            return GetMatchs(content, reg);
        }

        /// <summary>
        /// 匹配的帐号
        /// </summary>
        public ICollection<string> Accounts
        {
            get;
            set;
        }

        /// <summary>
        /// 匹配的话题
        /// </summary>
        public ICollection<string> HuaTi
        {
            get;
            set;
        }

        /// <summary>
        /// 匹配的Url
        /// </summary>
        public ICollection<string> Urls
        {
            get;
            set;
        }
    }
}
