namespace MetroFanfou.common
{
    public class Params
    {
        /// <summary>
        /// 每加载显示的条数
        /// </summary>
        public static int PageListCount
        {
            get
            {
                return 15;
            }
        }

        /// <summary>
        /// 后台任务名称
        /// </summary>
        public static string PeriodicTaskName
        {
            get
            {
                return "AltmanTweetChecker";
            }
        }
    }
}