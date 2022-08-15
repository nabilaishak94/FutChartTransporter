using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FutChartTransporter_DotCore.Models;
using FutChartTransporter_DotCore.Repos.CQChart;
using FutChartTransporter_DotCore.Common;

namespace FutChartTransporter_DotCore.Managers.CQChart
{
    public class InsertChartCandleManager
    {
        private static readonly string LOG_CATEGORY = typeof(InsertChartCandleManager).FullName;

        public void InsertChartCandle(List<ChartFeedModel> chartFeedList, string seriesCode, string group,
            string priceType, string timeframe, DateTime triggerTime)
        {
            try
            {
                TimeSpan ts = new TimeSpan();
                if (timeframe == "1W")
                    ts += TimeSpan.FromDays(5);
                else if (timeframe == "1D")
                    ts += TimeSpan.FromDays(1);
                else if (timeframe == "4H")
                    ts += TimeSpan.FromHours(4);
                else if (timeframe == "1H")
                    ts += TimeSpan.FromHours(1);
                else if (timeframe == "30m")
                    ts += TimeSpan.FromMinutes(30);
                else if (timeframe == "15m")
                    ts += TimeSpan.FromMinutes(15);
                else if (timeframe == "5m")
                    ts += TimeSpan.FromMinutes(5);
                else if (timeframe == "1m")
                    ts += TimeSpan.FromMinutes(1);

                DateTime edt = triggerTime;
                DateTime sdt = edt;
                sdt = sdt.Add(-ts);

                List<ChartFeedModel> list = chartFeedList.
                    Where(
                        x => x.AccountInstrumentGrpId == group
                    ).Where(
                        x => Convert.ToDateTime(x.UpdatedDt) >= sdt && Convert.ToDateTime(x.UpdatedDt) < edt
                    ).OrderBy(x => x.UpdatedDt).ToList();

                if (list.Count > 0)
                {
                    double open = Convert.ToDouble(list.First().Price);
                    double close = Convert.ToDouble(list.Last().Price);
                    double high = list.Max(x => Convert.ToDouble(x.Price));
                    double low = list.Min(x => Convert.ToDouble(x.Price));

                    ChartRecordModel model = new ChartRecordModel();
                    model.AccountInstrumentGroupID = group;
                    model.SeriesCode = seriesCode;
                    model.PriceType = priceType;
                    model.High = high;
                    model.Low = low;
                    model.Open = open;
                    model.Close = close;
                    model.CurrentVolume = Math.Abs(Convert.ToDouble(list.Last().Volume) - Convert.ToDouble(list.First().Volume));
                    model.UpdateDt = sdt;

                    string[] tempArr = model.SeriesCode.Split('.');
                    //string tablename = model.PriceType + "_" + tempArr[0] + "_" + tempArr[1] + "_" + tempArr[2];

                    string tablename = "";

                    if (ChartSetting.ContinuousEnabledOnly == "1")
                        tablename = "Last_" + tempArr[0] + "_" + tempArr[1] + "_" + tempArr[2].Substring(0, tempArr[2].IndexOf("?"));
                    else
                        tablename = "Last_" + tempArr[0] + "_" + tempArr[1] + "_" + tempArr[2];

                    ChartRepo repo = new ChartRepo();
                    if (model.PriceType == "Ask")
                        repo.InsertAskChart(model, timeframe, tablename);
                    if (model.PriceType == "Bid")
                        repo.InsertBidChart(model, timeframe, tablename);
                    if (model.PriceType == "Last")
                        repo.InsertLastChart(model, timeframe, tablename);
                }
                else
                {
                    Controller.logger.Info(LOG_CATEGORY,
                    "No Candle Generate for SeriesCode:{0}, Group:{1}, PType:{2}, Timeframe:{3}, TriggerDt:{4}, StartTime:{5}, EndTime:{6}",
                    seriesCode, group, priceType, timeframe, triggerTime.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                    sdt.ToString("yyyy-MM-dd HH:mm:ss.fff"), edt.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Error(LOG_CATEGORY,
                    "Error Occured in InsertChartCandle(). SeriesCode:{1}, Group:{2}, PType:{3}, Timeframe:{4}, TriggerDt:{5}, Error: {0}",
                    ex.ToString(), seriesCode, group, priceType, timeframe, triggerTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
        }

        public void InsertSettlement1mCandle(ChartFeedModel model, DateTime dt)
        {
            try
            {
                string[] tempArr = model.SeriesCode.Split('.');
                //string tablename = "Last_" + tempArr[0] + "_" + tempArr[1] + "_" + tempArr[2].Substring(0, tempArr[2].IndexOf("?"));
                //nput.Substring(0, input.IndexOf("/") + 1);
                string tablename = "";

                if (ChartSetting.ContinuousEnabledOnly == "1")
                    tablename = "Last_" + tempArr[0] + "_" + tempArr[1] + "_" + tempArr[2].Substring(0, tempArr[2].IndexOf("?"));
                else
                    tablename = "Last_" + tempArr[0] + "_" + tempArr[1] + "_" + tempArr[2];
                ChartRepo repo = new ChartRepo();
                repo.InsertSettlementLastChart(model, "1m", tablename, dt);
            }
            catch (Exception ex)
            {
                Controller.logger.Error(LOG_CATEGORY,
                    "Error Occured in InsertSettlement1mCandle(). SeriesCode:{1}, Group:{2}, PType:{3}, TriggerDt:{4}, Error: {0}",
                    ex.ToString(), model.SeriesCode, model.AccountInstrumentGrpId, model.PriceType, model.UpdatedDt);
            }
        }

        public void InsertLastCandle(ChartRecordModel model, string timeframe)
        {
            try
            {
                if (string.IsNullOrEmpty(model.SeriesCode))
                    return;

                string[] tempArr = model.SeriesCode.Split('.');

                string tablename = "";

                if (model.SeriesCode.Contains("F.OTC."))
                {
                    tablename = "Bid_" + tempArr[0] + "_" + tempArr[1] + "_" + tempArr[2];
                }
                else if (model.SeriesCode.Contains("X.OTC_X."))
                {
                    tablename = "Bid_" + tempArr[0] + "_" + tempArr[1] + "_" + tempArr[2];
                }
                else
                {
                    if (ChartSetting.ContinuousEnabledOnly == "1")
                        tablename = "Last_" + tempArr[0] + "_" + tempArr[1] + "_" + tempArr[2].Substring(0, tempArr[2].IndexOf("?"));
                    else
                        tablename = "Last_" + tempArr[0] + "_" + tempArr[1] + "_" + tempArr[2];
                }

                ChartRepo repo = new ChartRepo();
                repo.InsertLastChart1(model, timeframe, tablename);
            }
            catch (Exception ex)
            {
                Controller.logger.Error(LOG_CATEGORY,
                    "Error Occured in InsertCandle(). SeriesCode:{1}, Group:{2}, PType:{3}, TriggerDt:{4}, TimeFrame:{5} Error: {0}",
                    ex.ToString(), model.SeriesCode, model.AccountInstrumentGroupID, model.PriceType, model.UpdateDt, timeframe);
            }
        }
    }
}
