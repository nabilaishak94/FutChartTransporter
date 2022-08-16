using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;
using FutChartTransporter_DotCore.Common;

namespace FutChartTransporter_DotCore.Managers.Redis
{
    public class RedisManager
    {
        private static readonly string LOG_CATEGORY = typeof(RedisManager).FullName;
        private ConnectionMultiplexer connection;
        private ConfigurationOptions config = new ConfigurationOptions
        {
            EndPoints = {
                {
                    RedisSetting.Hostname, RedisSetting.Port
                }
            },
            AbortOnConnectFail = RedisSetting.AbortOnConnectFail,
            AllowAdmin = RedisSetting.AllowAdmin,
            DefaultDatabase = RedisSetting.DefaultDatabase,
            KeepAlive = RedisSetting.KeepAlive,
            ClientName = RedisSetting.ClientName,
            Password = RedisSetting.Password,
            DefaultVersion = new Version(3, 2, 100),
            ReconnectRetryPolicy = new LinearRetry(5000),
        };

        private int count = 0;
        private bool attemptingToConnect = false;
        private bool redisConnected = false;

        public void RedisInitialize()
        {
            ConnectToRedis();
        }

        public void ConnectToRedis()
        {
            try
            {
                connection = ConnectionMultiplexer.Connect(this.config);
                connection.ConnectionFailed += Connection_ConnectionFail;

            }
            catch (Exception ex)
            {
                Controller.logger.Error("Unable To Establish The Redis Connection : {0}", ex);
                redisConnected = false;
                ReconnectRedis();
            }
        }

        public void Connection_ConnectionFail(Object o, ConnectionFailedEventArgs e)
        {
            Controller.logger.Info(e.FailureType.ToString() + "|" + e.ConnectionType.ToString() + "|" + e.Exception.Message);
            redisConnected = false;
            ReconnectRedis();
        }

        public void AppendPriceUpdate(string channel, string message)
        {
            try
            {
                if (connection.IsConnected)
                {
                    IDatabase db = connection.GetDatabase(RedisSetting.DefaultDatabase);
                    db.StringAppend(channel, message);
                }
                else
                {
                    ReconnectRedis();
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Unable To Publish Price Update For SeriesCode {0} : {1}", channel, ex);
            }
        }

        private void ReconnectRedis()
        {
            do
            {
                try
                {
                    if (attemptingToConnect) return;
                    attemptingToConnect = true;
                    connection = ConnectionMultiplexer.Connect(this.config);
                    connection.ConnectionFailed += Connection_ConnectionFail;
                    redisConnected = true;
                }
                catch (Exception ex)
                {
                    Controller.logger.Error("Unable To Reconnect Redis Connection : {0}", ex);
                    System.Threading.Thread.Sleep(1000);
                }
                finally
                {
                    attemptingToConnect = false;
                }
            } while (!redisConnected);
        }

        public void Done()
        {
            try
            {
                connection.Close();
                connection.Dispose();
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Unable To Close Redis Connection : {0}", ex);
            }
        }
    }
}
