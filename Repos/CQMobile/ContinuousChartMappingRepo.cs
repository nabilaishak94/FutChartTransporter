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
    public class ContinuousChartMappingRepo
    {
        public List<ContinuousChartMappingModel> GetContinuousChartMappingList()
        {
            List<ContinuousChartMappingModel> list = new List<ContinuousChartMappingModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBSetting.ConnectionName_CQMobile))
                {
                    using (SqlCommand cmd = new SqlCommand(SPSetting.SP_GetContinuousChartMappingList, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        ContinuousChartMappingModel model;

                        while (reader.Read())
                        {
                            model = new ContinuousChartMappingModel();

                            model.ComputationCode = reader["ComputationCode"].ToString();
                            model.PlatformSubscriptionCode = reader["PlatformSubscriptionCode"].ToString();
                            model.SeriesCode = reader["SeriesCode"].ToString();
                            model.SubscribeCode = reader["SubscribeCode"].ToString();
                            model.TransporterCode = reader["TransporterCode"].ToString();
                            model.RoutingKey = reader["RoutingKey"].ToString();

                            list.Add(model);
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Unable to Fetch ContinuousChartMapping List. Error : {0}", ex.ToString());
            }

            return list;
        }
    }
}
