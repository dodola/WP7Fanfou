using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using FanFou.SDK.Objects;
using Microsoft.Xna.Framework.Input.Touch;

namespace MetroFanfou.Controls
{
    public partial class FansList : UserControl
    {
        public FansList()
        {
            InitializeComponent();
            TouchPanel.EnabledGestures = GestureType.VerticalDrag;
        }

        #region 私有属性

        public enum EDataType
        {
            /// <summary>
            /// 听众
            /// </summary>
            Fans,
            /// <summary>
            /// 收听
            /// </summary>
            Idol
        }

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

        /// <summary>
        /// 是否正在拉取数据
        /// </summary>
        private bool isPolling { get; set; }
        /// <summary>
        /// 是否初始化，加载数据
        /// </summary>
        private bool isInited { get; set; }

        /// <summary>
        /// 指定的用户帐号
        /// </summary>
        private string account { get; set; }

        /// <summary>
        /// 翻页开始位置
        /// </summary>
        private int startIndex { get; set; }

        /// <summary>
        /// 加载数据的类型
        /// </summary>
        private EPollType pollType { get; set; }

        #endregion

        #region 自定义属性
        /// <summary>
        /// 依赖项，显示的内容类型
        /// </summary>
        public static readonly DependencyProperty TimelineProperty = DependencyProperty.Register("DataType",
            typeof(EDataType),
            typeof(FansList),
            new PropertyMetadata(EDataType.Fans));



        /// <summary>
        /// 内容类型
        /// </summary>
        [Description("内容类型")]
        public EDataType DataType
        {
            get { return (EDataType)this.GetValue(TimelineProperty); }
            set { this.SetValue(TimelineProperty, value); }
        }


        #endregion

        #region 本控件事件

        private Action beforeLoadingCallback { get; set; }

        private Action<object> afterLoadedCallback { get; set; }

        /// <summary>
        /// 选择微博
        /// </summary>
        public Action<User> Selected { get; set; }

        /// <summary>
        /// 获取最新之后的回调
        /// </summary>
        public Action GotLastest { get; set; }

        #endregion


        #region 操作事件

        /// <summary>
        /// 选择查看的微博
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TweetListBox_Tap(object sender, GestureEventArgs e)
        {
            if (Selected != null)
            {
                Selected((User)TweetListBox.SelectedItem);
            }
        }

        /// <summary>
        /// 手势是否是垂直拖拽
        /// </summary>
        private bool isVerticalDrag { get; set; }

        /// <summary>
        /// 手势结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GestureListener_GestureCompleted(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            if (isVerticalDrag || AppSetting.HightDragSensitivity)
            {
                var scrollviewer = AppSetting.FindChildOfType<ScrollViewer>(TweetListBox);
                if (scrollviewer == null)
                {
                    return;
                }
                //到底部
                if (Math.Abs(scrollviewer.VerticalOffset - scrollviewer.ScrollableHeight) < 2)
                {
                    GetNextPage();
                }
                ////顶部
                //else if (scrollviewer.VerticalOffset < 0.000001)
                //{
                //    GetLastest();
                //}
            }
        }
        /// <summary>
        /// 手势开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GestureListener_GestureBegin(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            while (TouchPanel.IsGestureAvailable)
            {
                GestureSample gs = TouchPanel.ReadGesture();
                if (gs.GestureType == GestureType.VerticalDrag)
                {
                    isVerticalDrag = true;
                }
            }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 初始化控件
        /// </summary>
        /// <param name="beforeLoading"></param>
        /// <param name="afterCallback"></param>
        public void Init(string name, Action beforeLoading = null, Action<object> afterCallback = null)
        {

            if (isInited)
            {
                return;
            }

            account = name;

            if (beforeLoading != null)
            {
                beforeLoadingCallback = beforeLoading;
                beforeLoading();
            }

            if (afterCallback != null)
            {
                afterLoadedCallback = afterCallback;
            }

            isPolling = true;

            pollType = EPollType.Default;

            switch (DataType)
            {
                //case EDataType.Fans: Dal.Friends.Instance.GetFanslist(account, 0, GetFansListEnd); break;
                //case EDataType.Idol: Dal.Friends.Instance.GetIdollist(account, 0, GetFansListEnd); break;
            }

        }


        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {

            isInited = false;

            Init(account, beforeLoadingCallback, afterLoadedCallback);

        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 下一页
        /// </summary>
        private void GetNextPage()
        {

            if (!this.IsCanPollData()) return;

            if (beforeLoadingCallback != null)
            {
                beforeLoadingCallback();
            }

            isPolling = true;

            pollType = EPollType.NextPage;

            switch (DataType)
            {
                //case EDataType.Fans: Dal.Friends.Instance.GetFanslist(account, startIndex, GetFansListEnd); break;
                //case EDataType.Idol: Dal.Friends.Instance.GetIdollist(account, startIndex, GetFansListEnd); break;
            }



            Dispatcher.BeginInvoke(() =>
            {
                if (!TweetListBox.ShowListFooter)
                {
                    TweetListBox.ShowListFooter = true;
                    TweetListBox.ScrollTo(TweetListBox.ListFooter);
                }
            });

        }

        /// <summary>
        /// 获取最新
        /// </summary>
        private void GetLastest()
        {

            if (!this.IsCanPollData()) return;

            if (beforeLoadingCallback != null)
            {
                beforeLoadingCallback();
            }

            isPolling = true;

            pollType = EPollType.Lastest;

            switch (DataType)
            {
                //case EDataType.Fans: Dal.Friends.Instance.GetFanslist(account, 0, GetFansListEnd); break;
                //case EDataType.Idol: Dal.Friends.Instance.GetIdollist(account, 0, GetFansListEnd); break;
            }

            Dispatcher.BeginInvoke(() =>
            {
                if (!TweetListBox.ShowListHeader)
                {
                    TweetListBox.ShowListHeader = true;
                }
            });
        }

        /// <summary>
        /// 可以请求拉取数据
        /// </summary>
        /// <returns></returns>
        private bool IsCanPollData()
        {
            return !isPolling;
        }

        #endregion

        /// <summary>
        /// 获取结束回调
        /// </summary>
        /// <param name="users"></param>
        private void GetFansListEnd(ICollection<User> users)
        {

            isPolling = false;

            isVerticalDrag = false;

            Dispatcher.BeginInvoke(() =>
            {
                var data = (IEnumerable<User>)TweetListBox.ItemsSource;

                if (pollType == EPollType.NextPage)
                {
                    data = data.Concat(users).ToList();
                }
                else
                {
                    if (users != null && users.Count > 0)
                    {
                        isInited = true;
                    }
                    data = users;
                }

                if (data != null && data.Count() > 0)
                {
                    startIndex = data.Count();
                }

                //处理界面提示

                TweetListBox.ItemsSource = data;

                TweetListBox.UpdateLayout();

                TweetListBox.ShowListFooter = false;

                TweetListBox.ShowListHeader = false;

                if (afterLoadedCallback != null)
                {
                    afterLoadedCallback(users);
                }

                if (pollType == EPollType.Default && GotLastest != null)
                {
                    GotLastest();
                }
            });
        }

    }
}
