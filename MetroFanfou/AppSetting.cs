/////////////////////////////////////////////////////////
// Author   : huangzhiming
// Date     : 11/18/2011 9:32:32 AM
// Usage    : 应用设置
/////////////////////////////////////////////////////////

using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using MetroFanfou.common;

namespace MetroFanfou
{
    public class AppSetting
    {




        private static string LoginAccountName;

        /// <summary>
        /// 微博最大字数
        /// </summary>
        public static int MaxTweetWordCount = 140;

        /// <summary>
        /// 在线帮助文档
        /// </summary>
        public const string AppOnlineHelpPage = "http://www.topming.com/app/altman/help.html";

        /// <summary>
        /// IP地址
        /// </summary>
        public static string IpAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 高敏感翻页
        /// </summary>
        public static bool HightDragSensitivity
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 应用作者帐号
        /// </summary>
        public const string AuthorAccount = "hwangzhiming";

        /// <summary>
        /// 应用名称
        /// </summary>
        public const string AppName = "altman";

        /// <summary>
        /// 应用版本
        /// </summary>
        public const string AppVersion = "v2.1";

        /// <summary>
        /// 后台线程检查新数据的时间间隔(毫秒),0表示不检查
        /// </summary>
        private static int _CheckUpdateSecondSpan = -1;

        public static int CheckUpdateSecondSpan
        {
            get
            {
                if (_CheckUpdateSecondSpan > -1)
                {
                    return _CheckUpdateSecondSpan;
                }
                var s = Isolated.Get(Helper.IsolatedHelper.CheckUpdateCountSpanKey);
                if (s != null)
                {
                    _CheckUpdateSecondSpan = (int)s;
                    return _CheckUpdateSecondSpan;
                }
                return 0;
            }
            set
            {
                _CheckUpdateSecondSpan = value;
                Isolated.Set(Helper.IsolatedHelper.CheckUpdateCountSpanKey, value);
            }
        }

        /// <summary>
        /// 上传的图片质量
        /// </summary>
        public static int ImageQuality
        {
            get
            {
                var s = Isolated.Get(Helper.IsolatedHelper.PictureQualitykey);
                if (s != null)
                {
                    return (int)s;
                }
                return 60;
            }
            set
            {
                Isolated.Set(Helper.IsolatedHelper.PictureQualitykey, value);
            }
        }

        /// <summary>
        /// 退出确认
        /// </summary>
        public static bool IsExitConfirm
        {
            get
            {
                var s = Isolated.Get(Helper.IsolatedHelper.ExitAppConfirmKey);
                if (s != null)
                {
                    return (bool)s;
                }
                return false;
            }
            set
            {
                Isolated.Set(Helper.IsolatedHelper.ExitAppConfirmKey, value);
            }
        }

        /// <summary>
        /// 是否在应用关闭时启动后台进程检查新微博
        /// </summary>
        public static bool IsScheduledAgent
        {
            get
            {
                var s = Isolated.Get(Helper.IsolatedHelper.ScheduledAgentKey);
                if (s != null)
                {
                    return (bool)s;
                }
                return false;
            }
            set
            {
                Isolated.Set(Helper.IsolatedHelper.ScheduledAgentKey, value);
            }
        }

        /// <summary>
        /// AccountName账户名
        /// </summary>
        public static string AccountName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(LoginAccountName))
                {
                    return LoginAccountName;
                }
                var val = Isolated.Get(Helper.IsolatedHelper.AccountNameKey);
                if (val != null)
                {
                    LoginAccountName = (string)val;
                    return LoginAccountName;
                }
                return string.Empty;
            }
            set
            {
                LoginAccountName = value;
                Isolated.Set(Helper.IsolatedHelper.AccountNameKey, value);
            }
        }

        /// <summary>
        /// 主题
        /// </summary>
        public static string ThemeName
        {
            get
            {
                var val = Isolated.Get(Helper.IsolatedHelper.ThemeKey);
                if (val != null)
                {
                    return (string)val;
                }
                return "Blue";
            }
            set
            {
                Isolated.Set(Helper.IsolatedHelper.ThemeKey, value);
            }
        }

        public static int PageCount
        {
            get
            {

                var val = Isolated.Get(Helper.IsolatedHelper.PageCount);
                int count = 10;
                if (val != null)
                    count = Convert.ToInt32(val);

                return count;

            }
            set
            {
                Isolated.Set(Helper.IsolatedHelper.PageCount, value > 40 ? 10 : value);
            }

        }

        public static string LastId
        {
            get
            {
                var val = Isolated.Get(Helper.IsolatedHelper.LastId);

                return val == null ? null : val.ToString();
            }
            set
            {
                Isolated.Set(Helper.IsolatedHelper.LastId, value);
            }
        }

        #region 页面传值的查询key

        /// <summary>
        /// 详细页ID的查询KEY
        /// </summary>
        public static string TweetIdParameterKey
        {
            get
            {
                return "tweetid";
            }
        }

        /// <summary>
        /// 详细页视频地址的查询KEY
        /// </summary>
        public static string TweetVideoParameterKey
        {
            get
            {
                return "tweetvideo";
            }
        }

        /// <summary>
        /// 详细页图片地址的查询KEY
        /// </summary>
        public static string TweetImageParameterKey
        {
            get
            {
                return "tweetimage";
            }
        }

        /// <summary>
        /// 用户名查询KEY
        /// </summary>
        public static string UserNameParameterKey
        {
            get
            {
                return "name";
            }
        }

        /// <summary>
        /// 添加微博的类型查询KEY，新微博，转播，回复
        /// </summary>
        public static string AddTweetTypeParameterKey
        {
            get
            {
                return "type";
            }
        }

        /// <summary>
        /// 添加微博时带的扩展信息查询KEY，如@某人时是某人的name
        /// </summary>
        public static string AddTweetMetaParameterKey
        {
            get
            {
                return "meta";
            }
        }

        /// <summary>
        /// 添加微博时带的扩展信息2查询KEY，如@某人时是微博预置的内容
        /// </summary>
        public static string AddTweetSubMetaParameterKey
        {
            get
            {
                return "meta2";
            }
        }

        /// <summary>
        /// 是否是快速发微，当从快速发微瓦片进入是，add.xaml可以检查此参数
        /// </summary>
        public static string IsQuickTweetParameterKey
        {
            get
            {
                return "quickadd";
            }
        }

        /// <summary>
        /// 瓦片启动跳转页面的查询key，在Oauth验证后跳转
        /// </summary>
        public static string TileDirectUriKey
        {
            get
            {
                return "nav_to";
            }
        }

        #endregion

        #region 公共方法
        /// <summary>
        ///获取控件的子控件
        /// </summary>
        /// <typeparam name="T">子控件类</typeparam>
        /// <param name="root">父控件</param>
        /// <returns></returns>
        public static T FindChildOfType<T>(DependencyObject root) where T : class
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                DependencyObject current = queue.Dequeue();
                for (int i = VisualTreeHelper.GetChildrenCount(current) - 1; 0 <= i; i--)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    var typedChild = child as T;
                    if (typedChild != null)
                    {
                        return typedChild;
                    }
                    queue.Enqueue(child);
                }
            }
            return null;
        }
        #endregion
    }
}
