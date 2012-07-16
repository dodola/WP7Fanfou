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
    public class Firends : RequestBase
    {
        public Firends(OAuth oauth)
            : base(oauth)
        {
        }

        public Action<ICollection<string>> GetFriendIdsCallBack { get; set; }

        public void GetFriendIds(Action<ICollection<string>> callback, string id = null, int? page = null, int? count = null)
        {

            GetFriendIdsCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(id)) parameters.Add("id", id);
            if (page != null) parameters.Add("page", page);
            if (count != null) parameters.Add("count", count);

            GetData("http://api.fanfou.com/friends/ids.json", parameters, GetFriendIdsEnd);
        }

        private void GetFriendIdsEnd(string json)
        {
            if (GetFriendIdsCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<string>>(json);
                GetFriendIdsCallBack(result);
            }
        }
    }
}
