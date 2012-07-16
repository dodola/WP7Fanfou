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
    public class Notification
    {
        [DataMember(Name = "mentions")]
        public int Mentions { get; set; }
        [DataMember(Name = "direct_messages")]
        private int DirectMessages { get; set; }
        [DataMember(Name = "friend_requests")]
        private int FriendRequests { get; set; }

    }
}
