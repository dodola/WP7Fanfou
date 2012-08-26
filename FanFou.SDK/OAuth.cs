using System;
using System.Text;
using System.Security.Cryptography;
using FanFou.SDK.Http;
using FanFou.SDK.Objects;

namespace FanFou.SDK
{
    /// <summary>
    /// OAuth授权对象
    /// </summary>
    public class OAuth
    {
        /// <summary>
        /// 根据app_key与app_secret实例化
        /// </summary>
        public OAuth(string appKey, string appSecret)
        {
            this.AppKey = appKey;
            this.AppSecret = appSecret;
            this.Charset = Encoding.UTF8;
        }
        /// <summary>
        /// app_key
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// app_secret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// oauth_token, 根据不同的场合使用不同的值,如request_token或access_token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// oauth_token_secret, 根据不同的场合使用不同的值,如request_secret或access_secret
        /// </summary>
        public string TokenSecret { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Charset { get; set; }

        /// <summary>
        /// 授权过程中最后一次发生的错误
        /// </summary>
        public Exception LastError { get; private set; }

        /// <summary>
        /// 使用默认的API地址同步获取未授权的Request Token. 
        /// 如果获取成功, 对象实例的Token与TokenSecret属性值将改为request_token与request_secret
        /// </summary>
        /// <param name="callback">返回地址</param>
        /// <returns>是否获取request token成功</returns>
        public void GetRequestToken(Action<string> callback)
        {
            this.GetRequestToken("http://fanfou.com/oauth/request_token", callback);
        }
        /// <summary>
        /// 获取未授权的Request Token. 
        /// 如果获取成功, 对象实例的Token与TokenSecret属性值将改为request_token与request_secret
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="callback">返回函数</param>
        /// <returns>是否获取request token成功</returns>
        public void GetRequestToken(string requestUrl, Action<string> callback)
        {
            this.Token = string.Empty;
            this.TokenSecret = string.Empty;
            var parameters = new Parameters();
            parameters.Add("oauth_callback", "null");
            this.GetOAuthToken(requestUrl, parameters, EndRequestToken, callback);
        }

        /// <summary>
        /// 获取未授权的Request Token结束
        /// </summary>
        /// <param name="response"></param>
        /// <param name="callback"></param>
        private void EndRequestToken(string response, Action<string> callback)
        {
            if (!string.IsNullOrEmpty(response))
            {
                var responseData = Util.GetQueryParameters(response);
                this.Token = responseData["oauth_token"];
                this.TokenSecret = responseData["oauth_token_secret"];
                callback(this.Token);
            }
            else
            {
                if (callback != null)
                {
                    callback("");
                }
            }
        }

        /// <summary>
        /// 使用默认的API地址同步获取授权的Access Token，调用此方法时必须设置Token与TokenSecret属性的值为request_token与request_secret
        /// 如果获取成功, 对象实例的Token与TokenSecret属性值将改为access_token与access_secret
        /// </summary>
        /// <param name="verifier">请求授权request token时返回的验证码</param>
        /// <param name="callback">回调，参数是微博帐户名</param>
        /// <returns>是否获取Access Token成功</returns>
        public void GetAccessToken(string verifier, Action<string> callback)
        {
            this.GetAccessToken("http://fanfou.com/oauth/access_token", verifier, callback);
        }
        /// <summary>
        /// 同步获取授权的Access Token，调用此方法时必须设置Token与TokenSecret属性的值为request_token与request_secret
        /// 如果获取成功, 对象实例的Token与TokenSecret属性值将改为access_token与access_secret
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="verifier">请求授权request token时返回的验证码</param> 
        /// <param name="callback">回调，参数是微博帐户名</param>
        /// <returns>是否获取Access Token成功</returns>
        public void GetAccessToken(string requestUrl, string verifier, Action<string> callback)
        {
            var parameters = new Parameters();
            parameters.Add("oauth_token", this.Token);
            parameters.Add("oauth_verifier", verifier);
            this.GetOAuthToken(requestUrl, parameters, EndAccessToken, callback);
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


        /// <summary>
        /// 获取授权Token
        /// </summary>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="parameters"></param>
        /// <param name="responseEnd"></param>
        /// <returns></returns>
        private void GetOAuthToken(string requestUrl, Parameters parameters, Action<string, Action<string>> responseEnd, Action<string> callback)
        {
            this.LastError = null;
            parameters.Add("oauth_consumer_key", this.AppKey);
            parameters.Add("oauth_signature_method", "HMAC-SHA1");
            parameters.Add("oauth_timestamp", Util.GenerateTimestamp());
            parameters.Add("oauth_nonce", Util.GenerateRndNonce());
            parameters.Add("oauth_version", "1.0");
            parameters.Add("oauth_signature", this.GenerateSignature("GET", requestUrl, parameters));
            string url = string.Concat(requestUrl, "?", parameters.BuildQueryString(true));

            var request = new AsyncHttpRequest(url, this.Charset);
            try
            {
                request.Get(responseEnd, callback);
            }
            catch (Exception ex)
            {
                this.LastError = ex;
            }
        }

        /// <summary>
        /// 对数据进行签名
        /// </summary>
        /// <param name="requestMethod">请求方法.如GET或POST</param>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="parameters">提交参数</param>
        /// <returns></returns>
        public string GenerateSignature(string requestMethod, string requestUrl, Parameters parameters)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.UTF8.GetBytes(string.Format("{0}&{1}", Util.UrlEncode(this.AppSecret), Util.UrlEncode(this.TokenSecret)));

            StringBuilder data = new StringBuilder(100);
            data.AppendFormat("{0}&{1}&", requestMethod.ToUpper(), Util.UrlEncode(requestUrl));
            //处理参数
            if (parameters != null)
            {
                parameters.Sort();
                data.Append(Util.UrlEncode(parameters.BuildQueryString(true)));
            }


            byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(data.ToString());
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
