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
    public class DirectMessageObj
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "text")]
        public string Text { get; set; }
        [DataMember(Name = "sender_id")]
        public string SenderId { get; set; }
        [DataMember(Name = "recipient_id")]
        public string RecipientId { get; set; }
        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }
        [DataMember(Name = "sender_screen_name")]
        public string SenderScreenName { get; set; }
        [DataMember(Name = "recipient_screen_name")]
        public string RecipientScreenName { get; set; }
        [DataMember(Name = "sender")]
        public User Sender { get; set; }
        [DataMember(Name = "recipient")]
        public User Recipient { get; set; }
        [DataMember(Name = "in_reply_to")]
        public DirectMessageObj InReplyTo { get; set; }

    }
}
