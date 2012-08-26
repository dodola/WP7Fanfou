using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MetroFanfou.Helper;
using Microsoft.Phone.Controls;

namespace MetroFanfou.Controls
{
    /// <summary>
    /// 主要功能，存放一些全局方法
    /// </summary>
    public class BasePage : PhoneApplicationPage
    {
        /// <summary>
        /// 指示网络是否繁忙
        /// </summary>
        protected Boolean IsWebBusy { get; set; }

        protected void StartSystemTrayProgress()
        {
            this.Dispatcher.BeginInvoke(() =>
                                            {
                                                GlobalLoading.Instance.IsLoading = true;
                                            });
        }

        protected void StopSystemTrayProgress()
        {
            Dispatcher.BeginInvoke(() =>
                                       {
                                           GlobalLoading.Instance.IsLoading = false;
                                       });
        }
        /// <summary>
        /// 全局加载成功方法
        /// </summary>
        protected void OnLoadingSuccess()
        {

        }
    }
}
