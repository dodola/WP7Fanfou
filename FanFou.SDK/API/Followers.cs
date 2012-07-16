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
    public class Followers : RequestBase
    {
        public Followers(OAuth oauth)
            : base(oauth)
        {
        }

        public Action<ICollection<string>> FollowersIdsCallBack { get; set; }
        /// <summary>
        /// 返回用户关注者的id列表
        /// </summary>
        /// <param name="callback"></param>
        public void GetFollowersIds(Action<ICollection<string>> callback, string id = null, int? page = null, int? count = null)
        {

            FollowersIdsCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(id)) parameters.Add("id", id);
            if (page != null) parameters.Add("page", page);
            if (count != null) parameters.Add("count", count);
            GetData("http://api.fanfou.com/followers/ids.json", parameters, FollowersIdsEnd);
        }

        private void FollowersIdsEnd(string json)
        {
            if (FollowersIdsCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<string>>(json);
                FollowersIdsCallBack(result);
            }
        }
    }
}
