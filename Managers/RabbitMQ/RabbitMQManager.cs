using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using FutChartTransporter_DotCore.Common;
using FutChartTransporter_DotCore.Models;
using FutChartTransporter_DotCore.Managers.QuestDB;

namespace FutChartTransporter_DotCore.Managers.RabbitMQ
{
    public class RabbitMQManager
    {
        private static IConnection Receive_RabbitConnection = null;
        private static IModel Receive_RabbitChannel = null;
        private static IConnection Send_RabbitConnection = null;
        private static IModel Send_RabbitChannel = null;
        private Dictionary<string, DateTime> timestampChartDictionary = new Dictionary<string, DateTime>();

        public bool InitReceiveRabbitMQConnection()
        {
            bool rabbitmqStatus = false;
            try
            {
                ConnectionFactory connectionFactory = new ConnectionFactory();
                connectionFactory.HostName = RabbitMQSetting.Receive_Hostname;
                connectionFactory.UserName = RabbitMQSetting.Receive_Username;
                connectionFactory.Password = RabbitMQSetting.Receive_Password;
                connectionFactory.AutomaticRecoveryEnabled = true;
                connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(RabbitMQSetting.Receive_ReconnectInterval);
                Receive_RabbitConnection = connectionFactory.CreateConnection();
                Controller.logger.Info("Connected to Receive RabbitMQ Host : {0}", RabbitMQSetting.Receive_Hostname);

                Receive_RabbitConnection.ConnectionShutdown += Connection_ConnectionShutdown_Receive;
                rabbitmqStatus = true;
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Unable To Create Receive RabbitMQ Connection, Kindly Verify and Restart The Service : {0}", ex);
            }

            return rabbitmqStatus;
        }

        public bool InitSendRabbitMQConnection()
        {
            bool rabbitmqStatus = false;
            try
            {
                ConnectionFactory connectionFactory = new ConnectionFactory();
                connectionFactory.HostName = RabbitMQSetting.Send_Hostname;
                connectionFactory.UserName = RabbitMQSetting.Send_Username;
                connectionFactory.Password = RabbitMQSetting.Send_Password;
                connectionFactory.AutomaticRecoveryEnabled = true;
                connectionFactory.NetworkRecoveryInterval = TimeSpan.FromSeconds(RabbitMQSetting.Send_ReconnectInterval);
                Send_RabbitConnection = connectionFactory.CreateConnection();
                Controller.logger.Info("Connected to Send RabbitMQ Host : {0}", RabbitMQSetting.Send_Hostname);

                Send_RabbitConnection.ConnectionShutdown += Connection_ConnectionShutdown_Send;
                rabbitmqStatus = true;
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Unable To Create Send RabbitMQ Connection, Kindly Verify and Restart The Service : {0}", ex);
            }

            return rabbitmqStatus;
        }

        public void Connection_ConnectionShutdown_Receive(Object o, ShutdownEventArgs e)
        {
            if (e.ReplyCode.ToString() == "200")
            {
                //Proper Shutdown
                Controller.logger.Info("Transporter Listener Stopped");
            }
            else
            {
                //Host Down Or Other Reason
                Controller.logger.Error("Transporter Listener Down! Hostname : {0} , Reason : {1}", RabbitMQSetting.Receive_Hostname, e.ReplyText.ToString());
            }
        }

        public void Connection_ConnectionShutdown_Send(Object o, ShutdownEventArgs e)
        {
            if (e.ReplyCode.ToString() == "200")
            {
                //Proper Shutdown
                Controller.logger.Info("Transporter Listener Stopped");
            }
            else
            {
                //Host Down Or Other Reason
                Controller.logger.Error("Transporter Listener Down! Hostname : {0} , Reason : {1}", RabbitMQSetting.Send_Hostname, e.ReplyText.ToString());
            }
        }

