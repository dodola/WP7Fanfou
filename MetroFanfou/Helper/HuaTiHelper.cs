using System;
using System.Collections.Generic;
using System.Linq;
using MetroFanfou.common;

namespace MetroFanfou.Helper
{
    /// <summary>
    /// 话题帮助类
    /// </summary>
    public class HuaTiHelper
    {
        /// <summary>
        /// 获取我最近输入的话题
        /// </summary>
        /// <param name="count">获取条数</param>
        /// <returns></returns>
        public static List<string> GetRecent(int count = 0)
        {
            var ht = (List<string>)Isolated.Get(IsolatedHelper.HuaTiKey);
            if (ht == null) {
                return new List<string>();
            }
            if (count > 0) {
                count = Math.Min(count,ht.Count);
                ht = ht.Take(count).ToList();
            }
            return ht;
        }
        /// <summary>
        /// 设置话题
        /// </summary>
        /// <param name="ht"></param>
        /// <returns></returns>
        public static void SetOneHuaTi(string htStr) {
            var ht = (new List<string> { htStr });
            var all = GetRecent();
            if (all != null && all.Count > 0) {
                foreach (var item in all)
                {
                    if (string.Compare(item, htStr, StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        return;
                    }
                }
            }
            var rs = all == null ? ht : (ht.Concat(all).Take(10).ToList());
            Isolated.Set(IsolatedHelper.HuaTiKey,rs);
        }
    }
}
