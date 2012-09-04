﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FanFou.SDK.API;
using FanFou.SDK.Objects;
using MetroFanfou.Helper;
using MetroFanfou.common;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Input.Touch;
using WP7_ControlsLib.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace MetroFanfou.Controls
{
    public partial class StatusList : UserControl
    {
        private Statuses _statusApi;
        private double lastListBoxScrollableVerticalOffset;
        private ScrollViewer listBox_ScrollViewer;

        #region 属性枚举

        #region EShowType enum

        /// <summary>
        /// 显示类型
        /// </summary>
        public enum EShowType
        {
            /// <summary>
            /// 完全
            /// </summary>
            Full,

            /// <summary>
            /// 简单
            /// </summary>
            Simple,

            /// <summary>
            /// 回复，无父
            /// </summary>
            Reply
        }

        #endregion

        #region ETimeline enum

        /// <summary>
        /// 时间线类型
        /// </summary>
        public enum ETimeline
        {
            /// 首页
            ///
            Home = 1,

            /// <summary>
            /// 提及
            /// </summary>
            Reply,

            /// <summary/>
            /// <summary>
            /// 随便看看
            /// </summary>
            Public,

            User
        }

        #endregion

        /// <summary>
        /// 拉取数据类型
        /// </summary>
        private enum EPollType
        {
            /// <summary>
            /// 默认，结果绑定到控件
            /// </summary>
            Default,

            /// <summary>
            /// 下一页，结果追加到末尾
            /// </summary>
            NextPage,

            /// <summary>
            /// 最新，结果插到开头
            /// </summary>
            Lastest
        }

        #endregion 属性枚举

        #region 私有属性

        /// <summary>
        /// 拖取的指定用户或是指定的微博ID
        /// </summary>
        private string _additionalData;

        /// <summary>
        /// 是否正在拉取数据
        /// </summary>
        private bool IsPolling { get; set; }

        /// <summary>
        /// 是否初始化，加载数据
        /// </summary>
        private bool IsInited { get; set; }

        /// <summary>
        /// 加载数据的类型
        /// </summary>
        private EPollType PollType { get; set; }

        /// <summary>
        /// 最后一条微博rawid
        /// </summary>
        private long LastRawId { get; set; }

        /// <summary>
        /// 第一条微博的rawid
        /// </summary>
        private long FirstRawId { get; set; }

        private string FirstId { get; set; }

        private string LastId { get; set; }

        /// <summary>
        /// 最后一次刷新的时间
        /// </summary>
        private DateTime LastPollTime { get; set; }

        #endregion 私有属性

        #region 自定义属性

        /// <summary>
        /// 依赖项，显示的时间线内容类型
        /// </summary>
        public static readonly DependencyProperty TimelineProperty = DependencyProperty.Register("Timeline",
                                                                                                 typeof(ETimeline),
                                                                                                 typeof(StatusList),
                                                                                                 new PropertyMetadata(
                                                                                                     ETimeline.Home));

        /// <summary>
        /// 依赖项，显示方式
        /// </summary>
        public static readonly DependencyProperty ShowTypeProperty = DependencyProperty.Register("ShowType",
                                                                                                 typeof(EShowType),
                                                                                                 typeof(StatusList),
                                                                                                 new PropertyMetadata(
                                                                                                     EShowType.Full));

        /// <summary>
        /// 时间线类型
        /// </summary>
        [Description("时间线类型")]
        public ETimeline Timeline
        {
            get { return (ETimeline)GetValue(TimelineProperty); }
            set { SetValue(TimelineProperty, value); }
        }

        /// <summary>
        ///  显示类型
        /// </summary>
        [Description("显示类型")]
        public EShowType ShowType
        {
            get { return (EShowType)GetValue(ShowTypeProperty); }
            set { SetValue(ShowTypeProperty, value); }
        }

        #endregion 自定义属性

        #region 本控件事件

        private Action BeforeLoadingCallback { get; set; }

        private Action<object> AfterLoadedCallback { get; set; }

        /// <summary>
        /// 选择微博
        /// </summary>
        public Action<Status> Selected { get; set; }

        /// <summary>
        ///弹出菜单操作
        /// </summary>
        public Action<Status, EMenuItemAction> MenuItemClick { get; set; }

        /// <summary>
        /// 获取最新之后的回调
        /// </summary>
        public Action GotLastest { get; set; }

        #endregion 本控件事件

        /// <summary>
        /// 构造函数
        /// </summary>
        public StatusList()
        {
            if (!DesignerProperties.IsInDesignTool)
            {
                InitializeComponent();
            }

            TouchPanel.EnabledGestures = GestureType.VerticalDrag;
        }

        /// <summary>
        /// 获取微博数据结束
        /// </summary>
        /// <param name="tweets"></param>
        private void GetTimelineEnd(ICollection<Status> tweets)
        {
            IsPolling = false;

            isVerticalDrag = false;

            LastPollTime = DateTime.Now;

            Dispatcher.BeginInvoke(() =>
                                       {
                                           var data = (IEnumerable<Status>)FanListBox.ItemsSource;

                                           if (PollType == EPollType.NextPage)
                                           {
                                               data = data.Concat(tweets).ToList();
                                           }
                                           else if (PollType == EPollType.Lastest)
                                           {
                                               tweets = tweets.Where(t => t.Rawid > FirstRawId).ToList();
                                               if (tweets.Count > 0)
                                               {
                                                   data = tweets.Concat(data).ToList();
                                               }
                                           }
                                           else
                                           {
                                               if (tweets != null && tweets.Count > 0)
                                               {
                                                   IsInited = true;
                                               }
                                               data = tweets;
                                           }

                                           if (data != null && data.Any())
                                           {
                                               Status lastOrDefault = data.LastOrDefault();
                                               if (lastOrDefault != null)
                                                   LastRawId = lastOrDefault.Rawid;

                                               Status firstOrDefault = data.FirstOrDefault();
                                               if (firstOrDefault != null)
                                                   FirstRawId = firstOrDefault.Rawid;

                                               Status orDefault = data.FirstOrDefault();
                                               if (orDefault != null)
                                                   FirstId = orDefault.Id;

                                               Status last = data.LastOrDefault();
                                               if (last != null)
                                                   LastId = last.Id;
                                           }

                                           if (FanListBox.ItemTemplate == null)
                                           {
                                               if (ShowType == EShowType.Full)
                                               {
                                                   FanListBox.ItemTemplate =
                                                       (DataTemplate)Resources["FullFanListItemTemplate"];
                                               }
                                               else if (ShowType == EShowType.Reply)
                                               {
                                                   FanListBox.ItemTemplate =
                                                       (DataTemplate)Resources["ReplayListItemTemplate"];
                                               }
                                               else
                                               {
                                                   FanListBox.ItemTemplate =
                                                       (DataTemplate)Resources["SimpleFanListItemTemplate"];
                                               }
                                           }
                                           //处理界面提示

                                           FanListBox.ItemsSource = data;

                                           FanListBox.UpdateLayout();



                                           if (AfterLoadedCallback != null)
                                           {
                                               AfterLoadedCallback(tweets);
                                           }

                                           if ((PollType == EPollType.Lastest || PollType == EPollType.Default) &&
                                               GotLastest != null)
                                           {
                                               GotLastest();
                                           }
                                       });
        }

        private void MenuItem_Reply(object sender, RoutedEventArgs e)
        {
            var m = (MenuItem)sender;
            if (m != null)
            {
                Status t = GetMenuItemTweet(m.Tag.ToString());
                if (t != null)
                {
                    if (MenuItemClick != null)
                    {
                        MenuItemClick(t, EMenuItemAction.Reply);
                    }
                }
            }
        }

        private void MenuItem_Forward(object sender, RoutedEventArgs e)
        {
            var m = (MenuItem)sender;
            if (m != null)
            {
                Status t = GetMenuItemTweet(m.Tag.ToString());
                if (t != null)
                {
                    if (MenuItemClick != null)
                    {
                        MenuItemClick(t, EMenuItemAction.ReAdd);
                    }
                }
            }
        }

        private void MenuItem_Favorite(object sender, RoutedEventArgs e)
        {
            var m = (MenuItem)sender;
            if (m != null)
            {
                Status t = GetMenuItemTweet(m.Tag.ToString());
                if (t != null)
                {
                    if (MenuItemClick != null)
                    {
                        MenuItemClick(t, EMenuItemAction.Favorite);
                    }
                }
            }
        }

        private void MenuItem_Comment(object sender, RoutedEventArgs e)
        {
            var m = (MenuItem)sender;
            if (m != null)
            {
                Status t = GetMenuItemTweet(m.Tag.ToString());
                if (t != null)
                {
                    if (MenuItemClick != null)
                    {
                        MenuItemClick(t, EMenuItemAction.Comment);
                    }
                }
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            FanListBox.IsEnabled = false;
        }

        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            FanListBox.IsEnabled = true;
        }

        /// <summary>
        /// 获取弹出菜单所处的微博位置
        /// </summary>
        /// <param name="tweetId"></param>
        /// <returns></returns>
        private Status GetMenuItemTweet(string tweetId)
        {
            if (!string.IsNullOrWhiteSpace(tweetId))
            {
                var data = (IEnumerable<Status>)FanListBox.ItemsSource;
                if (data != null)
                {
                    return data.FirstOrDefault(t => t.Id == tweetId);
                }
            }
            return null;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                var image = (Image)sender;
                var photo = image.Tag as Photo;
                PhoneApplicationService.Current.State[Const.Imgobj] = photo;
                var app = Application.Current as App;
                if (app != null)
                    app.RootFrame.Navigate(new Uri("/ImageBrowse.xaml", UriKind.Relative));
            }
        }

        private void FanListBox_LayoutUpdated(object sender, EventArgs e)
        {
            if ((listBox_ScrollViewer != null) && IsCanPollData())
            {
                bool flag = listBox_ScrollViewer.VerticalOffset > lastListBoxScrollableVerticalOffset;
                lastListBoxScrollableVerticalOffset = listBox_ScrollViewer.VerticalOffset;
                if ((listBox_ScrollViewer.VerticalOffset >= (listBox_ScrollViewer.ScrollableHeight - 2.0)) && flag)
                {
                    GetNextPage();
                }
            }
            else
            {
                listBox_ScrollViewer = ControlHelper.FindChildOfType<ScrollViewer>(FanListBox);
            }
        }

        #region 操作事件

        /// <summary>
        /// 手势是否是垂直拖拽
        /// </summary>
        private bool isVerticalDrag { get; set; }

        /// <summary>
        /// 选择查看的微博
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FanListBox_Tap(object sender, GestureEventArgs gestureEventArgs)
        {
            if (Selected != null)
            {
                Selected((Status)FanListBox.SelectedItem);
            }
        }

        ///// <summary>
        ///// 手势结束
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void GestureListener_GestureCompleted(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        //{
        //    if (isVerticalDrag || AppSetting.HightDragSensitivity)
        //    {
        //        var scrollviewer = AppSetting.FindChildOfType<ScrollViewer>(FanListBox);
        //        if (scrollviewer == null)
        //        {
        //            return;
        //        }
        //        //到底部
        //        if (Math.Abs(scrollviewer.VerticalOffset - scrollviewer.ScrollableHeight) < 2)
        //        {
        //            GetNextPage();
        //        }
        //        //顶部
        //        else if (scrollviewer.VerticalOffset < 0.000001)
        //        {
        //            GetLastest();
        //        }
        //    }
        //}

        ///// <summary>
        ///// 手势开始
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void GestureListener_GestureBegin(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        //{
        //    while (TouchPanel.IsGestureAvailable)
        //    {
        //        GestureSample gs = TouchPanel.ReadGesture();
        //        if (gs.GestureType == GestureType.VerticalDrag)
        //        {
        //            isVerticalDrag = true;
        //        }
        //    }
        //}

        #endregion 操作事件

        #region 公共方法

        /// <summary>
        /// 初始化控件，首页和提及调用此方法
        /// </summary>
        /// <param name="beforeLoading"></param>
        /// <param name="afterCallback"></param>
        public void Init(Action beforeLoading = null, Action<object> afterCallback = null)
        {
            if (_statusApi == null)
                _statusApi = new Statuses(OauthHelper.OAuth());

            if (IsInited)
            {
                return;
            }

            if (beforeLoading != null)
            {
                BeforeLoadingCallback = beforeLoading;
                beforeLoading();
            }

            if (afterCallback != null)
            {
                AfterLoadedCallback = afterCallback;
            }

            PollType = EPollType.Default;

            IsPolling = true;

            switch (Timeline)
            {
                case ETimeline.Home:
                    _statusApi.GetHomeTimeLine(GetTimelineEnd, null, null, null, AppSetting.PageCount, 0, "default");
                    break;

                case ETimeline.Reply:
                    _statusApi.GetReplies(GetTimelineEnd, null, null, AppSetting.PageCount, 0, "default");
                    break;

                case ETimeline.Public:
                    _statusApi.GetPublicTimeline(GetTimelineEnd, AppSetting.PageCount, null, null, "default");
                    break;
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            IsInited = false;

            PollType = EPollType.Default;

            Init(BeforeLoadingCallback, AfterLoadedCallback);
        }

        #endregion 公共方法

        #region 私有方法

        /// <summary>
        /// 下一页
        /// </summary>
        private void GetNextPage()
        {
            if (!IsCanPollData()) return;

            if (BeforeLoadingCallback != null)
            {
                BeforeLoadingCallback();
            }

            IsPolling = true;

            PollType = EPollType.NextPage;

            switch (Timeline)
            {
                case ETimeline.Home:
                    _statusApi.GetHomeTimeLine(GetTimelineEnd, null, null, LastId, AppSetting.PageCount, null, "default");
                    break;

                case ETimeline.Reply:
                    _statusApi.GetReplies(GetTimelineEnd, null, LastId, AppSetting.PageCount, null, "default");
                    break;

                case ETimeline.Public:
                    _statusApi.GetPublicTimeline(GetTimelineEnd, AppSetting.PageCount, null, LastId, "default");
                    break;
            }


        }

        /// <summary>
        /// 获取最新
        /// </summary>
        private void GetLastest()
        {
            if (!IsCanPollData()) return;

            if (BeforeLoadingCallback != null)
            {
                BeforeLoadingCallback();
            }

            IsPolling = true;

            PollType = EPollType.Lastest;

            switch (Timeline)
            {
                case ETimeline.Home:
                    _statusApi.GetHomeTimeLine(GetTimelineEnd, null, FirstId, null, AppSetting.PageCount, null,
                                               "default");
                    break;

                case ETimeline.Reply:
                    _statusApi.GetReplies(GetTimelineEnd, FirstId, null, AppSetting.PageCount, null, "default");
                    break;

                case ETimeline.Public:
                    _statusApi.GetPublicTimeline(GetTimelineEnd, AppSetting.PageCount, FirstId, null, "default");
                    break;
            }


        }

        /// <summary>
        /// 可以请求拉取数据
        /// </summary>
        /// <returns></returns>
        private bool IsCanPollData()
        {
            return !IsPolling;
        }

        #endregion 私有方法
    }
}