        public void InitReceiveRabbitMQChannel()
        {
            try
            {
                Receive_RabbitChannel = Receive_RabbitConnection.CreateModel();

                if (RabbitMQSetting.Receive_EnabledDelay == "1")
                {
                    Dictionary<string, object> arg = new Dictionary<string, object>();
                    arg.Add("x-delayed-type", "topic");

                    Receive_RabbitChannel.ExchangeDeclare(exchange: RabbitMQSetting.Receive_ExchangeName,
                                            type: "x-delayed-message",
                                            durable: true,
                                            autoDelete: false,
                                            arguments: arg);
                }
                else
                {
                    Receive_RabbitChannel.ExchangeDeclare(exchange: RabbitMQSetting.Receive_ExchangeName,
                                    type: "topic",
                                    durable: true,
                                    autoDelete: false,
                                    arguments: null);
                }

                Receive_RabbitChannel.QueueDeclare(queue: RabbitMQSetting.Receive_Queue_ChartUpdate,
                                       durable: true,
                                       exclusive: false,
                                       autoDelete: false,
                                       arguments: null);

                Receive_RabbitChannel.QueueBind(queue: RabbitMQSetting.Receive_Queue_ChartUpdate,
                                exchange: RabbitMQSetting.Receive_ExchangeName,
                                routingKey: RabbitMQSetting.Receive_Queue_ChartUpdate_RoutingKey);

                Controller.logger.Info("RabbitMQ Channel Created.");
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Unable To Create RabbitMQ Channel : {0}", ex);
            }
        }

        public void InitSendRabbitMQChannel()
        {
            try
            {
                Send_RabbitChannel = Send_RabbitConnection.CreateModel();

                //Send_RabbitChannel.ExchangeDeclare(exchange: RabbitMQSetting.Send_ExchangeName,
                //                    type: "topic",
                //                    durable: true,
                //                    autoDelete: false,
                //                    arguments: null);
                //
                //Send_RabbitChannel.QueueDeclare(queue: RabbitMQSetting.Send_Queue_ChartUpdate,
                //                       durable: true,
                //                       exclusive: false,
                //                       autoDelete: false,
                //                       arguments: null);
                //
                //Send_RabbitChannel.QueueBind(queue: RabbitMQSetting.Send_Queue_ChartUpdate,
                //                exchange: RabbitMQSetting.Send_ExchangeName,
                //                routingKey: RabbitMQSetting.Send_Queue_ChartUpdate_RoutingKey);

                Controller.logger.Info("Send RabbitMQ Channel Created.");
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Unable To Create Send RabbitMQ Channel : {0}", ex);
            }
        }

        public void StartReceiveChart()
        {
            Controller.logger.Info("Start listening to " + RabbitMQSetting.Receive_Queue_ChartUpdate);
            var consumer = new EventingBasicConsumer(Receive_RabbitChannel);

            Receive_RabbitChannel.BasicQos(0, 10, false);

            consumer.Received += (o, e) =>
            {
                try
                {
                    string[] messageIdArr = e.BasicProperties.MessageId.Split('|');
                    string message = Encoding.ASCII.GetString(e.Body.ToArray());

                    Int64 seqnoint = GetHeaderSeqNo(e);

                    if (AppSetting.EnableLog == "1")
                        Controller.logger.Info($"Received Feed Msg: {Encoding.ASCII.GetString(e.Body.ToArray())} MsgId: {e.BasicProperties.MessageId}");

                    if (messageIdArr.Length != 2 && messageIdArr.Length != 3)
                    {
                        Controller.logger.Info("Received Invalid MessageID : {0}", e.BasicProperties.MessageId);
                    }
                    else
                    {
                        string[] messageArr = message.Split('|');
                        ProcessChartData(messageArr, messageIdArr, seqnoint);
                    }

                }
                catch (Exception ex)
                {
                    Controller.logger.Error("Error occur in StartReceiveChart. Error : {0}", ex);
                }
                finally
                {
                    if (!RabbitMQSetting.Receive_AutoAck)
                        Receive_RabbitChannel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
                }
            };
            Receive_RabbitChannel.BasicConsume(queue: RabbitMQSetting.Receive_Queue_ChartUpdate,
                                       autoAck: RabbitMQSetting.Receive_AutoAck,
                                       consumer: consumer);            
        }

        private Int64 GetHeaderSeqNo(BasicDeliverEventArgs e)
        {
            int headerkeyindex = -1;
            string seqno = "";
            Int64 seqnoint = -1;

            if (e.BasicProperties.Headers != null)
            {                
                for (int i = 0; i < e.BasicProperties.Headers.Keys.Count; i++)
                {
                    if (e.BasicProperties.Headers.Keys.ElementAt(i) == "SeqNo")
                    {
                        headerkeyindex = i;
                        break;
                    }
                }

                if (headerkeyindex != -1)
                {
                    seqno = GetSeqNoStr(e.BasicProperties.Headers.ElementAt(headerkeyindex));

                    if (!string.IsNullOrEmpty(seqno))
                        seqnoint = Convert.ToInt64(seqno);
                }
            }
            return seqnoint;
        }

