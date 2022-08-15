using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FutChartTransporter_DotCore.Models;
using FutChartTransporter_DotCore.Common;

namespace FutChartTransporter_DotCore.Managers
{
    public class GenerateChartUpdateManager
    {
        public ChartRecordModel Generate1mChartUpdate(List<ChartFeedModel> chartFeedList, ChartFeedModel chartFeedModel)
        {
            DateTime dateTime = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            DateTime sdt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
            DateTime edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
            edt = edt.AddMinutes(1.0);
            List<ChartFeedModel> list = (from x in chartFeedList
                                         where x.AccountInstrumentGrpId == chartFeedModel.AccountInstrumentGrpId
                                         where Convert.ToDateTime(x.UpdatedDt) >= sdt && Convert.ToDateTime(x.UpdatedDt) < edt
                                         orderby x.UpdatedDt
                                         select x).ToList();
            ChartRecordModel chartRecordModel = new ChartRecordModel();
            if (list.Count > 0)
            {
                double open = Convert.ToDouble(list.First().Price);
                double close = Convert.ToDouble(list.Last().Price);
                double high = list.Max((ChartFeedModel x) => Convert.ToDouble(x.Price));
                double low = list.Min((ChartFeedModel x) => Convert.ToDouble(x.Price));
                chartRecordModel.AccountInstrumentGroupID = chartFeedModel.AccountInstrumentGrpId;
                chartRecordModel.SeriesCode = chartFeedModel.SeriesCode;
                chartRecordModel.PriceType = chartFeedModel.PriceType;
                chartRecordModel.High = high;
                chartRecordModel.Low = low;
                chartRecordModel.Open = open;
                chartRecordModel.Close = close;
                chartRecordModel.CurrentVolume = Math.Abs(Convert.ToDouble(list.Last().Volume) - Convert.ToDouble(list.First().Volume));
                chartRecordModel.UpdateDt = sdt;
            }
            return chartRecordModel;
        }

