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
    public class Photo
    {
        /// <summary>
        /// 指向用户相册图片url
        /// </summary>
        [DataMember(Name = "url")]
        public string Url { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        [DataMember(Name = "imageurl")]
        public string ImageUrl { get; set; }
        /// <summary>
        /// 缩略图地址
        /// </summary>
        [DataMember(Name = "thumburl")]
        public string ThumbUrl { get; set; }
        /// <summary>
        /// 图片原图地址
        /// </summary>
        [DataMember(Name = "largeurl")]
        public string LargeUrl { get; set; }
    }

}
