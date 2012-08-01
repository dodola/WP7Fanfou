using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FanFou.SDK.API;
using FanFou.SDK.Objects;
using MetroFanfou.common;

namespace MetroFanfou
{
    public partial class MainPage
    {
        private readonly Users _usersApi = new Users(OauthHelper.OAuth());

        public MainPage()
        {
            InitializeComponent();
            ApplicationBar.BackgroundColor = (Color) Application.Current.Resources["ApplicationBarBackgroundColor"];
            homeItem.Init(BeforeLoading, AfterLoaded);

            new Thread(() => _usersApi.UsersShow(UserLoaded)).Start();
        }

        private void UserLoaded(User obj)
        {
            Dispatcher.BeginInvoke(() =>
                                       {
                                           tbTweetCount.Text = obj.StatusesCount.ToString(CultureInfo.InvariantCulture);
                                           tbCommentCount.Text =
                                               obj.FavouritesCount.ToString(CultureInfo.InvariantCulture);
                                           tbFanxCount.Text = obj.FollowersCount.ToString(CultureInfo.InvariantCulture);
                                           tbAccountName.Text = obj.Name;
                                           tbMessageCount.Text = obj.FriendsCount.ToString(CultureInfo.InvariantCulture);
                                       });
        }

        private void MSettingClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/Setting.xaml"), UriKind.Relative));
        }

        private void MLogoutClick(object sender, EventArgs e)
        {
        }

        private void MHelpClick(object sender, EventArgs e)
        {
        }

        private void MExitClick(object sender, EventArgs e)
        {
        }

        private void BtnReloadClick(object sender, EventArgs e)
        {
            switch (PivotMain.SelectedIndex)
            {
                case 0:
                    homeItem.Reset();
                    break;
                case 1:
                    replyItem.Reset();
                    break;
                case 2:
                    publicItem.Reset();
                    break;
            }
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
        }

        private void BtnProfileClick(object sender, EventArgs e)
        {
        }

        private void MAboutClick(object sender, EventArgs e)
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
            Dispatcher.BeginInvoke(() => { performanceBar.IsIndeterminate = false; });
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