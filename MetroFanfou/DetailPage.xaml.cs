using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MetroFanfou.common;
using Microsoft.Phone.Controls;

namespace MetroFanfou
{
    public partial class DetailPage : PhoneApplicationPage
    {

        private string _id;
        public DetailPage()
        {
            InitializeComponent();
            this.ApplicationBar.BackgroundColor = (Color)App.Current.Resources["ApplicationBarBackgroundColor"];

        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey(Const.StatusID))
            {
                var tid = NavigationContext.QueryString[Const.StatusID];
                if (!string.IsNullOrWhiteSpace(tid))
                {
                    progressBar.IsIndeterminate = true;
                    tweetDetail.LoadTweet(tid, null, null);//TODO:完成回调，取出用户信息
                }
            }
            base.OnNavigatedTo(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomeClick(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SmsClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmailClick(object sender, EventArgs e)
        {

        }

        private void btnReply_Click(object sender, EventArgs e)
        {

        }

        private void btnForward_Click(object sender, EventArgs e)
        {

        }

        private void btnFavorite_Click(object sender, EventArgs e)
        {

        }

        private void btnComment_Click(object sender, EventArgs e)
        {
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}