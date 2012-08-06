using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using FanFou.SDK.API;
using FanFou.SDK.Objects;
using MetroFanfou.common;

namespace MetroFanfou.Controls
{
    public partial class TweetDetail : UserControl
    {
        #region 私有变量

        private FanFou.SDK.API.Statuses statusAPI = new Statuses(OauthHelper.OAuth());
        private Status _status;

        #endregion

        #region 公共变量
        /// <summary>
        /// 选择微博
        /// </summary>
        public Action<string> SelectedSource { get; set; }



        #endregion

        #region 控件事件

        /// <summary>
        /// 加载后回调
        /// </summary>
        private Action<Status> AfterLoadedCallback { get; set; }

        #endregion

        public TweetDetail()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载status的详细信息
        /// </summary>
        /// <param name="tweetId"></param>
        /// <param name="beforeLoading"> </param>
        /// <param name="afterCallback"> </param>
        public void LoadTweet(string tweetId, Action beforeLoading = null, Action<Status> afterCallback = null)
        {
            if (beforeLoading != null)
            {
                beforeLoading();
            }
            if (afterCallback != null)
            {
                AfterLoadedCallback = afterCallback;
            }

            statusAPI.ShowStatus(GetStatusCallback, tweetId);
        }

        /// <summary>
        /// 加载回调
        /// </summary>
        /// <param name="rs"></param>
        private void GetStatusCallback(Status rs)
        {
            Dispatcher.BeginInvoke(() =>
            {
                _status = rs;
                TweetDetailLayoutRoot.DataContext = rs;


                rtRichTweetBox.SetContent(rs.Text);

                if (AfterLoadedCallback != null)
                {
                    AfterLoadedCallback(rs);
                }
            });
        }

        private void Grid_Tap(object sender, GestureEventArgs e)
        {

        }

        /// <summary>
        /// 点击图片打开图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TweetImageTap(object sender, GestureEventArgs e)
        {

        }
    }
}
