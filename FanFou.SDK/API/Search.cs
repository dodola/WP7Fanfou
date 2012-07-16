using System;
using System.Collections.Generic;
using FanFou.SDK.Objects;
using FanFou.SDK.Objects.Search;

namespace FanFou.SDK.API
{
    public class Search : RequestBase
    {
        public enum MyEnum
        {

        }
        public Search(OAuth oauth)
            : base(oauth)
        {

        }

        public Action<ICollection<Status>> SearchPublicTimeCallBack { get; set; }

        public Action<SearchUsers> SearchUsersCallBack { get; set; }

        public Action<ICollection<Status>> SearchUserTimeLineCallBack { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryWord"></param>
        /// <param name="callback"></param>
        /// <param name="sinceId"></param>
        /// <param name="maxId"></param>
        /// <param name="count"></param>
        /// <param name="mode"></param>
        /// <param name="format"></param>
        public void SearchPublicTime(Action<ICollection<Status>> callback, string queryWord, string sinceId = null, string maxId = null, int? count = null, string mode = null, string format = null)
        {

            if (String.IsNullOrEmpty(queryWord))
                throw new ArgumentException("搜索关键字不能为空");

            this.SearchPublicTimeCallBack = callback;
            var paramters = new Parameters();
            paramters.Add("q", queryWord);
            if (!string.IsNullOrEmpty(sinceId)) paramters.Add("since_id", sinceId);
            if (!string.IsNullOrEmpty(maxId)) paramters.Add("max_id", maxId);
            if (count != null) paramters.Add("count", count);
            if (!string.IsNullOrEmpty(mode)) paramters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) paramters.Add("format", format);
            GetData("http://api.fanfou.com/search/public_timeline.json", paramters, SearchPublicTimeEnd);
        }

        private void SearchPublicTimeEnd(string json)
        {
            if (SearchPublicTimeCallBack != null)
            {
                var obj = Util.JsonToObject<ICollection<Status>>(json);

                SearchPublicTimeCallBack(obj);
            }
        }

        // public Action<ICollection<Status>> UserTimeLineCallBack { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="q"></param>
        /// <param name="callback"></param>
        /// <param name="id"></param>
        /// <param name="sinceId"></param>
        /// <param name="maxId"></param>
        /// <param name="count"></param>
        /// <param name="mode"></param>
        /// <param name="format"></param>
        public void SearchUserTimeLine(Action<ICollection<Status>> callback, string q, string id = null, string sinceId = null, string maxId = null, int? count = null, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(q))
                throw new ArgumentException("搜索关键字不能为空");
            this.SearchUserTimeLineCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("q", q);
            if (!string.IsNullOrEmpty(id)) parameters.Add("id", id);
            if (!string.IsNullOrEmpty(sinceId)) parameters.Add("since_id", sinceId);
            if (count != null) parameters.Add("count", count);
            if (!string.IsNullOrEmpty(maxId)) parameters.Add("max_id", maxId);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/search/user_timeline.json", parameters, SearchUserTimeLineEnd);



        }
        private void SearchUserTimeLineEnd(string json)
        {
            if (SearchUserTimeLineCallBack != null)
            {
                var result = Util.JsonToObject<ICollection<Status>>(json);
                SearchUserTimeLineCallBack(result);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="q"></param>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <param name="mode"></param>
        /// <param name="format"></param>
        public void SearchUsers(Action<SearchUsers> callback, string q, int? count = null, int? page = null, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(q))
            {
                throw new ArgumentException("搜索关键字不能为空");

            }
            this.SearchUsersCallBack = callback;
            var paramters = new Parameters();
            paramters.Add("q", q);
            if (count != null) paramters.Add("count", count);
            if (page != null) paramters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) paramters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) paramters.Add("format", format);
            GetData("http://api.fanfou.com/search/users.json", paramters, SearchUserEnd);

        }
        private void SearchUserEnd(string json)
        {
            if (SearchUsersCallBack != null)
            {

                var result = Util.JsonToObject<SearchUsers>(json);
                SearchUsersCallBack(result);
            }
        }

    }
}