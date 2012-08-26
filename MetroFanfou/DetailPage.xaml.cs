using System;
using System.Windows.Controls;
using System.Windows.Media;
using FanFou.SDK.Objects;
using MetroFanfou.common;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace MetroFanfou
{
    public partial class DetailPage : PhoneApplicationPage
    {
        private string _id;
        public Status CurrentStatus { get; set; }
        public DetailPage()
        {
            InitializeComponent();
            this.ApplicationBar.BackgroundColor = (Color)App.Current.Resources["ApplicationBarBackgroundColor"];
            if (App.CurrentUser != null) this.tbAccountName.Text = App.CurrentUser.ScreenName;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey(Const.StatusID))
            {
                var tid = NavigationContext.QueryString[Const.StatusID];
                if (!string.IsNullOrWhiteSpace(tid))
                {
                    progressBar.IsIndeterminate = true;
                    tweetDetail.LoadTweet(tid, BeforeLoading, AfterLoaded);
                }
            }
            base.OnNavigatedTo(e);
        }
        /// <summary>
        /// 加载数据前显示loading状态
        /// </summary>
        private void BeforeLoading()
        {
            Dispatcher.BeginInvoke(() => progressBar.IsIndeterminate = true);
        }

        /// <summary>
        /// 加载数据后隐藏loading状态，显示相关信息
        /// </summary>
        private void AfterLoaded(Status status)
        {

            CurrentStatus = status;
            this.DataContext = CurrentStatus;
            Dispatcher.BeginInvoke(() => { progressBar.IsIndeterminate = false; });
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HomeClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        /// <summary>
        ///短信分享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SmsClick(object sender, EventArgs e)
        {
            var smsTask = new SmsComposeTask();
            smsTask.Body = CurrentStatus.Text;
            smsTask.Show();
        }

        /// <summary>
        /// 邮件分享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmailClick(object sender, EventArgs e)
        {
            var emailTask = new EmailComposeTask();
            emailTask.Subject = "分享微博";
            emailTask.Body = CurrentStatus.Text;
            emailTask.Show();
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


    }
}