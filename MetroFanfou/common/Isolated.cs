using System.IO.IsolatedStorage;

namespace MetroFanfou.common
{
    public class Isolated
    {
        private static readonly IsolatedStorageSettings Setting = IsolatedStorageSettings.ApplicationSettings;

        #region 方法

        /// <summary>
        /// 获取对应的属性值
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static object Get(string flag)
        {
            try
            {
                if (Setting.Contains(flag))
                {
                    return Setting[flag];
                }
                else
                {
                    Setting.Add(flag, null);
                }
                return null;
            }
            catch
            {
                Setting.Add(flag, null);
                return null;
            }
        }

        /// <summary>
        /// 获取对应的属性值
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool Get<T>(string flag, out T val)
        {
            try
            {
                if (Setting.Contains(flag))
                {
                    return Setting.TryGetValue<T>(flag, out val);
                }
                else
                {
                    Setting.Add(flag, null);
                }
                val = default(T);
                return false;
            }
            catch
            {
                Setting.Add(flag, null);

                val = default(T);
                return false;
            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="val"></param>
        public static void Set(string flag, object val)
        {
            try
            {
                if (Setting.Contains(flag))
                {
                    Setting[flag] = val;
                }
                else
                {
                    Setting.Add(flag, val);
                }
                Setting.Save();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 移除属性值
        /// </summary>
        /// <param name="flag"></param>
        public static bool Remove(string flag)
        {
            try
            {
                return Setting.Remove(flag);
            }
            catch
            {
                return false;
            }
        }

        #endregion 方法
    }
}