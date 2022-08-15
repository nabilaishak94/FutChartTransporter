using System;
using System.Collections.Generic;
using System.Text;
using FutChartTransporter_DotCore.Common;
using FutChartTransporter_DotCore.Models;
using FutChartTransporter_DotCore.Managers.QuestDB;
using System.Net;
using System.Globalization;

namespace FutChartTransporter_DotCore.Managers
{
    public class ChartFeedManager
    {
        public void ProcessChartFeed(ChartFeedModel model)
        {
            try
            {
                var address = IPAddress.Loopback.ToString();

                using (var sender = new LineTcpSenderManager(DBSetting.QuestDB_Hostname, DBSetting.QuestDB_Port))
                {
                    //2022-06-08 16:13:03
                    DateTime feedDt;                    
                    string Dtformat = "yyyy-MM-dd HH:mm:ss";

                    sender.ValidateSocketConnected();

                    if (DateTime.TryParseExact(model.UpdatedDt, Dtformat, CultureInfo.InvariantCulture, DateTimeStyles.None, out feedDt))
                    {
                        sender
                        .Table(DBSetting.QuestDB_DBName) // targeted table name
                        .Column("AccountGroupId", model.AccountInstrumentGrpId)
                        .Column("SeriesCode", model.SeriesCode)
                        .Column("Price", (double)model.Price)
                        .Column("PriceType", model.PriceType)
                        .Column("Volume", Convert.ToDouble(model.Volume))
                        .At(new DateTime(feedDt.Year, feedDt.Month, feedDt.Day, feedDt.Hour, feedDt.Minute, feedDt.Second)) //timestamp column assigned datetime
                        ;

                        sender.Flush();
                        Controller.rabbitMQManager.SendChartFeed(model);
                    }                        
                }                
            }
            catch(Exception ex)
            {
                Controller.logger.Error($"Error occured while processing chart feed. Error:{ex}");
            }
        }

        public void ProcessChartFeedStoreToRedis(ChartFeedModel model)
        {
            try
            {
                //if (model.SeriesCode == "F.CME.NQ.U22")
                //{
                //    Controller.logger.Info($"{model.ToString()}");
                //}

                DateTime feedDt;
                string Dtformat = "yyyy-MM-dd HH:mm:ss";
                if (DateTime.TryParseExact(model.UpdatedDt, Dtformat, CultureInfo.InvariantCulture, DateTimeStyles.None, out feedDt))
                {
                    string key = $"FEED|{model.SeriesCode}|{model.PriceType}|{model.AccountInstrumentGrpId}|{feedDt.ToString("yyyy-MM-dd HH:mm:00")}";
                    string value = $"{model.Price}@{model.Volume}@{model.UpdatedDt}|";
                    Controller.RedisManager.AppendPriceUpdate(key,value);
                    Controller.rabbitMQManager.SendChartFeed(model);
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Error($"Error occured while processing chart feed. Error:{ex}");
            }
        }
    }
}
