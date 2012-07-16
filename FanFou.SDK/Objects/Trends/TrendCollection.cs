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

namespace FanFou.SDK.Objects.Trends
{
    [DataContract]
    public class TrendCollection
    {
        [DataMember(Name = "as_of")]
        public string AsOf { get; set; }

        [DataMember(Name = "trends")]
        public ICollection<TrendObj> Trends { get; set; }
    }
}
