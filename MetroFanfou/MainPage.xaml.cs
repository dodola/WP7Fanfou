using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using FanFou.SDK.API;
using FanFou.SDK.Objects;
using MetroFanfou.common;
using Microsoft.Phone.Controls;

namespace MetroFanfou
{
    public partial class MainPage : PhoneApplicationPage
    {
        private Users usersApi = new Users(OauthHelper.OAuth());
        public MainPage()
        {
            InitializeComponent();
            this.ApplicationBar.BackgroundColor = (Color)App.Current.Resources["ApplicationBarBackgroundColor"];
            homeItem.Init(BeforeLoading, AfterLoaded);

            new Thread(() => usersApi.UsersShow(UserLoaded)).Start();

        }

        private void UserLoaded(User obj)
        {
            this.Dispatcher.BeginInvoke(() =>
                                            {

                                                this.tbTweetCount.Text = obj.StatusesCount.ToString();
                                                this.tbCommentCount.Text = obj.FavouritesCount.ToString();
                                                this.tbFanxCount.Text = obj.FollowersCount.ToString();
                                                this.tbAccountName.Text = obj.Name;
                                                this.tbMessageCount.Text = obj.FriendsCount.ToString();

                                            });
        }

        private void mSetting_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/Setting.xaml"), UriKind.Relative));
        }

        private void mLogout_Click(object sender, EventArgs e)
        {

        }

        private void mHelp_Click(object sender, EventArgs e)
        {

        }

        private void mExit_Click(object sender, EventArgs e)
        {

        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            switch (PivotMain.SelectedIndex)
            {
                case 0: homeItem.Reset(); break;
                case 1: replyItem.Reset(); break;
                case 2: publicItem.Reset(); break;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnProfile_Click(object sender, EventArgs e)
        {

        }

        private void mAbout_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 加载数据前显示loading状态
        /// </summary>
        private void BeforeLoading()
        {
            Dispatcher.BeginInvoke(() => performanceBar.IsIndeterminate = true);
        }

        /// <summary>
        /// 加载数据后隐藏loading状态，显示相关信息
        /// </summary>
        private void AfterLoaded(object tweets)
        {
            Dispatcher.BeginInvoke(() =>
            {
                performanceBar.IsIndeterminate = false;
            });
        }

        private void PhoneApplicationPageLoaded(object sender, RoutedEventArgs e)
        {


        }

        private void PivotMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (PivotMain.SelectedIndex)
            {
                case 0:
                    homeItem.Init(BeforeLoading, AfterLoaded);
                    break;
                case 1:
                    replyItem.Init(BeforeLoading, AfterLoaded);
                    break;
                case 2:
                    publicItem.Init(BeforeLoading, AfterLoaded);
                    break;
            }
        }
    }
}