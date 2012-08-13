namespace MetroFanfou.Helper
{
    /// <summary>
    /// 添加微博的类型
    /// </summary>
    public enum EAddTweetType
    {
        /// <summary>
        /// 添加
        /// </summary>
        Add = 0,

        /// <summary>
        /// 转播
        /// </summary>
        ReAdd = 1,

        /// <summary>
        /// 回复
        /// </summary>
        Reply = 2,

        /// <summary>
        /// 提及某人
        /// </summary>
        Mention = 3,

        /// <summary>
        /// 私信
        /// </summary>
        PrivateMessage = 4,

        /// <summary>
        /// 对话
        /// </summary>
        Comment = 5
    }
}