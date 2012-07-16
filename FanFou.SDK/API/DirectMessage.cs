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
    /// <summary>
    /// 私信
    /// </summary>
    public class DirectMessage : RequestBase
    {
        public DirectMessage(OAuth oauth)
            : base(oauth)
        {
        }

        public Action<DirectMessageObj> DestroyMessageCallBack { get; set; }
        /// <summary>
        /// 删除某条私信
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="id"></param>
        public void DestroyMessage(Action<DirectMessageObj> callback, string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            DestroyMessageCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            GetData("http://api.fanfou.com/direct_messages/destroy.json", parameters, DestroyMessageEnd);
        }

        private void DestroyMessageEnd(string json)
        {
            if (DestroyMessageCallBack != null)
            {

                var result = Util.JsonToObject<DirectMessageObj>(json);
                DestroyMessageCallBack(result);
            }
        }


        public Action<ICollection<DirectMessageObj>> GetConversationCallBack { get; set; }
        /// <summary>
        /// 以对话的形式返回当前用户与某用户的私信
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <param name="format"></param>
        /// <param name="mode"></param>
        public void GetConversation(Action<ICollection<DirectMessageObj>> callback, string id, int? count = null,
                                    int? page = null, string max_id = null, string since_id = null, string format = null,
                                    string mode = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("id不能为空");
            GetConversationCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("id", id);
            if (!string.IsNullOrEmpty(since_id)) parameters.Add("since_id", since_id);
            if (!string.IsNullOrEmpty(max_id)) parameters.Add("max_id", max_id);
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/direct_messages/conversation.json", parameters, GetConversationEnd);
        }

        private void GetConversationEnd(string json)
        {
            if (GetConversationCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<DirectMessageObj>>(json);
                GetConversationCallBack(result);
            }
        }


        public Action<ICollection<DirectMessageObj>> GetSentMessagesCallBack { get; set; }
        /// <summary>
        /// 获取发件箱中的私信
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="count"></param>
        /// <param name="page"></param>
        /// <param name="max_id"></param>
        /// <param name="since_id"></param>
        /// <param name="format"></param>
        /// <param name="mode"></param>
        public void GetSentMessages(Action<ICollection<DirectMessageObj>> callback, int? count = null,
                                    int? page = null, string max_id = null, string since_id = null, string format = null,
                                    string mode = null)
        {

            GetSentMessagesCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(since_id)) parameters.Add("since_id", since_id);
            if (!string.IsNullOrEmpty(max_id)) parameters.Add("max_id", max_id);
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/direct_messages/sent.json", parameters, GetSentMessagesEnd);
        }

        private void GetSentMessagesEnd(string json)
        {
            if (GetSentMessagesCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<DirectMessageObj>>(json);
                GetSentMessagesCallBack(result);
            }
        }


        public Action<DirectMessageObj> NewDirectMessageCallBack { get; set; }

        public void NewDirectMessage(Action<DirectMessageObj> callback, string user, string text, string in_reply_to_id = null, string mode = null)
        {
            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(text)) throw new ArgumentException("user不能为空");
            NewDirectMessageCallBack = callback;
            var parameters = new Parameters();
            parameters.Add("user", user);
            parameters.Add("text", text);
            if (!string.IsNullOrEmpty(in_reply_to_id)) parameters.Add("in_reply_to_id", in_reply_to_id);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            PostData("http://api.fanfou.com/direct_messages/new.json", parameters, NewDirectMessageEnd);
        }

        private void NewDirectMessageEnd(string json)
        {
            if (NewDirectMessageCallBack != null)
            {

                var result = Util.JsonToObject<DirectMessageObj>(json);
                NewDirectMessageCallBack(result);
            }
        }


        public Action<ConversationList> GetConversationListCallBack { get; set; }

        public void GetConversationList(Action<ConversationList> callback, int? page = null, int? count = null,
                                        string mode = null)
        {

            GetConversationListCallBack = callback;
            var parameters = new Parameters();
            if (page != null) parameters.Add("page", page);
            if (count != null) parameters.Add("count", count);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            GetData("http://api.fanfou.com/direct_messages/conversation_list.json", parameters, GetConversationListEnd);
        }

        private void GetConversationListEnd(string json)
        {
            if (GetConversationListCallBack != null)
            {

                var result = Util.JsonToObject<ConversationList>(json);
                GetConversationListCallBack(result);
            }
        }


        public Action<ICollection<DirectMessageObj>> GetInboxMessagesesCallBack { get; set; }

        public void GetInboxMessageses(Action<ICollection<DirectMessageObj>> callback, int? count = null,
                                    int? page = null, string max_id = null, string since_id = null, string format = null,
                                    string mode = null)
        {

            GetInboxMessagesesCallBack = callback;
            var parameters = new Parameters();
            if (!string.IsNullOrEmpty(since_id)) parameters.Add("since_id", since_id);
            if (!string.IsNullOrEmpty(max_id)) parameters.Add("max_id", max_id);
            if (count != null) parameters.Add("count", count);
            if (page != null) parameters.Add("page", page);
            if (!string.IsNullOrEmpty(mode)) parameters.Add("mode", mode);
            if (!string.IsNullOrEmpty(format)) parameters.Add("format", format);
            GetData("http://api.fanfou.com/direct_messages/inbox.json", parameters, GetInboxMessagesesEnd);
        }

        private void GetInboxMessagesesEnd(string json)
        {
            if (GetInboxMessagesesCallBack != null)
            {

                var result = Util.JsonToObject<ICollection<DirectMessageObj>>(json);
                GetInboxMessagesesCallBack(result);
            }
        }







    }
}
