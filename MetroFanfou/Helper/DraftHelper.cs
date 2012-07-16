using MetroFanfou.common;

namespace MetroFanfou.Helper
{
    public class DraftHelper
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="content"></param>
        public void Save(string content) {
            Isolated.Set(IsolatedHelper.TweetDraftKey,content);
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public string Get() {
            var str=Isolated.Get(IsolatedHelper.TweetDraftKey);
            if(str!=null){
                return str.ToString();
            }
            return "";
        }

        /// <summary>
        /// 清空草稿
        /// </summary>
        public void Clear() {
            Isolated.Set(IsolatedHelper.TweetDraftKey, "");
        }
    }
}
