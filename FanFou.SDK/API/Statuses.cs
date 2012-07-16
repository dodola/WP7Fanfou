using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FanFou.SDK.Objects;

namespace FanFou.SDK.API
{
    public class Statuses : RequestBase
    {
        public Statuses(OAuth oauth)
            : base(oauth)
        {
        }

        public Action<Status> DestroyStatusCallBack { get; set; }
        /// <summary>
        /// 删除指定的消息
        /// </summary>
        /// <param name="callback"></param>
        public void DestroyStatus(Action<Status> callback, string id, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            DestroyStatusCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            PostData("http://api.fanfou.com/statuses/destroy.json", parameters, DestroyStatusEnd);
        }

        private void DestroyStatusEnd(string json)
        {
            if (DestroyStatusCallBack != null)
            {
                var result = Util.JsonToObject<Status>(json);
                DestroyStatusCallBack(result);
            }
        }


        public Action<ICollection<Status>> GetHomeTimeLineCallBack { get; set; }
        /// <summary>
        /// 显示指定用户及其好友的消息(未设置隐私用户和登录用户好友的消息)
        /// </summary>
        /// <param name="callback"></param>
        public void GetHomeTimeLine(Action<ICollection<Status>> callback, string id = null, string since_id = null, string max_id = null, int? count = null, int? page = null, string mode = null, string format = null)
        {

            GetHomeTimeLineCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(id)) parameters.Add("id", id);
            if (!string.IsNullOrEmpty(since_id)) parameters.Add("since_id", since_id);
            if (!string.IsNullOrEmpty(max_id)) parameters.Add("max_id", max_id);
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/statuses/home_timeline.json", parameters, GetHomeTimeLineEnd);
        }

        private void GetHomeTimeLineEnd(string json)
        {
            if (GetHomeTimeLineCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<Status>>(json);
                GetHomeTimeLineCallBack(result);
            }
        }



        public Action<ICollection<Status>> GetPublicTimelineCallBack { get; set; }
        /// <summary>
        /// 显示20条随便看看的消息(未设置隐私用户的消息)
        /// </summary>
        /// <param name="callback"></param>
        public void GetPublicTimeline(Action<ICollection<Status>> callback, int? count = null, string since_id = null, string max_id = null, string mode = null, string format = null)
        {

            GetPublicTimelineCallBack = callback;
            var parameters = new Parameters();
            if (count != null) parameters.Add("count", count);
            if (!string.IsNullOrEmpty(since_id)) parameters.Add("since_id", since_id);
            if (!string.IsNullOrEmpty(max_id)) parameters.Add("max_id", max_id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/statuses/public_timeline.json", parameters, GetPublicTimelineEnd);
        }

        private void GetPublicTimelineEnd(string json)
        {
            if (GetPublicTimelineCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<Status>>(json);
                GetPublicTimelineCallBack(result);
            }
        }

        public Action<ICollection<Status>> GetRepliesCallBack { get; set; }

        /// <summary>
        /// 显示回复当前用户的20条消息(未设置隐私用户和登录用户好友的消息)
        /// </summary>
        /// <param name="callback"></param>
        public void GetReplies(Action<ICollection<Status>> callback, string since_id = null, string max_id = null, int? count = null, int? page = null, string mode = null, string format = null)
        {

            GetRepliesCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(since_id)) parameters.Add("since_id", since_id);
            if (!string.IsNullOrEmpty(max_id)) parameters.Add("max_id", max_id);
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/statuses/replies.json", parameters, GetRepliesEnd);
        }

        private void GetRepliesEnd(string json)
        {
            if (GetRepliesCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<Status>>(json);
                GetRepliesCallBack(result);
            }
        }


