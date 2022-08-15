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
    public class InstrumentScheduleRepo
    {
        public List<InstrumentScheduleModel> GetInstrumentScheduleList()
        {
            List<InstrumentScheduleModel> list = new List<InstrumentScheduleModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(DBSetting.ConnectionName_CQMobile))
                {
                    using (SqlCommand cmd = new SqlCommand(SPSetting.SP_GetInstrumentScheduleList, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        InstrumentScheduleModel model;

                        while (reader.Read())
                        {
                            model = new InstrumentScheduleModel();

                            model.InstrumentCode = reader["InstrumentCode"].ToString();
                            model.Day = reader["Day"].ToString();
                            model.Close = reader["Close"].ToString();
                            model.Date = reader["Date"].ToString();
                            model.DayLightSaving = reader["DayLightSaving"].ToString();
                            model.Open = reader["Open"].ToString();
                            model.TimeZoneCode = reader["TimeZoneCode"].ToString();

                            list.Add(model);
                        }

                        conn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Unable to Fetch InstrumentSchedule List. Error : {0}", ex.ToString());
            }

            return list;
        }
    }
}