        private string GetSeqNoStr(KeyValuePair<string, object> e)
        {
            if (e.Value is BinaryTableValue)
            {
                BinaryTableValue v = (BinaryTableValue)e.Value;
                return Encoding.UTF8.GetString(v.Bytes);
            }
            else if (e.Value is AmqpTimestamp)
            {
                AmqpTimestamp v = (AmqpTimestamp)e.Value;
                return v.UnixTime.ToString();
            }
            else if (e.Value is byte[])
            {
                byte[] seqnobyte = (byte[])e.Value;
                return Encoding.ASCII.GetString(seqnobyte);
            }
            else if (e.Value is int)
            {
                int seqnum = (int)e.Value;
                return seqnum.ToString();
            }
            else if (e.Value is long)
            {
                long seqnum = (long)e.Value;
                return seqnum.ToString();
            }
            else
            {
                Controller.logger.Info("Unknow Message SeqNo Header type. " + e.Value);
                return "";
            }
        }

        public void SendChartFeed(ChartFeedModel model)
        {
            IBasicProperties properties = Send_RabbitChannel.CreateBasicProperties();
            properties.MessageId = model.MessageId;
            properties.DeliveryMode = 2;

            properties.Headers = new Dictionary<string, object>();            
            properties.Headers.Add("SeqNo", model.SeqNo);

            byte[] message = Encoding.UTF8.GetBytes(model.ChartFeedStr());
            
                Send_RabbitChannel.BasicPublish(exchange: RabbitMQSetting.Send_ExchangeName,
                routingKey: RabbitMQSetting.Send_Queue_ChartUpdate_RoutingKey_Prefix + model.SeriesCode.Split('.')[1],
                basicProperties: properties,
                body: message);

            if (AppSetting.EnableLog == "1")
                Controller.logger.Info($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffffff")} Sent SeqNo: {model.SeqNo} MsgID: {model.MessageId} Msg: {model.ChartFeedStr()}");

        }

        public void ProcessChartData(string[] messageArr, string[] messageIdArr, Int64 seqNo)
        {
            ChartFeedModel chartFeedModel = new ChartFeedModel();
            chartFeedModel = AssignChartModel(chartFeedModel, messageArr, messageIdArr, seqNo);
            //Controller.chartFeedManager.ProcessChartFeed(chartFeedModel);
            Controller.chartFeedManager.ProcessChartFeedStoreToRedis(chartFeedModel);

            //Controller.ChartFeedQueue.Enqueue(chartFeedModel);

            //LineTcpSenderManager LineTcpSenderManager = new LineTcpSenderManager(DBSetting.QuestDB_Hostname, DBSetting.QuestDB_Port);
            //Controller.chartFeedManager.ProcessChartFeed(chartFeedModel);
            //Controller.chartFeedManager.ProcessChartFeed(chartFeedModel, LineTcpSenderManager);
        }

        private ChartFeedModel AssignChartModel(ChartFeedModel chartFeedModel, string[] messageArr, string[] messageIdArr, Int64 seqNo)
        {
            if (messageIdArr.Length == 2)
            {
                chartFeedModel.AccountInstrumentGrpId = "NA";
                chartFeedModel.SeriesCode = messageIdArr[1];
            }
            else if (messageIdArr.Length == 3)
            {
                chartFeedModel.AccountInstrumentGrpId = messageIdArr[1];
                chartFeedModel.SeriesCode = messageIdArr[2];
            }

            chartFeedModel.Price = Convert.ToDecimal(messageArr[0]);
            chartFeedModel.PriceType = messageArr[1];
            chartFeedModel.Volume = messageArr[2];
            chartFeedModel.UpdatedDt = messageArr[3];
            chartFeedModel.MessageId = string.Join("|", messageIdArr);
            chartFeedModel.SeqNo = seqNo.ToString();

            return chartFeedModel;
        }
        
        public void DoneRabbitMQConnection()
        {
            try
            {
                Receive_RabbitChannel.Close();
                Send_RabbitChannel.Close();

                Receive_RabbitChannel.Dispose();
                Send_RabbitChannel.Dispose();

                Receive_RabbitConnection.Close();
                Send_RabbitConnection.Close();

                Receive_RabbitConnection.Dispose();
                Send_RabbitConnection.Dispose();
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Failed To Disconnect From RabbitMQ : {0}", ex);
            }
        }
    }
}
