using System;
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
    public class Account : RequestBase
    {
        public Account(OAuth oauth)
            : base(oauth)
        {
        }


        public Action<User> UpdateProfileImageCallBack { get; set; }
        /// <summary>
        /// 通过 API 更新用户头像
        /// </summary>
        /// <param name="callback"></param>
        public void UpdateProfileImage(Action<User> callback)
        {
            //TODO:未完成
            UpdateProfileImageCallBack = callback;
            var parameters = new Parameters();

            PostData("http://api.fanfou.com/account/update_profile_image.json", parameters, UpdateProfileImageEnd);
        }

        private void UpdateProfileImageEnd(string json)
        {
            if (UpdateProfileImageCallBack != null)
            {
                var result = Util.JsonToObject<User>(json);
                UpdateProfileImageCallBack(result);
            }
        }

        public Action<User> UpdateProfileCallBack { get; set; }
        /// <summary>
        /// 通过 API 更新用户资料
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="url"></param>
        /// <param name="mode"></param>
        /// <param name="location"></param>
        /// <param name="description"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        public void UpdateProfile(Action<User> callback, string url = null, string mode = null, string location = null, string description = null, string name = null, string email = null)
        {

            UpdateProfileCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(url)) parameters.Add("url", url);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(location)) parameters.Add("location", location);
            if (!string.IsNullOrEmpty(description)) parameters.Add("description", description);
            if (!string.IsNullOrEmpty(name)) parameters.Add("name", name);
            if (!string.IsNullOrEmpty(email)) parameters.Add("email", email);
            GetData("http://api.fanfou.com/account/update_profile.json", parameters, UpdateProfileEnd);
        }

        private void UpdateProfileEnd(string json)
        {
            if (UpdateProfileCallBack != null)
            {

                var result = Util.JsonToObject<User>(json);
                UpdateProfileCallBack(result);
            }
        }

        public Action<Notification> NotificationCallBack { get; set; }
        /// <summary>
        /// 返回未读的mentions, direct message 以及关注请求数量
        /// </summary>
        /// <param name="callback"></param>
        public void GetNotification(Action<Notification> callback)
        {

            NotificationCallBack = callback;
            GetData("http://api.fanfou.com/account/notification.json", null, NotificationEnd);
        }

        private void NotificationEnd(string json)
        {
            if (NotificationCallBack != null)
            {

                var result = Util.JsonToObject<Notification>(json);
                NotificationCallBack(result);
            }
        }

    }
}
