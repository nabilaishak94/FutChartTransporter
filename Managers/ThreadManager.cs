using System;
using System.Collections.Generic;
using System.Text;
using FutChartTransporter_DotCore.Models;
using FutChartTransporter_DotCore.Common;
using FutChartTransporter_DotCore.Managers.QuestDB;
using System.Threading;

namespace FutChartTransporter_DotCore.Managers
{
    public class ThreadManager
    {
        public void StartThread()
        {
            while (true)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ThreadManager.ProcessChart));
                System.Threading.Thread.Sleep(AppSetting.ThreadSleepTime);
            }
        }

        public static void ProcessChart(object obj)
        {
            ChartFeedModel model = new ChartFeedModel(); ;
            try
            {
                if (Controller.ChartFeedQueue.Count > 0)
                {
                    Controller.ChartFeedQueue.TryDequeue(out model);

                    if (model != null)
                    {                        
                        Controller.chartFeedManager.ProcessChartFeed(model);
                    }
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Info($"ProcessChartFeed Error occured. Error: " + ex.ToString());
            }
        }
    }
}
