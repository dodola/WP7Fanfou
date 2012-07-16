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
    public class FirendShips : RequestBase
    {
        public FirendShips(OAuth oauth)
            : base(oauth)
        {
        }

        public Action<User> CreateFriendshipCallBack { get; set; }
        /// <summary>
        /// 添加用户为好友
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        public void CreateFriendship(Action<User> callback, string id, string mode = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            CreateFriendshipCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);

            PostData("http://api.fanfou.com/friendships/create.json", parameters, CreateFriendshipEnd);
        }

        private void CreateFriendshipEnd(string json)
        {
            if (CreateFriendshipCallBack != null)
            {

                var result = Util.JsonToObject<User>(json);
                CreateFriendshipCallBack(result);
            }
        }

        public Action<User> DestroyFriendshipCallBack { get; set; }
        /// <summary>
        /// 取消关注好友
        /// </summary>
        /// <param name="callback"></param>
        public void DestroyFriendship(Action<User> callback, string id, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            DestroyFriendshipCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);

            PostData("http://api.fanfou.com/friendships/destroy.json", parameters, DestroyFriendshipEnd);
        }

        private void DestroyFriendshipEnd(string json)
        {
            if (DestroyFriendshipCallBack != null)
            {

                var result = Util.JsonToObject<User>(json);
                DestroyFriendshipCallBack(result);
            }
        }

        public Action<ICollection<User>> RequestFriendshipCallBack { get; set; }
        /// <summary>
        /// 查询Follow请求.
        /// </summary>
        /// <param name="callback"></param>
        public void RequestFriendship(Action<ICollection<User>> callback, int? page = null, int? count = null, string mode = null, string format = null)
        {

            RequestFriendshipCallBack = callback;
            var parameters = new Parameters();
            if (page != null) parameters.Add("page", page);
            if (count != null) parameters.Add("count", count);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/friendships/requests.json", parameters, RequestFriendshipEnd);
        }

        private void RequestFriendshipEnd(string json)
        {
            if (RequestFriendshipCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<User>>(json);
                RequestFriendshipCallBack(result);
            }
        }


        public Action<User> DenyFriendshipCallBack { get; set; }
        /// <summary>
        /// 拒绝好友请求
        /// </summary>
        /// <param name="callback"></param>
        public void DenyFriendship(Action<User> callback, string id, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            DenyFriendshipCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);

            PostData("http://api.fanfou.com/friendships/deny.json", parameters, DenyFriendshipEnd);
        }

        private void DenyFriendshipEnd(string json)
        {
            if (DenyFriendshipCallBack != null)
            {

                var result = Util.JsonToObject<User>(json);
                DenyFriendshipCallBack(result);
            }
        }

        public Action<User> AcceptFriendshipCallBack { get; set; }
        /// <summary>
        /// 接受好友请求
        /// </summary>
        /// <param name="callback"></param>
        public void AcceptFriendship(Action<User> callback, string id, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            AcceptFriendshipCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            PostData("http://api.fanfou.com/friendships/accept.json", parameters, AcceptFriendshipEnd);
        }

        private void AcceptFriendshipEnd(string json)
        {
            if (AcceptFriendshipCallBack != null)
            {

                var result = Util.JsonToObject<User>(json);
                AcceptFriendshipCallBack(result);
            }
        }

        //TODO:返回两个用户之间follow关系的详细信息 https://github.com/FanfouAPI/FanFouAPIDoc/wiki/friendships.show




    }
}
