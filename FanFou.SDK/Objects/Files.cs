using System.Collections.Generic;

namespace FanFou.SDK.Objects
{
    /// <summary>
    /// 文件集
    /// </summary>
    public class Files
    {
        /// <summary>
        /// 
        /// </summary>
        public Files()
        {
            this.Items = new List<KeyValuePair<string, UploadFile>>();
        }

        /// <summary>
        /// 文件集
        /// </summary>
        public List<KeyValuePair<string, UploadFile>> Items
        {
            get;
            private set;
        }
        /// <summary>
        /// 清空所有文件
        /// </summary>
        /// <returns></returns>
        public void Clear()
        {
            this.Items.Clear();
        }
        /// <summary>
        /// 添加文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="file"></param>
        public void Add(string name, UploadFile file)
        {
            this.Items.Add(new KeyValuePair<string, UploadFile>(name, file));
        }
    }
}
