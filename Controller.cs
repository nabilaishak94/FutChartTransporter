using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

using FutChartTransporter_DotCore.Common;
using FutChartTransporter_DotCore.Managers;
using FutChartTransporter_DotCore.Managers.RabbitMQ;
using FutChartTransporter_DotCore.Managers.Redis;
using FutChartTransporter_DotCore.Managers.QuestDB;
using FutChartTransporter_DotCore.Models;
using System.Threading;
using System.Collections.Concurrent;

namespace FutChartTransporter_DotCore
{
    public class Controller
    {
        public static Logger logger;

        public static RabbitMQManager rabbitMQManager = new RabbitMQManager();        
        public static ThreadManager threadManager = new ThreadManager();
        public static ChartFeedManager chartFeedManager = new ChartFeedManager();
        public static LineTcpSenderManager LineTcpSenderManager = new LineTcpSenderManager();
        public static RedisManager RedisManager = new RedisManager();
        public static ConcurrentQueue<ChartFeedModel> ChartFeedQueue = new ConcurrentQueue<ChartFeedModel>();

        public static object lockLineTcpSenderManager = new object();

        Thread t;

        public void Init()
        {
            InitLogger();

            //LineTcpSenderManager = new LineTcpSenderManager();
            RedisManager.RedisInitialize();

            rabbitMQManager.InitSendRabbitMQConnection();
            rabbitMQManager.InitSendRabbitMQChannel();
            rabbitMQManager.InitReceiveRabbitMQConnection();
            rabbitMQManager.InitReceiveRabbitMQChannel();
            
            //t = new Thread(new ThreadStart(threadManager.StartThread));
            //t.Start();
            
            rabbitMQManager.StartReceiveChart();
        }

        private static IServiceProvider BuildDi(IConfiguration config)
        {
            return new ServiceCollection()
               .AddTransient<LoggerService>() // LoggerService is the custom class
               .AddLogging(loggingBuilder =>
               {
                   // configure Logging with NLog
                   loggingBuilder.ClearProviders();
                   loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   loggingBuilder.AddNLog(config);
               })
               .BuildServiceProvider();
        }

        public void InitLogger()
        {            
            try
            {
                logger = LogManager.GetCurrentClassLogger();
                var config = new ConfigurationBuilder()
                   .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                   .Build();

                var servicesProvider = BuildDi(config);
                var runner = servicesProvider.GetRequiredService<LoggerService>();

                logger.Info("Initialized Logger Service.");
            }
            catch (Exception ex)
            {
                System.Environment.Exit(1);
                throw;
            }
        }

        public void Done()
        {
            rabbitMQManager.DoneRabbitMQConnection();
            //LineTcpSenderManager.StopSocket();
            LogManager.Shutdown();
        }
    }
}
