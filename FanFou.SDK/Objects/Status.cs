using System;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FanFou.SDK.Objects
{
    [DataContract]
    public class Status
    {
        /// <summary>
        /// 消息发送时间
        /// </summary>
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }
        /// <summary>
        /// 消息id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }
        /// <summary>
        /// 消息序列号（可用于排序）
        /// </summary>
        [DataMember(Name = "rawid")]
        public long Rawid { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }
        /// <summary>
        /// 消息来源
        /// </summary>
        [DataMember(Name = "source")]
        public string Source { get; set; }
        /// <summary>
        /// 消息是否被截断
        /// </summary>
        [DataMember(Name = "truncated")]
        public bool Truncated { get; set; }
        /// <summary>
        /// 回复的消息id
        /// </summary>
        [DataMember(Name = "in_reply_to_status_id")]
        public string InReplyToStatusId { get; set; }
        /// <summary>
        /// 回复的用户id
        /// </summary>
        [DataMember(Name = "in_reply_to_user_id")]
        public string InReplyToUserId { get; set; }
        /// <summary>
        /// 消息是否被登录用户收藏
        /// </summary>
        [DataMember(Name = "favorited")]
        public bool Favorited { get; set; }
        /// <summary>
        /// 回复用户的昵称
        /// </summary>
        [DataMember(Name = "in_reply_to_screen_name")]
        public string InReplyToScreenName { get; set; }
        /// <summary>
        /// 转发的用户id
        /// </summary>
        [DataMember(Name = "repost_status_id")]
        public string RepostStatusId { get; set; }
        /// <summary>
        /// 被转发用户的id
        /// </summary>
        [DataMember(Name = "repost_user_id")]
        public string RepostUserId { get; set; }
        /// <summary>
        /// 转发用户的昵称
        /// </summary>
        [DataMember(Name = "repost_screen_name")]
        public string RepostScreenName { get; set; }
        /// <summary>
        /// 转发的消息详细信息
        /// </summary>
        [DataMember(Name = "repost_status")]
        public Status RepostStatus { get; set; }
        /// <summary>
        /// 消息的位置，格式可能是"北京 朝阳区"也可能是"234.333,47.9"
        /// </summary>
        [DataMember(Name = "location")]
        public string Location { get; set; }
        /// <summary>
        /// 发送消息的用户信息
        /// </summary>
        [DataMember(Name = "user")]
        public User User { get; set; }
        /// <summary>
        /// 消息中图片信息
        /// </summary>
        [DataMember(Name = "photo")]
        public Photo StatusPhoto { get; set; }
    }


}