        public Action<ICollection<User>> GetFollowersCallBack { get; set; }
        /// <summary>
        /// 返回用户的前100个关注者
        /// </summary>
        /// <param name="callback"></param>
        public void GetFollowers(Action<ICollection<User>> callback, string id = null, int? count = null, int? page = null, string format = null, string mode = null)
        {

            GetFollowersCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(id)) parameters.Add("id", id);
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            GetData("http://api.fanfou.com/statuses/followers.json", parameters, GetFollowersEnd);
        }

        private void GetFollowersEnd(string json)
        {
            if (GetFollowersCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<User>>(json);
                GetFollowersCallBack(result);
            }
        }


        public Action<Status> UpdateStatusCallBack { get; set; }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="status"></param>
        /// <param name="in_reply_to_status_id"></param>
        /// <param name="in_reply_to_user_id"></param>
        /// <param name="repost_status_id"></param>
        /// <param name="source"></param>
        /// <param name="mode"></param>
        /// <param name="format"></param>
        /// <param name="location"></param>
        public void UpdateStatus(Action<Status> callback, string status, string in_reply_to_status_id = null, string in_reply_to_user_id = null, string repost_status_id = null, string source = null, string mode = null, string format = null, string location = null)
        {
            if (string.IsNullOrEmpty(status)) throw new ArgumentException("status不能为空");
            UpdateStatusCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("status", status);
            if (!string.IsNullOrEmpty(in_reply_to_status_id)) parameters.Add("in_reply_to_status_id", in_reply_to_status_id);
            if (!string.IsNullOrEmpty(in_reply_to_user_id)) parameters.Add("in_reply_to_user_id", in_reply_to_user_id);
            if (!string.IsNullOrEmpty(repost_status_id)) parameters.Add("repost_status_id", repost_status_id);
            if (!string.IsNullOrEmpty(source)) parameters.Add("source", source);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            if (!string.IsNullOrEmpty(location)) parameters.Add("location", location);

            PostData("http://api.fanfou.com/statuses/update.json", parameters, UpdateStatusEnd);
        }

        private void UpdateStatusEnd(string json)
        {
            if (UpdateStatusCallBack != null)
            {

                var result = Util.JsonToObject<Status>(json);
                UpdateStatusCallBack(result);
            }
        }



        public Action<ICollection<Status>> GetUserTimelineCallBack { get; set; }
        /// <summary>
        /// 浏览指定用户已发送消息
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="id"></param>
        /// <param name="since_id"></param>
        /// <param name="max_id"></param>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <param name="mode"></param>
        /// <param name="format"></param>
        public void GetUserTimeline(Action<ICollection<Status>> callback, string id = null, string since_id = null,
                                    string max_id = null, int? count = null, int? page = null, string mode = null,
                                    string format = null)
        {

            GetUserTimelineCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(id)) parameters.Add("id", id);
            if (!string.IsNullOrEmpty(since_id)) parameters.Add("since_id", since_id);
            if (!string.IsNullOrEmpty(max_id)) parameters.Add("max_id", max_id);
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/statuses/user_timeline.json", parameters, GetUserTimelineEnd);
        }

        private void GetUserTimelineEnd(string json)
        {
            if (GetUserTimelineCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<Status>>(json);
                GetUserTimelineCallBack(result);
            }
        }

        public Action<ICollection<User>> GetFriendsCallBack { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <param name="mode"></param>
        public void GetFriends(Action<ICollection<User>> callback, string id = null, int? count = null, int? page = null,
                               string mode = null)
        {

            GetFriendsCallBack = callback;

            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(id)) parameters.Add("id", id);
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            GetData("http://api.fanfou.com/statuses/friends.json", parameters, GetFriendsEnd);
        }

        private void GetFriendsEnd(string json)
        {
            if (GetFriendsCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<User>>(json);
                GetFriendsCallBack(result);
            }
        }


        public Action<ICollection<Status>> GetContextTimelineCallBack { get; set; }
        /// <summary>
        /// 按照时间先后顺序显示消息上下文(好友和未设置隐私用户的消息)
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        /// <param name="format"></param>
        public void GetContextTimeline(Action<ICollection<Status>> callback, string id, string mode = null,
                                       string format = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            GetContextTimelineCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/statuses/context_timeline.json", parameters, GetContextTimelineEnd);
        }

        private void GetContextTimelineEnd(string json)
        {
            if (GetContextTimelineCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<Status>>(json);
                GetContextTimelineCallBack(result);
            }
        }


        public Action<ICollection<Status>> GetMetionsCallBack { get; set; }
        /// <summary>
        /// 显示回复/提到当前用户的20条消息(未设置隐私用户和登录用户好友的消息)
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="since_id"></param>
        /// <param name="max_id"></param>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <param name="mode"></param>
        /// <param name="format"></param>
        public void GetMetions(Action<ICollection<Status>> callback, string since_id = null,
                                    string max_id = null, int? count = null, int? page = null, string mode = null,
                                    string format = null)
        {

            GetMetionsCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(since_id)) parameters.Add("since_id", since_id);
            if (!string.IsNullOrEmpty(max_id)) parameters.Add("max_id", max_id);
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/statuses/mentions.json", parameters, GetMetionsEnd);
        }

        private void GetMetionsEnd(string json)
        {
            if (GetMetionsCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<Status>>(json);
                GetMetionsCallBack(result);
            }
        }


        public Action<Status> ShowStatusCallBack { get; set; }

        public void ShowStatus(Action<Status> callback, string id, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            ShowStatusCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/statuses/show.json", parameters, ShowStatusEnd);
        }

        private void ShowStatusEnd(string json)
        {
            if (ShowStatusCallBack != null)
            {

                var result = Util.JsonToObject<Status>(json);
                ShowStatusCallBack(result);
            }
        }
    }
}
