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
    public class Blocks : RequestBase
    {
        public Blocks(OAuth oauth)
            : base(oauth)
        {
        }

        public Action<ICollection<string>> GetBlocksIdsCallBack { get; set; }

        /// <summary>
        /// 获取黑名单id列表
        /// </summary>
        /// <param name="callback"></param>
        public void GetBlocksIds(Action<ICollection<String>> callback)
        {
            GetBlocksIdsCallBack = callback;
            GetData("http://api.fanfou.com/blocks/ids.json", null, GetBlocksIdsEnd);
        }

        private void GetBlocksIdsEnd(string json)
        {
            if (GetBlocksIdsCallBack != null)
            {
                var result = Util.JsonToObject<ICollection<string>>(json);
                GetBlocksIdsCallBack(result);
            }
        }

        public Action<ICollection<User>> GetBlockingCallBack { get; set; }
        /// <summary>
        /// 获取黑名单上用户资料
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="mode"></param>
        /// <param name="page"></param>
        /// <param name="count">count不设置的时候默认值为20</param>
        public void GetBlocking(Action<ICollection<User>> callback, string mode = null, int? page = null, int? count = null)
        {
            this.GetBlockingCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (page != null) parameters.Add("page", page);
            if (count != null) parameters.Add("count", count);
            GetData("http://api.fanfou.com/blocks/blocking.json", parameters, GetBlockingEnd);
        }

        private void GetBlockingEnd(string json)
        {
            if (GetBlockingCallBack != null)
            {
                var result = Util.JsonToObject<ICollection<User>>(json);
                GetBlockingCallBack(result);

            }
        }

        public Action<User> CreateBlockCallBack { get; set; }
        /// <summary>
        /// 把指定id用户加入黑名单
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="id"></param>
        /// <param name="mode"></param>
        /// <param name="format"></param>
        public void CreateBlock(Action<User> callback, string id, string mode = null, string format = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            this.CreateBlockCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);

            GetData("http://api.fanfou.com/blocks/create.json", parameters, CreateBlockEnd);


        }
        private void CreateBlockEnd(string json)
        {
            if (CreateBlockCallBack != null)
            {
                var result = Util.JsonToObject<User>(json);

                CreateBlockCallBack(result);
            }
        }


        public Action<User> ExistsBlockCallBack { get; set; }

        public void ExistsBlock(Action<User> callback, string id, string mode = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            ExistsBlockCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            GetData("http://api.fanfou.com/blocks/exists.json", parameters, ExistsBlockEnd);
        }
        private void ExistsBlockEnd(string json)
        {
            if (ExistsBlockCallBack != null)
            {

                var result = Util.JsonToObject<User>(json);
                ExistsBlockCallBack(result);
            }
        }




        public Action<User> DestroyBlockCallBack { get; set; }

        public void DestroyBlock(Action<User> callback, string id, string mode = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            DestroyBlockCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            GetData("http://api.fanfou.com/blocks/destroy.json", parameters, DestroyBlockEnd);
        }
        private void DestroyBlockEnd(string json)
        {
            if (DestroyBlockCallBack != null)
            {
                var result = Util.JsonToObject<User>(json);
                DestroyBlockCallBack(result);
            }
        }
    }
}
