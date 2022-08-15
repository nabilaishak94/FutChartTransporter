using System;
using System.Collections.Generic;
using System.Text;


using FutChartTransporter_DotCore.Models.CQMobile;
using FutChartTransporter_DotCore.Common;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace FutChartTransporter_DotCore.Repos.CQMobile
{
    public class TransporterSubscribeMapRepo
    {
        public List<TransporterSubscribeMapModel> GetTransporterSubMap()
        {
            List<TransporterSubscribeMapModel> transporterSubscribeMapList = new List<TransporterSubscribeMapModel>();

            try
            {                
                using (SqlConnection conn = new SqlConnection(DBSetting.ConnectionName_CQMobile))
                {
                    using (SqlCommand cmd = new SqlCommand(SPSetting.SP_GetTransporterSubMap, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TransporterCode", RabbitMQSetting.TransporterCode);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        TransporterSubscribeMapModel transporterSubscribeMapModel;

                        while (reader.Read())
                        {
                            transporterSubscribeMapModel = new TransporterSubscribeMapModel();

                            transporterSubscribeMapModel.TransporterCode = reader["TransporterCode"].ToString();
                            transporterSubscribeMapModel.InstrumentCode = reader["InstrumentCode"].ToString();
                            transporterSubscribeMapModel.SeriesCode = reader["SeriesCode"].ToString();
                            transporterSubscribeMapModel.SubscribeQueue = reader["SubscribeQueue"].ToString();

                            transporterSubscribeMapList.Add(transporterSubscribeMapModel);
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Unable to Fetch Transporter Subscribe Map. Error : {0}", ex.ToString());
            }

            return transporterSubscribeMapList;
        }
    }
}
