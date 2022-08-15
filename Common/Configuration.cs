using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Configuration.Json;

namespace FutChartTransporter_DotCore.Common
{
    public static class Configuration
    {
        public static IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json", true, true).Build();
    }

    public static class AppSetting
    {
        public static readonly int ThreadSleepTime = string.IsNullOrEmpty(Configuration.config["AppSetting:ThreadSleepTime"]) ? 2 : Convert.ToInt32(Configuration.config["AppSetting:ThreadSleepTime"]);
        public static readonly string EnableLog = Configuration.config["AppSetting:EnableLog"];
    }

    //public static class SPSetting
    //{
    //    public static readonly string SP_InsertHLOCVAsk = Configuration.config["SPSetting:Insert_HLOCVAsk"];
    //    public static readonly string SP_InsertHLOCVBid = Configuration.config["SPSetting:Insert_HLOCVBid"];
    //    public static readonly string SP_InsertHLOCVLast = Configuration.config["SPSetting:Insert_HLOCVLast"];
    //    public static readonly string SP_GetTransporterSubMap = Configuration.config["SPSetting:GetTransporterSubMap"];
    //    public static readonly string SP_GetTimeZoneList = Configuration.config["SPSetting:GetTimeZoneList"];
    //    public static readonly string SP_GetInstrumentScheduleList = Configuration.config["SPSetting:GetInstrumentScheduleList"];
    //    public static readonly string SP_GetContinuousChartMappingList = Configuration.config["SPSetting:GetContinuousChartMappingList"];
    //
    //}

    public static class DBSetting
    {
        //public static readonly string ConnectionName_CQMobile = Configuration.config["DBSetting:CQMobile.ConnectionName"];
        //public static readonly string ConnectionName_CQChart = Configuration.config["DBSetting:CQChart.ConnectionName"];

        public static readonly string QuestDB_Hostname = Configuration.config["DBSetting:QuestDB_Hostname"];
        public static readonly string QuestDB_DBName = Configuration.config["DBSetting:QuestDB_DBName"];
        public static readonly int QuestDB_Port = Convert.ToInt32(Configuration.config["DBSetting:QuestDB_Port"]);
    }
    
    public static class RedisSetting
    {
        public static readonly string Hostname = Configuration.config["RedisSetting:Hostname"];
        public static readonly int Port = Convert.ToInt32(Configuration.config["RedisSetting:Port"]);
        public static readonly bool AbortOnConnectFail = Convert.ToBoolean(Configuration.config["RedisSetting:AbortOnConnectFail"]);
        public static readonly bool AllowAdmin = Convert.ToBoolean(Configuration.config["RedisSetting:AllowAdmin"]);
        public static readonly int DefaultDatabase = Convert.ToInt32(Configuration.config["RedisSetting:DefaultDatabase"]);
        public static readonly int KeepAlive = Convert.ToInt32(Configuration.config["RedisSetting:KeepAlive"]);
        public static readonly string ClientName = Configuration.config["RedisSetting:ClientName"];
        public static readonly string Password = Configuration.config["RedisSetting:Password"];
        public static readonly int ConnectTimeout = Convert.ToInt32(Configuration.config["RedisSetting:ConnectTimeout"]);
    }

    public static class RabbitMQSetting
    {
        public static readonly string Receive_Hostname = Configuration.config["RabbitMQSetting:Receive_Hostname"];
        public static readonly string Receive_Username = Configuration.config["RabbitMQSetting:Receive_Username"];
        public static readonly string Receive_Password = Configuration.config["RabbitMQSetting:Receive_Password"];
        public static readonly int Receive_ReconnectInterval = Convert.ToInt32(Configuration.config["RabbitMQSetting:Receive_ConnectionReconnectInterval"]);
        public static readonly string Receive_ExchangeName = Configuration.config["RabbitMQSetting:Receive_ExchangeName"];
        public static readonly bool Receive_AutoAck = Convert.ToBoolean(Configuration.config["RabbitMQSetting:Receive_AutoAck"]);
        public static readonly string Receive_Queue_ChartUpdate = Configuration.config["RabbitMQSetting:Receive_Queue_ChartUpdateQueue"];
        public static readonly string Receive_Queue_ChartUpdate_RoutingKey = Configuration.config["RabbitMQSetting:Receive_Queue_ChartUpdateQueue_RoutingKey"];                
        public static readonly string Receive_EnabledDelay = Configuration.config["RabbitMQSetting:Receive_EnabledDelay"];

        public static readonly string Send_Hostname = Configuration.config["RabbitMQSetting:Send_Hostname"];
        public static readonly string Send_Username = Configuration.config["RabbitMQSetting:Send_Username"];
        public static readonly string Send_Password = Configuration.config["RabbitMQSetting:Send_Password"];
        public static readonly int Send_ReconnectInterval = Convert.ToInt32(Configuration.config["RabbitMQSetting:Send_ConnectionReconnectInterval"]);
        public static readonly string Send_ExchangeName = Configuration.config["RabbitMQSetting:Send_ExchangeName"];
        //public static readonly string Send_Queue_ChartUpdate = Configuration.config["RabbitMQSetting:Send_Queue_ChartUpdateQueue"];
        public static readonly string Send_Queue_ChartUpdate_RoutingKey_Prefix = Configuration.config["RabbitMQSetting:Send_Queue_ChartUpdateQueue_RoutingKey_PreFix"];        
        //public static readonly string Send_Queue_ChartUpdate_RoutingKey = Configuration.config["RabbitMQSetting:Send_Queue_ChartUpdateQueue_RoutingKey"];        
    }

    //public static class ChartSetting
    //{
    //    public static readonly string[] csvArray = "1m,5m,15m,30m,1H,4H,1D,1W".Split(',');
    //    public static readonly Dictionary<string, string> TimeFrameDictionary = //ConfigurationManager.AppSettings["Chart.TimeFrameDictionary"]
    //                                                                            "1m=60;5m=300;15m=900;30m=1800;1H=3600;4H=14400;1D=86400;1W=604800"
    //                                                                            .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
    //                                                                            .Select(part => part.Split('='))
    //                                                                            .ToDictionary(split => split[0], split => split[1]);
    //
    //    public class TimeFrame
    //    {
    //        public enum Period
    //        {
    //            OneMinute = 60,
    //            FiveMinutes = 300,
    //            FifteenMinutes = 900,
    //            ThirtyMinutes = 1800,
    //            OneHour = 3600,
    //            FourHours = 14400,
    //            OneDay = 86400,
    //            OneWeek = 604800
    //        }
    //    }
    //
    //    public static readonly int Chart_FirstDayOfWeek = Convert.ToInt32(Configuration.config["Chart.FirstDayOfWeek"]);
    //    public static readonly string Chart_EODTime = Configuration.config["Chart.EODTime"];
    //    public static readonly int EOWDay = Convert.ToInt32(Configuration.config["Chart.EOWDay"]);
    //    public static readonly string[] ExtraPointTimeframeIndex = Configuration.config["Chart.ExtraPointTimeframeIndex"].Split(',');
    //    public static readonly string ContinuousEnabledOnly = Configuration.config["Chart.ContinuousEnabledOnly"];
    //
    //}
}
