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
    public class Users : RequestBase
    {
        public Users(OAuth oauth)
            : base(oauth)
        {
        }

        public Action<ICollection<User>> GetUsersByTaggedCallBack { get; set; }

        public void GetUsersByTagged(Action<ICollection<User>> callback, string tag, int count = 100, int? page = null, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(tag)) throw new ArgumentException("tag不能为空");
            this.GetUsersByTaggedCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("tag", tag);
            parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("", parameters, GetUsersByTaggedEnd);
        }
        private void GetUsersByTaggedEnd(string json)
        {
            if (GetUsersByTaggedCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<User>>(json);
                GetUsersByTaggedCallBack(result);
            }
        }


        public Action<User> UsersShowCallBack { get; set; }

        public void UsersShow(Action<User> callback, string id, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            UsersShowCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/users/show.json", parameters, UsersShowEnd);
        }

        private void UsersShowEnd(string json)
        {
            if (UsersShowCallBack != null)
            {

                var result = Util.JsonToObject<User>(json);
                UsersShowCallBack(result);
            }
        }


        public Action<ICollection<string>> GetUsersTagListCallBack { get; set; }

        public void GetUsersTagList(Action<ICollection<string>> callback, string id = null)
        {

            GetUsersTagListCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(id)) parameters.Add("id", id);
            GetData("http://api.fanfou.com/users/tag_list.json", parameters, GetUsersTagListEnd);
        }

        private void GetUsersTagListEnd(string json)
        {
            if (GetUsersTagListCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<string>>(json);
                GetUsersTagListCallBack(result);
            }
        }

        public Action<ICollection<User>> GetUserFollowersCallBack { get; set; }

        public void GetUserFollowers(Action<ICollection<User>> callback, string id = null, int? count = null, int? page = null, string mode = null, string format = null)
        {

            GetUserFollowersCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(id)) parameters.Add("id", id);
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);

            GetData("http://api.fanfou.com/users/followers.json", parameters, GetUserFollowersEnd);
        }

        private void GetUserFollowersEnd(string json)
        {
            if (GetUserFollowersCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<User>>(json);
                GetUserFollowersCallBack(result);
            }
        }

        public Action<ICollection<User>> GetUsersFriendsCallBack { get; set; }

        public void GetUsersFriends(Action<ICollection<User>> callback, string id = null, int? count = null, int? page = null, string mode = null, string format = null)
        {

            GetUsersFriendsCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(id)) parameters.Add("id", id);
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/users/friends.json", parameters, GetUsersFriendsEnd);
        }

        private void GetUsersFriendsEnd(string json)
        {
            if (GetUsersFriendsCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<User>>(json);
                GetUsersFriendsCallBack(result);
            }
        }

        public Action<User> CancleRecommendationCallBack { get; set; }

        public void CancleRecommendation(Action<User> callback, string id, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            CancleRecommendationCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/2/users/cancel_recommendation.json", parameters, CancleRecommendationEnd);
        }

        private void CancleRecommendationEnd(string json)
        {
            if (CancleRecommendationCallBack != null)
            {

                var result = Util.JsonToObject<User>(json);
                CancleRecommendationCallBack(result);
            }
        }

        public Action<ICollection<User>> GetUserRecommendationCallBack { get; set; }

        public void GetUserRecommendation(Action<ICollection<User>> callback, int? count = null, int? page = null, string mode = null, string format = null)
        {

            GetUserRecommendationCallBack = callback;
            var parameters = new Parameters();
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/2/users/recommendation.json", parameters, GetUserRecommendationEnd);
        }

        private void GetUserRecommendationEnd(string json)
        {
            if (GetUserRecommendationCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<User>>(json);
                GetUserRecommendationCallBack(result);
            }
        }

    }
}
