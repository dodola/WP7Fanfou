using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FanFou.SDK.API;
using FanFou.SDK.Objects;
using MetroFanfou.Controls;
using MetroFanfou.common;

namespace MetroFanfou
{
    public partial class MainPage : BasePage
    {
        private readonly Users _usersApi = new Users(OauthHelper.OAuth());

        public MainPage()
        {
            InitializeComponent();
            ApplicationBar.BackgroundColor = (Color)Application.Current.Resources["ApplicationBarBackgroundColor"];
            homeItem.Selected = SelectStatus;
            replyItem.Selected = SelectStatus;
            publicItem.Selected = SelectStatus;
            Dispatcher.BeginInvoke(() =>
                                       {
                                           _usersApi.UsersShow(UserLoaded);
                                           if (NavigationService.CanGoBack)
                                               NavigationService.RemoveBackEntry();
                                       });
        }

        private void SelectStatus(Status status)
        {
            Dispatcher.BeginInvoke(
                () =>
                NavigationService.Navigate(new Uri(
                                               String.Format("/DetailPage.xaml?{0}={1}", Const.StatusID, status.Id),
                                               UriKind.Relative)));
        }

        private void UserLoaded(User obj)
        {
            App.CurrentUser = obj;
            Dispatcher.BeginInvoke(() =>
                                       {
                                           //tbTweetCount.Text = obj.StatusesCount.ToString(CultureInfo.InvariantCulture);
                                           //tbCommentCount.Text =
                                           //    obj.FavouritesCount.ToString(CultureInfo.InvariantCulture);
                                           //tbFanxCount.Text = obj.FollowersCount.ToString(CultureInfo.InvariantCulture);
                                           tbAccountName.Text = obj.Name;
                                           //tbMessageCount.Text = obj.FriendsCount.ToString(CultureInfo.InvariantCulture);
                                       });
        }

        private void MSettingClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/Setting.xaml"), UriKind.Relative));
        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MLogoutClick(object sender, EventArgs e)
        {
            var rs = MessageBox.Show("确定注销吗？\r\n注销后将会自动退出，重启程序继续使用。", "注销", MessageBoxButton.OKCancel);
            if (rs == MessageBoxResult.OK)
            {
                OauthHelper.Logout();
                OauthHelper.IsVerified = false;
                AppSetting.IsScheduledAgent = false;
                AppSetting.CheckUpdateSecondSpan = 0;

                App.Quit();
            }
        }

        private void MHelpClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MExitClick(object sender, EventArgs e)
        {
            var b = true;
            if (AppSetting.IsExitConfirm)
            {
                var rs = MessageBox.Show("确定退出吗？", "提示", MessageBoxButton.OKCancel);
                b = rs != MessageBoxResult.Cancel;
            }
            if (b)
            {
                App.Quit();
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAddClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/AddStatus.xaml"), UriKind.Relative));
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

        private void PivotMainSelectionChanged(object sender, SelectionChangedEventArgs e)
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