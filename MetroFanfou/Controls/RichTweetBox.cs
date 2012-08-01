using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using FanFou.SDK.Objects;
using Microsoft.Phone.Tasks;

namespace MetroFanfou.Controls
{
    public class RichTweetBox : RichTextBox
    {
        #region 自定义属性

        /// <summary>
        /// 依赖项，显示的时间线内容类型
        /// </summary>
        public static DependencyProperty OrigTextProperty = DependencyProperty.Register("OrigText",
                                                                                        typeof(string),
                                                                                        typeof(RichTweetBox),
                                                                                        new PropertyMetadata("",
                                                                                                             OrigTextPropertyChangeCallback));

        /// <summary>
        /// 微博的原始内容
        /// </summary>
        [Description("微博的原始内容")]
        public string OrigText
        {
            get { return GetValue(OrigTextProperty).ToString(); }
            set { SetValue(OrigTextProperty, value); }
        }

        private static void OrigTextPropertyChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d != null)
            {
                var richTextBox = (RichTweetBox)d;
                string s = e.NewValue.ToString();
                richTextBox.Dispatcher.BeginInvoke(() => richTextBox.SetContent(s));
            }
        }

        #endregion

        /// <summary>
        /// 设置内容解析帐号
        /// </summary>
        public void SetContent(string text, ICollection<User> userInfos)
        {
            Blocks.Clear();
            PaserContent(userInfos, text);
        }

        /// <summary>
        /// 设置内容解析帐号
        /// </summary>
        public void SetContent(string text)
        {
            Blocks.Clear();
            PaserContent(null, text);
        }


        private void PaserContent(ICollection<User> userInfos, string content)
        {
            const string reg = @"@.*?\s+|\#([^\#|.]+)\#|http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)";
            var regAccount = new Regex(reg);
            MatchCollection matches = regAccount.Matches(content);
            int lastPos = 0;
            var block = new Paragraph();
            foreach (Match match in matches)
            {
                if (match.Index != lastPos)
                {
                    block.Inlines.Add(TextRun(content.Substring(lastPos, match.Index - lastPos)));
                }
                string val = match.Value;
                if (!string.IsNullOrWhiteSpace(val))
                {
                    if (val.IndexOf("@") == 0)
                    {
                        var run = new Run
                                      {
                                          Foreground =
                                              (SolidColorBrush)Application.Current.Resources["DetailComponentBrush"],
                                          Text = GetUserNickName(val.Substring(1), userInfos)
                                      };
                        block.Inlines.Add(run);
                    }
                    else if (val.IndexOf("#") == 0)
                    {
                        var run = new Run
                                      {
                                          Foreground =
                                              (SolidColorBrush)Application.Current.Resources["DetailComponentBrush"],
                                          Text = val
                                      };
                        block.Inlines.Add(run);
                    }
                    else
                    {
                        var link = new Hyperlink
                                       {
                                           Foreground =
                                               (SolidColorBrush)Application.Current.Resources["DetailComponentBrush"],
                                           NavigateUri = new Uri(val, UriKind.Absolute)
                                       };
                        link.Inlines.Add(val);
                        link.Click += HyperlinkClick;
                        block.Inlines.Add(link);
                    }

                    lastPos = match.Index + match.Length;
                }
            }
            if (lastPos < content.Length)
            {
                block.Inlines.Add(TextRun(content.Substring(lastPos)));
            }
            Selection.Insert(block);
        }


        private Run TextRun(string text)
        {
            return new Run
                       {
                           Foreground = new SolidColorBrush((Color)Application.Current.Resources["DetailContentColor"]),
                           Text = text,
                           FontFamily = new FontFamily("Microsoft YaHei")
                       };
        }

        private string GetUserNickName(string name, IEnumerable<User> users)
        {
            string rs = name;
            if (users != null)
            {
                foreach (User u in users)
                {
                    if (string.Compare(name, u.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        return u.ScreenName;
                    }
                }
            }
            return "@" + rs;
        }

        /// <summary>
        /// 打开网页
        /// </summary>
        private void HyperlinkClick(object sender, RoutedEventArgs e)
        {


            var link = sender as Hyperlink;
            if (link != null)
            {
                var webBrowserTask = new WebBrowserTask
                                         {
                                             Uri =
                                                 new Uri(
                                                 link.NavigateUri.AbsoluteUri,
                                                 UriKind.Absolute)
                                         };
                webBrowserTask.Show();
            }

        }
    }
}