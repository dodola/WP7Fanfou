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
    public class Favorites : RequestBase
    {
        public Favorites(OAuth oauth)
            : base(oauth)
        {
        }

        public Action<Status> DestroyFavoriteCallBack { get; set; }
        /// <summary>
        /// 取消收藏指定消息(当前用户的收藏)
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        public void DestroyFavorite(Action<Status> callback, string id, string mode = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            DestroyFavoriteCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            PostData("http://api.fanfou.com/statuses/favorites/destroy/id.json", parameters, DestroyFavoriteEnd);
        }

        private void DestroyFavoriteEnd(string json)
        {
            if (DestroyFavoriteCallBack != null)
            {

                var result = Util.JsonToObject<Status>(json);
                DestroyFavoriteCallBack(result);
            }
        }


        public Action<ICollection<Status>> GetFavoritesCallBack { get; set; }

        public void GetFavorites(Action<ICollection<Status>> callback, string id = null, int? count = null, int? page = null, string mode = null, string format = null)
        {

            GetFavoritesCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(id)) parameters.Add("id", id);
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/favorites/id.json", parameters, GetFavoritesEnd);
        }

        private void GetFavoritesEnd(string json)
        {
            if (GetFavoritesCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<Status>>(json);
                GetFavoritesCallBack(result);
            }
        }

        public Action<Status> CreateFavoriteCallBack { get; set; }

        public void CreateFavorite(Action<Status> callback, string id, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            CreateFavoriteCallBack = callback;
            var parameters = new Parameters();

            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            PostData(string.Format("http://api.fanfou.com/statuses/favorites/create/{0}.json", id), parameters, CreateFavoriteEnd);
        }

        private void CreateFavoriteEnd(string json)
        {
            if (CreateFavoriteCallBack != null)
            {

                var result = Util.JsonToObject<Status>(json);
                CreateFavoriteCallBack(result);
            }
        }

    }
}
