using System.Collections.Generic;
using System.Linq;

namespace MetroFanfou.Helper
{
    public class ThemeHelper
    {
        public List<string> ThemeNames
        {
            get
            {
                return new List<string> { "Blue", "Black", "DarkBlue" };
            }
        }

        public List<KeyValuePair<string, string>> ThemeThumbUri
        {
            get
            {
                const string format = "/Resource/Images/theme.{0}.png";
                return ThemeNames.Select(n => new KeyValuePair<string, string>(n, string.Format(format, n.ToLower()))).ToList();
            }
        }

        /// <summary>
        /// 主题xaml文件
        /// </summary>
        /// <param name="themename"></param>
        /// <returns></returns>
        public static string ThemeFile(string themename)
        {
            return string.Format("/MetroFanfou;component/Themes/{0}.xaml", themename);
        }
    }
}