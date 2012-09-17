using System;
using System.Text;
using FanFou.SDK.Http;
using FanFou.SDK.Objects;

namespace FanFou.SDK.API
{
    /// <summary>
    /// 接口请求的基类
    /// </summary>
    public abstract class RequestBase
    {
        /// <summary>
        /// 根据请求基本地址实例化对象
        /// </summary>
        /// <param name="oauth">OAuth授权对象</param>
        protected RequestBase(OAuth oauth)
        {
            this.OAuth = oauth;
            this.ResponseDataFormat = ResponseDataFormat.JSON;
        }

        /// <summary>
        /// 授权Key
        /// </summary>
        public OAuth OAuth { get; private set; }

        /// <summary>
        /// 操作中最后一次发生的错误
        /// </summary>
        public Exception LastError { get; private set; }


        /// <summary>
        /// GET数据
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="parameters"></param>
        /// <param name="callback"></param>
        protected virtual void GetData(string requestUrl, Parameters parameters, Action<string> callback)
        {
            this.LastError = null;
            //  this.AddOAuthParameter("GET", requestUrl, parameters);
            var request = new AsyncHttpRequest(requestUrl, this.OAuth.Charset) { Parameters = parameters };
            request.Get(EndGetResponseData, callback);
        }

        /// <summary>
        /// POST数据
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="parameters"></param>
        /// <param name="callback"></param>
        protected virtual void PostData(string requestUrl, Parameters parameters, Action<string> callback)
        {
            this.PostData(requestUrl, parameters, null, callback);
        }

        private string AuthHeaderStr = "";
        /// <summary>
        /// POST数据
        /// </summary>
        /// <param name="requestUrl"></param>
        /// <param name="parameters"></param>
        /// <param name="files"></param>
        /// <param name="callback"></param>
        protected virtual void PostData(string requestUrl, Parameters parameters, Files files, Action<string> callback)
        {
            this.LastError = null;
            this.AddOAuthParameter("POST", requestUrl, parameters);
            var request = new AsyncHttpRequest(requestUrl, this.OAuth.Charset) { Parameters = parameters, AuthHeader = AuthHeaderStr };
            if (files != null)
            {
                request.PostFile(EndGetResponseData, files, callback);
            }
            else
            {
                request.Post(EndGetResponseData, callback);
            }
        }

        /// <summary>
        /// 完成数据请求，调用回调
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="callback"></param>

        private static void EndGetResponseData(string data, Action<string> callback)
        {
            if (!string.IsNullOrEmpty(data) && callback != null)
            {
                callback(data);
            }
        }


        /// <summary>
        /// 设置或返回获取数据的格式
        /// </summary>
        protected ResponseDataFormat ResponseDataFormat { get; set; }

        /// <summary>
        /// 增加OAuth授权的参数
        /// </summary>
        /// <param name="requestMethod">请求方法.如GET或POST</param>
        /// <param name="requestUrl">API请求地址</param>
        /// <param name="parameters">提交参数</param>
        /// <returns></returns>
        //protected virtual void AddOAuthParameter(string requestMethod, string requestUrl, Parameters parameters)
        //{
        //    Parameters oParameters = new Parameters();

        //    oParameters.Add("oauth_consumer_key", this.OAuth.AppKey);
        //    oParameters.Add("oauth_token", this.OAuth.Token);
        //    oParameters.Add("oauth_signature_method", "HMAC-SHA1");
        //    oParameters.Add("oauth_timestamp", Util.GenerateTimestamp().ToString());
        //    oParameters.Add("oauth_nonce", Util.GenerateRndNonce());
        //    oParameters.Add("oauth_version", "1.0");
        //    oParameters.Add("oauth_signature", this.OAuth.GenerateSignature(requestMethod, requestUrl, parameters));
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var oitem in oParameters.Items)
        //    {
        //        parameters.Add(oitem.Key, oitem.Value);
        //        sb.AppendFormat("{0}=\"{1}\",", oitem.Key, oitem.Value);
        //    }
        //    AuthHeaderStr = sb.ToString();


        //}

        protected virtual void AddOAuthParameter(string requestMethod, string requestUrl, Parameters parameters)
        {
            Parameters oParameters = new Parameters();

            parameters.Add("oauth_consumer_key", this.OAuth.AppKey);
            parameters.Add("oauth_token", this.OAuth.Token);
            parameters.Add("oauth_signature_method", "HMAC-SHA1");
            parameters.Add("oauth_timestamp", Util.GenerateTimestamp().ToString());
            parameters.Add("oauth_nonce", Util.GenerateRndNonce());
            parameters.Add("oauth_version", "1.0");
            parameters.Add("oauth_signature", this.OAuth.GenerateSignature(requestMethod, requestUrl, parameters));



        }
    }
}
