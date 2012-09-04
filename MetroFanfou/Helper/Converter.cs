﻿/////////////////////////////////////////////////////////
// Author   : huangzhiming
// Date     : 8/17/2011 3:18:44 PM
// Usage    :
/////////////////////////////////////////////////////////

using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using FanFou.SDK.Objects;

namespace MetroFanfou.Helper
{
    /// <summary>
    /// 对象是否为空判断显隐属性
    /// </summary>
    public class ObjectToVisibleConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    /// <summary>
    /// 字符串是否为空判断显隐属性
    /// </summary>
    public class StringToVisibleConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return true;
            return string.IsNullOrEmpty(value.ToString()) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    /// <summary>
    /// 整型是否为0判断显隐属性，带参取反
    /// </summary>
    public class IntToVisibleConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = (long) value < 1;
            b = parameter == null ? b : !b;
            return b ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    /// <summary>
    /// 头像路径转换
    /// </summary>
    public class HeadUrlConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string size = parameter.ToString();
            return string.IsNullOrEmpty(value.ToString())
                       ? "/MetroFanfou;component/Resource/Images/noHead.png"
                       : value + "/" + (string.IsNullOrEmpty(size) ? "50" : size);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    /// <summary>
    /// 截取私信
    /// </summary>
    public class MessageListTextConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value.ToString();
            if (!string.IsNullOrWhiteSpace(str) && str.Length > 16)
            {
                str = str.Substring(0, 16) + "...";
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    /// <summary>
    /// 父微博内容
    /// </summary>
    public class SourceTweetContentConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var t = (Status) value;
                return string.Format("{0}: {1}", t.User.ScreenName, t.Text);
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    public class TimeToSpanConvert : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                const string format = "ddd MMM dd HH:mm:ss zzz yyyy";
                DateTime date;
                if (DateTime.TryParseExact(value.ToString(), format, CultureInfo.InvariantCulture,
                                           DateTimeStyles.None, out date))
                {
                    TimeSpan timeSpan = DateTime.Now.Subtract(date).Duration();

                    if ((int) timeSpan.TotalDays > 0)
                    {
                        return string.Format("{0}天前", (int) timeSpan.TotalDays);
                    }
                    if ((int) timeSpan.TotalHours > 0)
                    {
                        return string.Format("{0}小时前", (int) timeSpan.TotalHours);
                    }
                    if ((int) timeSpan.TotalMinutes > 0)
                    {
                        return string.Format("{0}分钟前", (int) timeSpan.TotalMinutes);
                    }
                    if ((int) timeSpan.TotalSeconds > 0)
                        return String.Format("{0}秒前", (int) timeSpan.TotalSeconds);
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion

        /// <summary>
        /// 本地时间转成GMT格式的时间
        /// </summary>
        public static string ToGMTFormat(DateTime dt)
        {
            return dt.ToString("r") + dt.ToString("zzz").Replace(":", "");
        }
    }

    public class HtmlToLinkConvert : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String link = "";
            if (value != null)
            {
                var r = new Regex("<a\\shref=\"(?<link>.*?)\"[^>]*?>(?<content>.*?)<\\/a>");
                Match match = r.Match(value.ToString());
                if (match.Length > 0)
                {
                    link = match.Groups["link"].Value;
                }
            }
            return link;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }

    public class HtmlToContentConvert : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String content = "";
            if (value != null)
            {
                var r = new Regex("<a\\shref=\"(?<link>.*?)\"[^>]*?>(?<content>.*?)<\\/a>");
                Match match = r.Match(value.ToString());
                if (match.Length > 0)
                    content = match.Groups["content"].Value;
            }
            return content;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}