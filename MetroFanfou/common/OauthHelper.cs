using System;
using FanFou.SDK;

namespace MetroFanfou.common
{
    /// <summary>
    /// 授权验证帮助类
    /// </summary>
    public class OauthHelper
    {
        #region 私有属性

        /// <summary>
        /// App Key
        /// </summary>
        private const string AppKey = "2b6f6ad1b48d1f001c1e5a63c4428571";//输入你的AppKey
        /// <summary>
        /// App Secret
        /// </summary>
        private const string AppSecret = "c4de15dc883746811beffa8444383779";//输入你的AppSecret

        /// <summary>
        /// Token
        /// </summary>
        private static string OAuthToken { get; set; }

        /// <summary>
        /// TokenSecret
        /// </summary>
        private static string OAuthTokenSecret { get; set; }

        /// <summary>
        /// Token的存储KEY
        /// </summary>
        private const string TokenStoreKey = "OAuthHelper.Flags.Token";

        /// <summary>
        /// TokenSecret的存储KEY
        /// </summary>
        private const string TokenSecretStoreKey = "OAuthHelper.Flags.TokenSecret";

        #endregion

        #region 公共属性

        /// <summary>
        /// 判断是否授权
        /// </summary>
        public static bool IsVerified
        {
            get
            {
                var val = Isolated.Get("OAuthHelper.Flags.isVerified");
                if (val != null)
                {
                    return (bool)val;
                }
                return false;
            }
            set
            {
                Isolated.Set("OAuthHelper.Flags.isVerified", value);
            }
        }

        /// <summary>
        /// oauth_token, 根据不同的场合使用不同的值,如request_token或access_token
        /// </summary>
        public static string Token
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(OAuthToken))
                {
                    return OAuthToken;
                }
                var val = Isolated.Get(TokenStoreKey);
                if (val != null)
                {
                    OAuthToken = (string)val;
                    return OAuthToken;
                }
                return string.Empty;
            }
            set
            {
                OAuthToken = value;
                Isolated.Set(TokenStoreKey, value);
            }
        }

        /// <summary>
        /// oauth_token_secret, 根据不同的场合使用不同的值,如request_secret或access_secret
        /// </summary>
        public static string TokenSecret
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(OAuthTokenSecret))
                {
                    return OAuthTokenSecret;
                }
                var val = Isolated.Get(TokenSecretStoreKey);
                if (val != null)
                {
                    OAuthTokenSecret = (string)val;
                    return OAuthTokenSecret;
                }
                return string.Empty;
            }
            set
            {
                OAuthTokenSecret = value;
                Isolated.Set(TokenSecretStoreKey, value);
            }
        }

        #endregion

        #region 公共方法
        /// <summary>
        /// 获取OAuth对象
        /// </summary>
        /// <returns></returns>
        public static OAuth OAuth()
        {
            if (!string.IsNullOrWhiteSpace(AppKey) && !string.IsNullOrWhiteSpace(AppSecret))
            {
                var oauth = new OAuth(AppKey, AppSecret) { Token = Token, TokenSecret = TokenSecret };
                return oauth;
            }
            else
            {
                throw new Exception("AppKey和AppSecret不能为空！");
            }
        }

        /// <summary>
        /// 注销帐号
        /// </summary>
        public static void Logout()
        {
            Token = string.Empty;
            TokenSecret = string.Empty;
        }

        #endregion
    }
}
