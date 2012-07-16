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

namespace FanFou.SDK.Objects.Search
{
    [DataContract]
    public class SavedSearch
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "query")]
        public string Query { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "create_at")]
        public string CreateAt { get; set; }
    }
}
