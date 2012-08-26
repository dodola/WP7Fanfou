using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using FanFou.SDK.API;
using FanFou.SDK.Objects;
using MetroFanfou.Helper;
using MetroFanfou.common;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MetroFanfou
{
    public partial class Profile : PhoneApplicationPage
    {

        #region 私有变量

        private User _user;
        private string AccountId { get; set; }

        /// <summary>
        /// 是否是我自己
        /// </summary>
        private bool IsMyself
        {
            get { return System.String.CompareOrdinal(App.CurrentUser.Id, AccountId) == 0; }
        }

        #endregion

        public Profile()
        {
            InitializeComponent();
            profileTweetList.Selected = SelectTweet;
            idolList.Selected = SelectUser;
            fansList.Selected = SelectUser;
            //fansList.GotLastest = ClearFansCount;
            profileTweetList.MenuItemClick = MenuItemClick;
            ApplicationBar.BackgroundColor = (Color)Application.Current.Resources["ApplicationBarBackgroundColor"];
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (NavigationContext.QueryString.ContainsKey(AppSetting.UserNameParameterKey))
            {
                string name = NavigationContext.QueryString[AppSetting.UserNameParameterKey];
                if (!string.IsNullOrWhiteSpace(name))
                {
                    AccountId = name;
                    progressBar.IsIndeterminate = true;
                    GetUserInfo();
                }
            }
        }

        /// <summary>
        ///  请求用户数据
        /// </summary>
        private void GetUserInfo()
        {
            if (IsMyself)
            {
                GetUserInfoEnd(App.CurrentUser);
                // TM.QWeibo.Dal.User.Instance.MyInfo(GetUserInfoEnd);
            }
            else
            {

                //TM.QWeibo.Dal.User.Instance.OtherUser(_accountID, GetUserInfoEnd);
            }
        }

        /// <summary>
        /// 请求用户资料回调
        /// </summary>
        /// <param name="userInfo"></param>
        private void GetUserInfoEnd(User userInfo)
        {
            Dispatcher.BeginInvoke(() =>
                                       {
                                           _user = userInfo;
                                           UserInfo.BindInfo(userInfo);
                                           profileInfo.BindInfo(userInfo);
                                           tbIdolNum.Text = userInfo.FollowersCount.ToString();
                                           tbFansNum.Text = userInfo.FriendsCount.ToString();
                                           tbTweetNum.Text = userInfo.StatusesCount.ToString();
                                           progressBar.IsIndeterminate = false;
                                           InitButton(userInfo);
                                       });
        }

        /// <summary>
        /// 初始化页面的按钮
        /// </summary>
        /// <param name="user"></param>
        private void InitButton(User user)
        {
            Dispatcher.BeginInvoke(() =>
                                       {
                                           IList appbarbtns = ApplicationBar.Buttons;

                                           appbarbtns.Clear();

                                           if (!IsMyself)
                                           {
                                               //收听按钮
                                               var IdolBtn = new ApplicationBarIconButton
                                                                 {
                                                                     //Text = user.IsMyidol ? "取消收听" : "收听",
                                                                     //IconUri =
                                                                     //    new Uri(
                                                                     //    "/Resource/Icons/" +
                                                                     //    (user.IsMyidol ? "noidol" : "idol") + ".png",
                                                                     //    UriKind.Relative)
                                                                 };
                                               IdolBtn.Click += btnIdol_Click;
                                               appbarbtns.Add(IdolBtn);

                                               //收听我的人可以发私信
                                               if (user.Following)
                                               {
                                                   var MessageBtn = new ApplicationBarIconButton
                                                                        {
                                                                            Text = "私信",
                                                                            IconUri =
                                                                                new Uri("/Resource/Icons/message.png",
                                                                                        UriKind.Relative)
                                                                        };
                                                   MessageBtn.Click += btnMessage_Click;
                                                   appbarbtns.Add(MessageBtn);
                                               }

                                               //@某人
                                               var MentioBtn = new ApplicationBarIconButton
                                                                   {
                                                                       Text = "@ta",
                                                                       IconUri =
                                                                           new Uri("/Resource/Icons/mentions.png",
                                                                                   UriKind.Relative)
                                                                   };
                                               MentioBtn.Click += btnMention_Click;
                                               appbarbtns.Add(MentioBtn);
                                           }

                                           //返回首页
                                           var homeBtn = new ApplicationBarIconButton
                                                             {
                                                                 Text = "首页",
                                                                 IconUri =
                                                                     new Uri("/Resource/Icons/home.png",
                                                                             UriKind.Relative)
                                                             };
                                           homeBtn.Click += btnHome_Click;
                                           appbarbtns.Add(homeBtn);
                                       });
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_user != null)
            {
                int index = pivotProfile.SelectedIndex;
                switch (index)
                {
                    case 1:
                        profileTweetList.Init(BeforeLoading, AfterLoaded);
                        break;
                    case 2:
                        idolList.Init(AccountId, BeforeLoading, AfterLoaded);
                        break;
                    case 3:
                        fansList.Init(AccountId, BeforeLoading, AfterLoaded);
                        break;
                }
            }
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
        private void AfterLoaded(object count)
        {
            Dispatcher.BeginInvoke(() => { progressBar.IsIndeterminate = false; });
        }

        /// <summary>
        /// 选中微博，跳转到详细页
        /// </summary>
        /// <param name="t"></param>
        private void SelectTweet(Status t)
        {
            if (t != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                    NavigationService.Navigate(
                        new Uri(string.Format("/Tweet/Detail.xaml?{0}={1}", AppSetting.TweetIdParameterKey, t.Id),
                                UriKind.Relative)));
            }
        }

        /// <summary>
        /// 选中用户，跳转到详细页
        /// </summary>
        /// <param name="t"></param>
        private void SelectUser(User u)
        {
            if (u != null)
            {
                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                    NavigationService.Navigate(
                        new Uri(string.Format("/Profile.xaml?{0}={1}", AppSetting.UserNameParameterKey, u.Name),
                                UriKind.Relative)));
            }
        }



        /// <summary>
        /// 控件弹出菜单服务
        /// </summary>
        /// <param name="t"></param>
        /// <param name="act"></param>
        private void MenuItemClick(Status t, EMenuItemAction act)
        {
            if (act == EMenuItemAction.Reply)
            {
                Isolated.Set(IsolatedHelper.TargetTweetKey, t);
                NavigationService.Navigate(
                    new Uri(
                        string.Format("/Tweet/Add.xaml?{0}={1}", AppSetting.AddTweetTypeParameterKey,
                                      EAddTweetType.Reply.GetHashCode()), UriKind.Relative));
            }
            else if (act == EMenuItemAction.ReAdd)
            {
                Isolated.Set(IsolatedHelper.TargetTweetKey, t);
                NavigationService.Navigate(
                    new Uri(
                        string.Format("/Tweet/Add.xaml?{0}={1}", AppSetting.AddTweetTypeParameterKey,
                                      EAddTweetType.ReAdd.GetHashCode()), UriKind.Relative));
            }
            else if (act == EMenuItemAction.Favorite)
            {
                //Dal.Tweet.Instance.AddFavorite(t.Id,
                //                               (rs) =>
                //                               {
                //                                   (new MessageBoxHelper()).Show("收藏" + (rs ? "成功" : "失败"), "提示",
                //                                                                 LayoutRoot);
                //                               });
            }
            else if (act == EMenuItemAction.Comment)
            {
                Isolated.Set(IsolatedHelper.TargetTweetKey, t);
                NavigationService.Navigate(
                    new Uri(
                        string.Format("/Tweet/Add.xaml?{0}={1}", AppSetting.AddTweetTypeParameterKey,
                                      EAddTweetType.Comment.GetHashCode()), UriKind.Relative));
            }
        }

        #region 按钮事件

        /// <summary>
        /// 收听按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIdol_Click(object sender, EventArgs e)
        {
            //if (_user.IsMyidol)
            //{
            //    Dal.Friends.Instance.Delete(_user.Name, FriendActionEnd);
            //}
            //else
            //{
            //    Dal.Friends.Instance.Add(_user.Name, FriendActionEnd);
            //}
        }

        //private void FriendActionEnd(bool rs)
        //{
        //    if (rs)
        //    {
        //        _user.IsMyidolFlag = _user.IsMyidol ? 0 : 1;
        //        Dispatcher.BeginInvoke(() =>
        //                                   {
        //                                       var btn = (ApplicationBarIconButton)ApplicationBar.Buttons[0];
        //                                       btn.IconUri =
        //                                           new Uri(
        //                                               "/Resource/Icons/" + (_user.IsMyidol ? "noidol" : "idol") +
        //                                               ".png", UriKind.Relative);
        //                                       btn.Text = _user.IsMyidol ? "取消" : "收听";
        //                                       (new MessageBoxHelper()).Show(
        //                                           (_user.IsMyidol
        //                                                ? "收听" + _user.NickName + "成功"
        //                                                : "已取消收听" + _user.NickName + "成功"), null, LayoutRoot);
        //                                   });
        //    }
        //}

        /// <summary>
        /// 私信
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMessage_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(
                new Uri(
                    string.Format("/Tweet/Add.xaml?{0}={1}&{2}={3}", AppSetting.AddTweetTypeParameterKey,
                                  EAddTweetType.PrivateMessage.GetHashCode(), AppSetting.AddTweetMetaParameterKey,
                                  _user.Name), UriKind.Relative));
        }

        /// <summary>
        /// 提及
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMention_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(
                new Uri(
                    string.Format("/Tweet/Add.xaml?{0}={1}&{2}={3}", AppSetting.AddTweetTypeParameterKey,
                                  EAddTweetType.Mention.GetHashCode(), AppSetting.AddTweetMetaParameterKey, _user.Name),
                    UriKind.Relative));
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnHome_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        #endregion
    }
}