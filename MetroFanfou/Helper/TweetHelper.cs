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
        public const string AccountReg = @"@([A-Za-z0-9-_]*)?";
        /// <summary>
        /// 话题正则
        /// </summary>
        public const string HuatiReg = @"\#([^\#|.]+)\#";
        /// <summary>
        /// Url正则
        /// </summary>
        public const string UrlReg = @"http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?";

        /// <summary>
        /// 匹配表情
        /// </summary>
        public const string FaceReg = @"/(no|ok|乒乓|亲亲|便便|偷笑|傲慢|再见|冷汗|凋谢|刀|勾引|发呆|发怒|发抖|可怜|可爱|右哼哼|右太极|吐|吓|咒骂|咖啡|哈欠|啤酒|嘘|回头|困|坏笑|大兵|大哭|太阳|奋斗|委屈|害羞|尴尬|左哼哼|左太极|差劲|弱|强|得意|微笑|心碎|快哭了|怄火|惊恐|惊讶|憨笑|抓狂|折磨|抠鼻|抱拳|拥抱|拳头|挥手|握手|撇嘴|擦汗|敲打|晕|月亮|流汗|流泪|激动|炸弹|爱你|爱心|爱情|猪头|献吻|玫瑰|瓢虫|疑问|白眼|睡|磕头|示爱|礼物|篮球|糗大了|胜利|色|菜刀|蛋糕|街舞|衰|西瓜|调皮|足球|跳绳|跳跳|转圈|鄙视|酷|闪电|闭嘴|阴险|难过|飞吻|饥饿|饭|骷髅|鼓掌|呲牙)";

        #endregion

        #region 私有方法

        private static List<string> GetMatchs(string content,string reg)
        {
            var rs=new List<string>();

            if(!string.IsNullOrEmpty(content))
            {
                var regex = new Regex(reg);
                var matches = regex.Matches(content);
                foreach (Match m in matches)
                {
                    if(!string.IsNullOrWhiteSpace(m.Value)){
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
        public List<string> MatchString(string content, string reg) {
            return GetMatchs(content, reg);
        }

        /// <summary>
        /// 匹配的帐号
        /// </summary>
        public ICollection<string> Accounts
        {
            get; set;
        }

        /// <summary>
        /// 匹配的话题
        /// </summary>
        public ICollection<string> HuaTi
        {
            get;set;
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
