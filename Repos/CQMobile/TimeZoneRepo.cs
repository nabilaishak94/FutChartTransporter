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
    public class TimeZoneRepo
    {
        public List<TimeZoneModel> GetTimeZoneList()
        {
            List<TimeZoneModel> list = new List<TimeZoneModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBSetting.ConnectionName_CQMobile))
                {
                    using (SqlCommand cmd = new SqlCommand(SPSetting.SP_GetTimeZoneList, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        TimeZoneModel model;

                        while (reader.Read())
                        {
                            model = new TimeZoneModel();

                            model.TimeZoneCode = reader["TimeZoneCode"].ToString();
                            model.TimeZoneName = reader["TimeZoneName"].ToString();
                            model.DayLightSaving = reader["DayLightSaving"].ToString();
                            model.Remarks = reader["Remarks"].ToString();

                            list.Add(model);
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Unable to Fetch TimeZone List. Error : {0}", ex.ToString());
            }

            return list;
        }
    }
}
