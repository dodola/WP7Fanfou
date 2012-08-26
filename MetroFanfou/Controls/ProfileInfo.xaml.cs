using System.Windows.Controls;
using FanFou.SDK.Objects;

namespace MetroFanfou.Controls
{
    public partial class ProfileInfo : UserControl
    {
        public ProfileInfo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绑定用户信息
        /// </summary>
        /// <param name="userInfo"></param>
        public void BindInfo(User userInfo)
        {
            Dispatcher.BeginInvoke(() => { ProfileInfoLayoutRoot.DataContext = userInfo; });
        }
    }
}