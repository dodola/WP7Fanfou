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
    public class ConversationList
    {
        [DataMember(Name = "dm")]
        public DirectMessageObj Dm { get; set; }

        [DataMember(Name = "otherid")]
        public string Otherid { get; set; }

        [DataMember(Name = "msg_num")]
        public int MsgNum { get; set; }

        [DataMember(Name = "new_conv")]
        public bool NewConv { get; set; }
    }
}
