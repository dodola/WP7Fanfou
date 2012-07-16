using System;
using System.Collections.Generic;
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

namespace FanFou.SDK.Objects.Search
{
    [DataContract]
    public class SearchUsers
    {
        [DataMember(Name = "users")]
        public ICollection<User> Users { get; set; }

        [DataMember(Name = "total_number")]
        public int TotalNumber { get; set; }
    }
}
