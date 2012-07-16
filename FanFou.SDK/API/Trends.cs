using System;
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
using FanFou.SDK.Objects.Trends;

namespace FanFou.SDK.API
{
    public class Trends : RequestBase
    {
        public Trends(OAuth oauth)
            : base(oauth)
        {
        }

        public Action<TrendCollection> TrendsListCallBack { get; set; }
        /// <summary>
        /// 列出饭否热门话题
        /// </summary>
        /// <param name="callback"></param>
        public void GetTrendsList(Action<TrendCollection> callback)
        {

            TrendsListCallBack = callback;


            GetData("http://api.fanfou.com/trends/list.json", null, TrendsListEnd);
        }

        private void TrendsListEnd(string json)
        {
            if (TrendsListCallBack != null)
            {

                var result = Util.JsonToObject<TrendCollection>(json);
                TrendsListCallBack(result);
            }
        }

    }
}
