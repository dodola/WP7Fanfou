using System;
using System.Windows.Controls;
using System.Windows.Input;
using FanFou.SDK.Objects;

namespace MetroFanfou.Controls
{
    public partial class UserAvatar : UserControl
    {
        public UserAvatar()
        {
            InitializeComponent();
        }

        /// <summary>  
        /// 绑定用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        public void BindInfo(User userInfo)
        {
            Dispatcher.BeginInvoke(() => { LayoutRoot.DataContext = userInfo; });
        }

        private void LayoutRootTap(object sender, GestureEventArgs e)
        {
            if (TapAvatar != null)
            {
                TapAvatar(tbUserName.Text);
            }
        }

        #region 公共属性

        /// <summary>
        /// 点击事件
        /// </summary>
        public Action<string> TapAvatar { get; set; }

        #endregion
    }
}