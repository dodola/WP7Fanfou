using System;
using MetroFanfou.common;
using Microsoft.Phone.Scheduler;

namespace MetroFanfou.Helper
{
    /// <summary>
    /// 后台服务帮助类
    /// </summary>
    public class ScheduledHelper
    {
        /// <summary>
        /// 进程是否已经启动
        /// </summary>
        private static bool isAgentOn { get; set; }

        /// <summary>
        /// 启动
        /// </summary>
        public static void Start()
        {
            if (!AppSetting.IsScheduledAgent)
            {
                return;
            }

            if (isAgentOn)
            {
                return;
            }
            Stop();

            var periodicTask = new PeriodicTask(Params.PeriodicTaskName);

            periodicTask.Description = "腾讯微博客户端Altman后台任务，用于帮助用户检查是否有新微博，您可以在应用设置选项中将其关闭或者在手机的后台任务界面停止它。";

            try
            {
                ScheduledActionService.Add(periodicTask);
                //ScheduledActionService.LaunchForTest(Common.Params.PeriodicTaskName, TimeSpan.FromSeconds(2));
                isAgentOn = true;
            }
            catch { isAgentOn = false; }
        }

        /// <summary>
        /// 停止
        /// </summary>
        public static void Stop()
        {
            try
            {
                if (!AppSetting.IsScheduledAgent)
                {
                    return;
                }

                var periodicTask = ScheduledActionService.Find(Params.PeriodicTaskName) as PeriodicTask;

                if (periodicTask != null)
                {
                    ScheduledActionService.Remove(Params.PeriodicTaskName);
                }

                isAgentOn = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}