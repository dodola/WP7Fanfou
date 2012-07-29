using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MetroFanfou;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace MetroFanfou
{
    public partial class Setting : PhoneApplicationPage
    {
        bool isPictureQualityLoaded = false;
        bool isCheckUpdateStatusLoaded = false;
        bool isExitConfirmLoaded = false;
        bool isScheduleLoad = false;

        /// <summary>
        /// 选择的index
        /// </summary>
        const string SelectIndexKey = "index";

        string tileUriParam = AppSetting.TileDirectUriKey + "=/Tweet/Add.xaml?"+AppSetting.IsQuickTweetParameterKey+"=1";

        public Setting()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            tbAppName.Text = AppSetting.AppName + " "+AppSetting.AppVersion;

            var picQuality = new List<KeyValuePair<string, int>>();
            picQuality.Add(new KeyValuePair<string, int>("一般(50%)", 50));
            picQuality.Add(new KeyValuePair<string, int>("中等(60%)", 60));
            picQuality.Add(new KeyValuePair<string, int>("较高(80%)", 80));
            picQuality.Add(new KeyValuePair<string, int>("原图(100%)", 100));
            lpPictureQuality.ItemsSource = picQuality;

            var pq = picQuality.Where(k => k.Value == AppSetting.ImageQuality).FirstOrDefault();
            lpPictureQuality.SelectedItem = new KeyValuePair<string, int>(pq.Key, pq.Value);


            var timespan = new List<KeyValuePair<string, int>>();
            timespan.Add(new KeyValuePair<string, int>("不检查", 0));
            timespan.Add(new KeyValuePair<string, int>("每5秒", 5000));
            timespan.Add(new KeyValuePair<string, int>("每10秒", 10000));
            timespan.Add(new KeyValuePair<string, int>("每20秒", 20000));
            timespan.Add(new KeyValuePair<string, int>("每30秒", 30000));
            lpCheckUpdateStatus.ItemsSource = timespan;

            var ts = timespan.Where(k => k.Value == AppSetting.CheckUpdateSecondSpan).FirstOrDefault();
            lpCheckUpdateStatus.SelectedItem = new KeyValuePair<string, int>(ts.Key, ts.Value);


            var confirmData = new List<KeyValuePair<string, bool>>();
            confirmData.Add(new KeyValuePair<string, bool>("否", false));
            confirmData.Add(new KeyValuePair<string, bool>("是", true));
            lpExitConfirm.ItemsSource = confirmData;

            var cf = confirmData.Where(k => k.Value == AppSetting.IsExitConfirm).FirstOrDefault();
            lpExitConfirm.SelectedItem = new KeyValuePair<string, bool>(cf.Key, cf.Value);


            var schedulespan = new List<KeyValuePair<string, bool>>();
            schedulespan.Add(new KeyValuePair<string, bool>("否", false));
            schedulespan.Add(new KeyValuePair<string, bool>("是", true));

            lpSchedule.ItemsSource = schedulespan;

            var ss = schedulespan.Where(k => k.Value == AppSetting.IsScheduledAgent).FirstOrDefault();
            lpSchedule.SelectedItem = new KeyValuePair<string, bool>(ss.Key, ss.Value);

            cbHubTile.IsChecked=ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(tileUriParam))!=null;

        

            if (NavigationContext.QueryString.ContainsKey(SelectIndexKey))
            {
                var index = int.Parse(NavigationContext.QueryString[SelectIndexKey]);
                ptSettingPivot.SelectedIndex = index;
            }

            //tbIpAddr.Text = "IP地址："+AppSetting.IpAddress;

            /*处理主题*/
            themeListBox.ItemsSource = (new Helper.ThemeHelper()).ThemeThumbUri;
        }

        /// <summary>
        /// 图片质量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lpPictureQuality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isPictureQualityLoaded)
            {
                var val = (KeyValuePair<string, int>)lpPictureQuality.SelectedItem;
                AppSetting.ImageQuality = val.Value;
            }
        }

        /// <summary>
        /// 主界面查新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lpCheckUpdateStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isCheckUpdateStatusLoaded)
            {
                var val = (KeyValuePair<string, int>)lpCheckUpdateStatus.SelectedItem;
                AppSetting.CheckUpdateSecondSpan = val.Value;
            }
        }

        /// <summary>
        /// 退出确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lpExitConfirm_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isExitConfirmLoaded)
            {
                var val = (KeyValuePair<string, bool>)lpExitConfirm.SelectedItem;
                AppSetting.IsExitConfirm = val.Value;
            }
        }

        /// <summary>
        /// 后台查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lpSchedule_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isScheduleLoad) {
                var val = (KeyValuePair<string, bool>)lpSchedule.SelectedItem;
                AppSetting.IsScheduledAgent = val.Value;
            }
        }

        private void lpPictureQuality_Loaded(object sender, RoutedEventArgs e)
        {
            isPictureQualityLoaded = true;
        }

        private void lpCheckUpdateStatus_Loaded(object sender, RoutedEventArgs e)
        {
            isCheckUpdateStatusLoaded = true;
        }

        private void lpExitConfirm_Loaded(object sender, RoutedEventArgs e)
        {
            isExitConfirmLoaded = true;
        }

        private void lpSchedule_Loaded(object sender, RoutedEventArgs e)
        {
            isScheduleLoad = true;
        }

        private void cbHubTile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var ischecked = cbHubTile.IsChecked;
            var tiles=  ShellTile.ActiveTiles;           
            var TileToFind = tiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(tileUriParam));
            if (ischecked == true)
            {
                if (TileToFind == null)
                {
                    StandardTileData NewTileData = new StandardTileData
                    {
                        BackgroundImage = new Uri("/Resource/Images/AddTweetTileBackground.png", UriKind.Relative),
                        Title = "新微博",
                        BackTitle = "腾讯微博",
                        BackContent = "快速启动\r\n发布微博"
                    };
                    ShellTile.Create(new Uri("/Oauth.xaml?"+tileUriParam, UriKind.Relative), NewTileData);
                }
            }
            else {
                if (TileToFind != null) {
                    TileToFind.Delete();
                }
            }
        }

     

        private void btnIssue_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/Tweet/Add.xaml?{0}={1}&{2}={3}&{4}={5}", 
                AppSetting.AddTweetTypeParameterKey, 
                Helper.EAddTweetType.Mention.GetHashCode(), 
                AppSetting.AddTweetMetaParameterKey, 
                AppSetting.AuthorAccount,
                AppSetting.AddTweetSubMetaParameterKey,
                HttpUtility.UrlEncode("#"+AppSetting.AppName+"#") 
                ), UriKind.Relative));
        }

        private void btnWebSite_Click(object sender, RoutedEventArgs e)
        {
            var webBrowserTask = new WebBrowserTask
            {
                Uri = new Uri("http://topming.com/app/altman/")
            };
            webBrowserTask.Show();
        }

        private void tbEmail_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var emailTask = new EmailComposeTask();
            emailTask.To = "hwangzhiming@gmail.com";
            emailTask.Subject = AppSetting.AppName;
            emailTask.Show();
        }

        private void themeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var val = (KeyValuePair<string, string>)((ListBox)sender).SelectedValue;
            AppSetting.ThemeName = val.Key;
            (new Helper.MessageBoxHelper()).Show("主题设置成功，重启应用生效。", "提示", this.LayoutRoot);
        }
    }
}