        public ChartRecordModel GenerateChartUpdate(List<ChartFeedModel> chartFeedList, ChartFeedModel chartFeedModel, int forMin)
        {
            DateTime dateTime = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            DateTime sdt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
            DateTime edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
            if (dateTime.Minute == 0)
            {
                edt = edt.AddMinutes(forMin);
            }
            else
            {
                int num = (int)Math.Ceiling((double)dateTime.Minute / (double)forMin) * forMin;
                if (num == 60)
                {
                    edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour + 1, 0, 0);
                    sdt = edt;
                    sdt = sdt.AddMinutes(-forMin);
                }
                else
                {
                    edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, num, 0);
                    sdt = edt;
                    sdt = sdt.AddMinutes(-forMin);
                }
            }
            List<ChartFeedModel> list = (from x in chartFeedList
                                         where x.AccountInstrumentGrpId == chartFeedModel.AccountInstrumentGrpId
                                         where Convert.ToDateTime(x.UpdatedDt) >= sdt && Convert.ToDateTime(x.UpdatedDt) < edt
                                         orderby x.UpdatedDt
                                         select x).ToList();
            ChartRecordModel chartRecordModel = new ChartRecordModel();
            if (list.Count > 0)
            {
                double open = Convert.ToDouble(list.First().Price);
                double close = Convert.ToDouble(list.Last().Price);
                double high = list.Max((ChartFeedModel x) => Convert.ToDouble(x.Price));
                double low = list.Min((ChartFeedModel x) => Convert.ToDouble(x.Price));
                chartRecordModel.AccountInstrumentGroupID = chartFeedModel.AccountInstrumentGrpId;
                chartRecordModel.SeriesCode = chartFeedModel.SeriesCode;
                chartRecordModel.PriceType = chartFeedModel.PriceType;
                chartRecordModel.High = high;
                chartRecordModel.Low = low;
                chartRecordModel.Open = open;
                chartRecordModel.Close = close;
                chartRecordModel.CurrentVolume = Math.Abs(Convert.ToDouble(list.Last().Volume) - Convert.ToDouble(list.First().Volume));
                chartRecordModel.UpdateDt = sdt;
            }
            return chartRecordModel;
        }

        public ChartRecordModel Generate1HChartUpdate(List<ChartFeedModel> chartFeedList, ChartFeedModel chartFeedModel)
        {
            DateTime dateTime = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            DateTime sdt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
            DateTime edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
            edt = edt.AddHours(1.0);
            List<ChartFeedModel> list = (from x in chartFeedList
                                         where x.AccountInstrumentGrpId == chartFeedModel.AccountInstrumentGrpId
                                         where Convert.ToDateTime(x.UpdatedDt) >= sdt && Convert.ToDateTime(x.UpdatedDt) < edt
                                         orderby x.UpdatedDt
                                         select x).ToList();
            ChartRecordModel chartRecordModel = new ChartRecordModel();
            if (list.Count > 0)
            {
                double open = Convert.ToDouble(list.First().Price);
                double close = Convert.ToDouble(list.Last().Price);
                double high = list.Max((ChartFeedModel x) => Convert.ToDouble(x.Price));
                double low = list.Min((ChartFeedModel x) => Convert.ToDouble(x.Price));
                chartRecordModel.AccountInstrumentGroupID = chartFeedModel.AccountInstrumentGrpId;
                chartRecordModel.SeriesCode = chartFeedModel.SeriesCode;
                chartRecordModel.PriceType = chartFeedModel.PriceType;
                chartRecordModel.High = high;
                chartRecordModel.Low = low;
                chartRecordModel.Open = open;
                chartRecordModel.Close = close;
                chartRecordModel.CurrentVolume = Math.Abs(Convert.ToDouble(list.Last().Volume) - Convert.ToDouble(list.First().Volume));
                chartRecordModel.UpdateDt = sdt;
            }
            return chartRecordModel;
        }

        public ChartRecordModel Generate4HChartUpdate(List<ChartFeedModel> chartFeedList, ChartFeedModel chartFeedModel, int forHour)
        {
            DateTime dateTime = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            DateTime sdt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
            DateTime edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
            if (dateTime.Hour == 0)
            {
                edt = edt.AddHours(4.0);
            }
            else
            {
                int num = (int)Math.Ceiling((double)dateTime.Hour / (double)forHour) * forHour;
                if (num == dateTime.Hour)
                {
                    edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, num, 0, 0);
                    edt = edt.AddHours(forHour);
                    sdt = edt;
                    sdt = sdt.AddHours(-forHour);
                }
                else
                {
                    edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0).AddHours(num - dateTime.Hour);
                    sdt = edt;
                    sdt = sdt.AddHours(-forHour);
                }
            }
            List<ChartFeedModel> list = (from x in chartFeedList
                                         where x.AccountInstrumentGrpId == chartFeedModel.AccountInstrumentGrpId
                                         where Convert.ToDateTime(x.UpdatedDt) >= sdt && Convert.ToDateTime(x.UpdatedDt) < edt
                                         orderby x.UpdatedDt
                                         select x).ToList();
            ChartRecordModel chartRecordModel = new ChartRecordModel();
            if (list.Count > 0)
            {
                double open = Convert.ToDouble(list.First().Price);
                double close = Convert.ToDouble(list.Last().Price);
                double high = list.Max((ChartFeedModel x) => Convert.ToDouble(x.Price));
                double low = list.Min((ChartFeedModel x) => Convert.ToDouble(x.Price));
                chartRecordModel.AccountInstrumentGroupID = chartFeedModel.AccountInstrumentGrpId;
                chartRecordModel.SeriesCode = chartFeedModel.SeriesCode;
                chartRecordModel.PriceType = chartFeedModel.PriceType;
                chartRecordModel.High = high;
                chartRecordModel.Low = low;
                chartRecordModel.Open = open;
                chartRecordModel.Close = close;
                chartRecordModel.CurrentVolume = Math.Abs(Convert.ToDouble(list.Last().Volume) - Convert.ToDouble(list.First().Volume));
                chartRecordModel.UpdateDt = sdt;
            }
            return chartRecordModel;
        }

        public ChartRecordModel Generate1DChartUpdate(List<ChartFeedModel> chartFeedList, ChartFeedModel chartFeedModel)
        {
            DateTime t = Convert.ToDateTime(ChartSetting.Chart_EODTime);
            DateTime dateTime = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            DateTime sdt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, t.Hour, t.Minute, 0);
            if (DateTime.Now < t)
            {
                sdt = sdt.AddDays(-1.0);
            }
            DateTime edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, t.Hour, t.Minute, 0);
            if (DateTime.Now > t)
            {
                edt = edt.AddDays(1.0);
            }
            List<ChartFeedModel> list = (from x in chartFeedList
                                         where x.AccountInstrumentGrpId == chartFeedModel.AccountInstrumentGrpId
                                         where Convert.ToDateTime(x.UpdatedDt) >= sdt && Convert.ToDateTime(x.UpdatedDt) < edt
                                         orderby x.UpdatedDt
                                         select x).ToList();
            ChartRecordModel chartRecordModel = new ChartRecordModel();
            if (list.Count > 0)
            {
                double open = Convert.ToDouble(list.First().Price);
                double close = Convert.ToDouble(list.Last().Price);
                double high = list.Max((ChartFeedModel x) => Convert.ToDouble(x.Price));
                double low = list.Min((ChartFeedModel x) => Convert.ToDouble(x.Price));
                chartRecordModel.AccountInstrumentGroupID = chartFeedModel.AccountInstrumentGrpId;
                chartRecordModel.SeriesCode = chartFeedModel.SeriesCode;
                chartRecordModel.PriceType = chartFeedModel.PriceType;
                chartRecordModel.High = high;
                chartRecordModel.Low = low;
                chartRecordModel.Open = open;
                chartRecordModel.Close = close;
                chartRecordModel.CurrentVolume = Math.Abs(Convert.ToDouble(list.Last().Volume) - Convert.ToDouble(list.First().Volume));
                chartRecordModel.UpdateDt = sdt;
            }
            return chartRecordModel;
        }

        public ChartRecordModel Generate1WChartUpdate(List<ChartFeedModel> chartFeedList, ChartFeedModel chartFeedModel)
        {
            DateTime dateTime = Convert.ToDateTime(ChartSetting.Chart_EODTime);
            DateTime dateTime2 = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            int num = (int)(ChartSetting.EOWDay - dateTime.DayOfWeek + 7) % 7;
            int num2 = (int)(ChartSetting.Chart_FirstDayOfWeek - dateTime2.DayOfWeek) % 7;
            DateTime dateTime3 = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            DateTime sdt = new DateTime(dateTime3.Year, dateTime3.Month, dateTime3.Day, dateTime.Hour, dateTime.Minute, 0);
            DateTime edt = new DateTime(dateTime3.Year, dateTime3.Month, dateTime3.Day, dateTime.Hour, dateTime.Minute, 0);
            edt = edt.AddDays(num);
            sdt = sdt.AddDays(num2);
            List<ChartFeedModel> list = (from x in chartFeedList
                                         where x.AccountInstrumentGrpId == chartFeedModel.AccountInstrumentGrpId
                                         where Convert.ToDateTime(x.UpdatedDt) >= sdt && Convert.ToDateTime(x.UpdatedDt) < edt
                                         orderby x.UpdatedDt
                                         select x).ToList();
            ChartRecordModel chartRecordModel = new ChartRecordModel();
            if (list.Count > 0)
            {
                double open = Convert.ToDouble(list.First().Price);
                double close = Convert.ToDouble(list.Last().Price);
                double high = list.Max((ChartFeedModel x) => Convert.ToDouble(x.Price));
                double low = list.Min((ChartFeedModel x) => Convert.ToDouble(x.Price));
                chartRecordModel.AccountInstrumentGroupID = chartFeedModel.AccountInstrumentGrpId;
                chartRecordModel.SeriesCode = chartFeedModel.SeriesCode;
                chartRecordModel.PriceType = chartFeedModel.PriceType;
                chartRecordModel.High = high;
                chartRecordModel.Low = low;
                chartRecordModel.Open = open;
                chartRecordModel.Close = close;
                chartRecordModel.CurrentVolume = Math.Abs(Convert.ToDouble(list.Last().Volume) - Convert.ToDouble(list.First().Volume));
                chartRecordModel.UpdateDt = sdt;
            }
            return chartRecordModel;
        }

        public ChartRecordModel GenerateChartUpdate(List<ChartRecordModel> chartRecordList, ChartFeedModel chartFeedModel, int forMin)
        {
            DateTime dateTime = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            DateTime sdt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
            DateTime edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
            if (dateTime.Minute == 0)
            {
                edt = edt.AddMinutes(forMin);
            }
            else
            {
                int num = (int)Math.Ceiling((double)dateTime.Minute / (double)forMin) * forMin;
                num = ((dateTime.Minute % forMin != 0) ? ((int)Math.Ceiling((double)dateTime.Minute / (double)forMin) * forMin) : (num + forMin));
                if (num == 60)
                {
                    if (dateTime.Hour + 1 == 24)
                    {
                        DateTime dateTime2 = dateTime.AddDays(1.0);
                        edt = new DateTime(dateTime2.Year, dateTime2.Month, dateTime2.Day, 0, 0, 0);
                        sdt = edt;
                        sdt = sdt.AddMinutes(-forMin);
                    }
                    else
                    {
                        edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour + 1, 0, 0);
                        sdt = edt;
                        sdt = sdt.AddMinutes(-forMin);
                    }
                }
                else
                {
                    edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, num, 0);
                    sdt = edt;
                    sdt = sdt.AddMinutes(-forMin);
                }
            }
            List<ChartRecordModel> list = (from x in chartRecordList
                                           where x.AccountInstrumentGroupID == chartFeedModel.AccountInstrumentGrpId
                                           where Convert.ToDateTime(x.UpdateDt) >= sdt && Convert.ToDateTime(x.UpdateDt) < edt
                                           orderby x.UpdateDt
                                           select x).ToList();
            ChartRecordModel chartRecordModel = new ChartRecordModel();
            if (list.Count > 0)
            {
                double open = Convert.ToDouble(list.First().Open);
                double close = Convert.ToDouble(list.Last().Close);
                double high = list.Max((ChartRecordModel x) => Convert.ToDouble(x.High));
                double low = list.Min((ChartRecordModel x) => Convert.ToDouble(x.Low));
                chartRecordModel.AccountInstrumentGroupID = chartFeedModel.AccountInstrumentGrpId;
                chartRecordModel.SeriesCode = chartFeedModel.SeriesCode;
                chartRecordModel.PriceType = chartFeedModel.PriceType;
                chartRecordModel.High = high;
                chartRecordModel.Low = low;
                chartRecordModel.Open = open;
                chartRecordModel.Close = close;
                chartRecordModel.CurrentVolume = list.Sum((ChartRecordModel x) => x.CurrentVolume);
                chartRecordModel.UpdateDt = sdt;
            }
            return chartRecordModel;
        }

        public ChartRecordModel Generate1HChartUpdate(List<ChartRecordModel> chartRecordList, ChartFeedModel chartFeedModel)
        {
            DateTime dateTime = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            DateTime sdt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
            DateTime edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
            edt = edt.AddHours(1.0);
            List<ChartRecordModel> list = (from x in chartRecordList
                                           where x.AccountInstrumentGroupID == chartFeedModel.AccountInstrumentGrpId
                                           where Convert.ToDateTime(x.UpdateDt) >= sdt && Convert.ToDateTime(x.UpdateDt) < edt
                                           orderby x.UpdateDt
                                           select x).ToList();
            ChartRecordModel chartRecordModel = new ChartRecordModel();
            if (list.Count > 0)
            {
                double open = Convert.ToDouble(list.First().Open);
                double close = Convert.ToDouble(list.Last().Close);
                double high = list.Max((ChartRecordModel x) => Convert.ToDouble(x.High));
                double low = list.Min((ChartRecordModel x) => Convert.ToDouble(x.Low));
                chartRecordModel.AccountInstrumentGroupID = chartFeedModel.AccountInstrumentGrpId;
                chartRecordModel.SeriesCode = chartFeedModel.SeriesCode;
                chartRecordModel.PriceType = chartFeedModel.PriceType;
                chartRecordModel.High = high;
                chartRecordModel.Low = low;
                chartRecordModel.Open = open;
                chartRecordModel.Close = close;
                chartRecordModel.CurrentVolume = list.Sum((ChartRecordModel x) => x.CurrentVolume);
                chartRecordModel.UpdateDt = sdt;
            }
            return chartRecordModel;
        }

        public ChartRecordModel Generate4HChartUpdate(List<ChartRecordModel> chartRecordList, ChartFeedModel chartFeedModel, int forHour)
        {
            DateTime dateTime = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            DateTime sdt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
            DateTime edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
            if (dateTime.Hour == 0)
            {
                edt = edt.AddHours(4.0);
            }
            else
            {
                int num = (int)Math.Ceiling((double)dateTime.Hour / (double)forHour) * forHour;
                if (num == dateTime.Hour)
                {
                    edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, num, 0, 0);
                    edt = edt.AddHours(forHour);
                    sdt = edt;
                    sdt = sdt.AddHours(-forHour);
                }
                else
                {
                    edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0).AddHours(num - dateTime.Hour);
                    sdt = edt;
                    sdt = sdt.AddHours(-forHour);
                }
            }
            List<ChartRecordModel> list = (from x in chartRecordList
                                           where x.AccountInstrumentGroupID == chartFeedModel.AccountInstrumentGrpId
                                           where Convert.ToDateTime(x.UpdateDt) >= sdt && Convert.ToDateTime(x.UpdateDt) < edt
                                           orderby x.UpdateDt
                                           select x).ToList();
            ChartRecordModel chartRecordModel = new ChartRecordModel();
            if (list.Count > 0)
            {
                double open = Convert.ToDouble(list.First().Open);
                double close = Convert.ToDouble(list.Last().Close);
                double high = list.Max((ChartRecordModel x) => Convert.ToDouble(x.High));
                double low = list.Min((ChartRecordModel x) => Convert.ToDouble(x.Low));
                chartRecordModel.AccountInstrumentGroupID = chartFeedModel.AccountInstrumentGrpId;
                chartRecordModel.SeriesCode = chartFeedModel.SeriesCode;
                chartRecordModel.PriceType = chartFeedModel.PriceType;
                chartRecordModel.High = high;
                chartRecordModel.Low = low;
                chartRecordModel.Open = open;
                chartRecordModel.Close = close;
                chartRecordModel.CurrentVolume = list.Sum((ChartRecordModel x) => x.CurrentVolume);
                chartRecordModel.UpdateDt = sdt;
            }
            return chartRecordModel;
        }

        public ChartRecordModel Generate1DChartUpdate(List<ChartRecordModel> chartRecordList, ChartFeedModel chartFeedModel)
        {
            DateTime t = Convert.ToDateTime(ChartSetting.Chart_EODTime);
            DateTime dateTime = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            DateTime sdt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, t.Hour, t.Minute, 0);
            if (DateTime.Now < t)
            {
                sdt = sdt.AddDays(-1.0);
            }
            DateTime edt = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, t.Hour, t.Minute, 0);
            if (DateTime.Now > t)
            {
                edt = edt.AddDays(1.0);
            }
            List<ChartRecordModel> list = (from x in chartRecordList
                                           where x.AccountInstrumentGroupID == chartFeedModel.AccountInstrumentGrpId
                                           where Convert.ToDateTime(x.UpdateDt) >= sdt && Convert.ToDateTime(x.UpdateDt) < edt
                                           orderby x.UpdateDt
                                           select x).ToList();
            ChartRecordModel chartRecordModel = new ChartRecordModel();
            if (list.Count > 0)
            {
                double open = Convert.ToDouble(list.First().Open);
                double close = Convert.ToDouble(list.Last().Close);
                double high = list.Max((ChartRecordModel x) => Convert.ToDouble(x.High));
                double low = list.Min((ChartRecordModel x) => Convert.ToDouble(x.Low));
                chartRecordModel.AccountInstrumentGroupID = chartFeedModel.AccountInstrumentGrpId;
                chartRecordModel.SeriesCode = chartFeedModel.SeriesCode;
                chartRecordModel.PriceType = chartFeedModel.PriceType;
                chartRecordModel.High = high;
                chartRecordModel.Low = low;
                chartRecordModel.Open = open;
                chartRecordModel.Close = close;
                chartRecordModel.CurrentVolume = list.Sum((ChartRecordModel x) => x.CurrentVolume);
                chartRecordModel.UpdateDt = sdt;
            }
            return chartRecordModel;
        }

        public ChartRecordModel Generate1WChartUpdate(List<ChartRecordModel> chartRecordList, ChartFeedModel chartFeedModel)
        {
            DateTime dateTime = Convert.ToDateTime(ChartSetting.Chart_EODTime);
            DateTime dateTime2 = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            int num = (int)(ChartSetting.EOWDay - dateTime.DayOfWeek + 7) % 7;
            int num2 = (int)(ChartSetting.Chart_FirstDayOfWeek - dateTime2.DayOfWeek) % 7;
            DateTime dateTime3 = Convert.ToDateTime(chartFeedModel.UpdatedDt);
            DateTime sdt = new DateTime(dateTime3.Year, dateTime3.Month, dateTime3.Day, dateTime.Hour, dateTime.Minute, 0);
            DateTime edt = new DateTime(dateTime3.Year, dateTime3.Month, dateTime3.Day, dateTime.Hour, dateTime.Minute, 0);
            edt = edt.AddDays(num);
            sdt = sdt.AddDays(num2);
            List<ChartRecordModel> list = (from x in chartRecordList
                                           where x.AccountInstrumentGroupID == chartFeedModel.AccountInstrumentGrpId
                                           where Convert.ToDateTime(x.UpdateDt) >= sdt && Convert.ToDateTime(x.UpdateDt) < edt
                                           orderby x.UpdateDt
                                           select x).ToList();
            ChartRecordModel chartRecordModel = new ChartRecordModel();
            if (list.Count > 0)
            {
                double open = Convert.ToDouble(list.First().Open);
                double close = Convert.ToDouble(list.Last().Close);
                double high = list.Max((ChartRecordModel x) => Convert.ToDouble(x.High));
                double low = list.Min((ChartRecordModel x) => Convert.ToDouble(x.Low));
                chartRecordModel.AccountInstrumentGroupID = chartFeedModel.AccountInstrumentGrpId;
                chartRecordModel.SeriesCode = chartFeedModel.SeriesCode;
                chartRecordModel.PriceType = chartFeedModel.PriceType;
                chartRecordModel.High = high;
                chartRecordModel.Low = low;
                chartRecordModel.Open = open;
                chartRecordModel.Close = close;
                chartRecordModel.CurrentVolume = list.Sum((ChartRecordModel x) => x.CurrentVolume);
                chartRecordModel.UpdateDt = sdt;
            }
            return chartRecordModel;
        }
    }
}
