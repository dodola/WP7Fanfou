/////////////////////////////////////////////////////////
// Author   : huangzhiming
// Date     : 8/17/2011 3:18:44 PM
// Usage    :
/////////////////////////////////////////////////////////

using System;
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
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
    /// <summary>
    /// 字符串是否为空判断显隐属性
    /// </summary>
    public class StringToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return true;
            return string.IsNullOrEmpty(value.ToString()) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// 整型是否为0判断显隐属性，带参取反
    /// </summary>
    public class IntToVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var b = (long)value < 1;
            b = parameter == null ? b : !b;
            return b ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// 头像路径转换
    /// </summary>
    public class HeadUrlConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var size = parameter.ToString();
            return string.IsNullOrEmpty(value.ToString()) ? "/MetroFanfou;component/Resource/Images/noHead.png" : value.ToString() + "/" + (string.IsNullOrEmpty(size) ? "50" : size);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }


    /// <summary>
    /// 截取私信
    /// </summary>
    public class MessageListTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value.ToString();
            if (!string.IsNullOrWhiteSpace(str) && str.Length > 16)
            {
                str = str.Substring(0, 16) + "...";
            }
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// 父微博内容
    /// </summary>
    public class SourceTweetContentConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var t = (FanFou.SDK.Objects.Status)value;
                return string.Format("{0}: {1}", t.User.ScreenName, t.Text);
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
