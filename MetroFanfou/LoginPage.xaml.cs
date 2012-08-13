using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using FanFou.SDK;
using MetroFanfou.common;
using MetroFanfou.Helper;
using Microsoft.Phone.Controls;

namespace MetroFanfou
{
    public partial class LoginPage : PhoneApplicationPage
    {
        private readonly OAuth _oauth = OauthHelper.OAuth();

        private const string DefaultHtmlFormat = "<html><head><title>Altman</title></head><body style='text-align:center;background:#FFF;color:#000;'><br/><br/><br/>{0}</body></html>";

        private DispatcherTimer _timer;

        /// <summary>
        /// 页面加载超时时间
        /// </summary>
        private const int Timerout = 6;

        private bool _isLoaded = false;

        /// <summary>
        /// 默认加载中页面内容
        /// </summary>
        private static string DefaultHtml
        {
            get
            {
                return BuildHtml("正在装载饭否微博授权页...");
            }
        }

        private static string BuildHtml(string text)
        {
            return ConvertExtendedAscii(string.Format(DefaultHtmlFormat, text));
        }

        public LoginPage()
        {
            InitializeComponent();
            //this.ApplicationBar.BackgroundColor = (Color)App.Current.Resources["ApplicationBarBackgroundColor"];
        }

        /// <summary>
        /// 中文乱码
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ConvertExtendedAscii(string html)
        {
            string retVal = "";
            char[] s = html.ToCharArray();

            foreach (char c in s)
            {
                if (Convert.ToInt32(c) > 127)
                    retVal += "&#" + Convert.ToInt32(c) + ";";
                else
                    retVal += c;
            }

            return retVal;
        }

        /// <summary>
        /// 页面导航
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                base.OnNavigatedTo(e);

                if (!OauthHelper.IsVerified)
                {
                    var size = App.Current.RootVisual.RenderSize;
                    webBrowser.Width = size.Width;
                    webBrowser.Height = size.Height;
                    webBrowser.NavigateToString(DefaultHtml);
                    _oauth.GetRequestToken(WebBrowserVerifier);
                    //检查超时
                    _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(Timerout) };
                    _timer.Tick += timer_Tick;
                    _timer.Start();
                }
                else
                    NavigateTo();
            }
            catch (Exception ex)
            {
                (new Helper.MessageBoxHelper()).Show(ex.Message.ToString(), "出错了", this.LayoutRoot);
            }
        }

        /// <summary>
        /// 1. 获取验证Token，调用浏览器进行授权
        /// </summary>
        /// <param name="token"></param>
        private void WebBrowserVerifier(string token)
        {
            try
            {
                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(token))
                {
                    OauthHelper.Token = _oauth.Token;
                    OauthHelper.TokenSecret = _oauth.TokenSecret;
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        _timer.Stop();
                        var size = App.Current.RootVisual.RenderSize;
                        webBrowser.Width = size.Width;
                        webBrowser.Height = size.Height;
                        webBrowser.IsScriptEnabled = true;
                        webBrowser.LoadCompleted += WebBrowserLoadCompleted;
                        webBrowser.Navigate(new Uri("http://m.fanfou.com/oauth/authorize?oauth_token=" + OauthHelper.Token, UriKind.Absolute));
                        _timer.Start();
                    });
                }
                else
                {
                    throw new Exception("请求授权失败，请检查网络或稍后重试！");
                }
            }
            catch (Exception ex)
            {
                (new Helper.MessageBoxHelper()).Show(ex.Message.ToString(), "出错了", this.LayoutRoot);
                Dispatcher.BeginInvoke(() => webBrowser.NavigateToString(BuildHtml(ex.Message.ToString())));
            }
        }

        /// <summary>
        /// 2. 授权，浏览器加载完成，检查内容中是否含有授权码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowserLoadCompleted(object sender, NavigationEventArgs e)
        {
            _isLoaded = true;
            _timer.Stop();
            var s = webBrowser.SaveToString();
            var reg = new Regex(@"你同意了");
            var match = reg.Match(s);
            if (!string.IsNullOrEmpty(match.Value))
            {
                _oauth.GetAccessToken("", AccessTokenEnd);
            }
        }

        ///<summary>
        ///3.授权成功， 获取Access Token结束
        /// </summary>
        /// <param name="name"></param>
        private void AccessTokenEnd(string value)
        {
            try
            {
                OauthHelper.Token = _oauth.Token;
                OauthHelper.TokenSecret = _oauth.TokenSecret;
                OauthHelper.IsVerified = true;
                NavigateTo();
            }
            catch (Exception ex)
            {
                (new MessageBoxHelper()).Show(ex.Message.ToString(CultureInfo.InvariantCulture), "出错了", this.LayoutRoot);
                Dispatcher.BeginInvoke(() => webBrowser.NavigateToString(BuildHtml(ex.Message.ToString(CultureInfo.InvariantCulture))));
            }
        }

        /// <summary>
        /// 跳转页面
        /// </summary>
        private void NavigateTo()
        {
            try
            {
                //var ipHelper = new IpAddressHelper();
                //ipHelper.GetIpAddress((string rs) =>
                //{
                //    AppSetting.IpAddress = rs;
                //});
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    var url = "/MainPage.xaml";
                    if (NavigationContext.QueryString.ContainsKey(AppSetting.TileDirectUriKey))
                    {
                        url = NavigationContext.QueryString[AppSetting.TileDirectUriKey];
                    }
                    NavigationService.Navigate(new Uri(url, UriKind.Relative));
                });
            }
            catch (Exception ex)
            {
                (new MessageBoxHelper()).Show(ex.Message.ToString(), "出错了", this.LayoutRoot);
            }
        }

        /// <summary>
        /// 第一个界面，按下后退键，市场认证需要
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneApplicationPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var rs = MessageBox.Show("您确定要退出微博客户端吗？", "退出确认", MessageBoxButton.OKCancel);
            if (rs == MessageBoxResult.OK)
            {
                // App.Quit();
                return;
            }
            e.Cancel = true;
        }

        /// <summary>
        /// 检测是否超时
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        public void timer_Tick(object obj, EventArgs e)
        {
            if (!_isLoaded)
            {
                (new Helper.MessageBoxHelper()).Show("加载授权页面超时！\r\n请检查网络，刷新或退出重试！", "出错了", this.LayoutRoot);
                _timer.Stop();
            }
        }

        /// <summary>
        /// 刷新重载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReload_Click(object sender, EventArgs e)
        {
            _isLoaded = false;
            _timer.Stop();
            webBrowser.NavigateToString(DefaultHtml);
            _oauth.GetRequestToken(WebBrowserVerifier);
            _timer.Start();
            //Deployment.Current.Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/Oauth.xaml", UriKind.Relative)));
        }
    }
}