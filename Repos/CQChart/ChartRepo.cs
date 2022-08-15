using System;
using System.Collections.Generic;
using System.Text;

using FutChartTransporter_DotCore.Models;
using FutChartTransporter_DotCore.Common;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace FutChartTransporter_DotCore.Repos.CQChart
{
    public class ChartRepo
    {

        public void InsertBidChart(ChartRecordModel model, string timeFrame, string tableName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DBSetting.ConnectionName_CQChart))
                {
                    using (SqlCommand cmd = new SqlCommand(SPSetting.SP_InsertHLOCVBid, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@AccountInstrumentGroupId", SqlDbType.VarChar).Value = model.AccountInstrumentGroupID;
                        cmd.Parameters.Add("@SeriesCode", SqlDbType.VarChar).Value = model.SeriesCode;
                        cmd.Parameters.Add("@HighPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.High);
                        cmd.Parameters.Add("@LowPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Low);
                        cmd.Parameters.Add("@OpenPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Open);
                        cmd.Parameters.Add("@ClosePrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Close);
                        cmd.Parameters.Add("@Volume", SqlDbType.Decimal).Value = Convert.ToDecimal(model.CurrentVolume);
                        cmd.Parameters.Add("@UpdateDt", SqlDbType.DateTime).Value = model.UpdateDt;
                        cmd.Parameters.Add("@TimeFrame", SqlDbType.VarChar).Value = timeFrame;
                        cmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tableName;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Insert HLOCVBid into DB Error : {0}", ex.ToString());
            }
        }

        public void InsertAskChart(ChartRecordModel model, string timeFrame, string tableName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DBSetting.ConnectionName_CQChart))
                {
                    using (SqlCommand cmd = new SqlCommand(SPSetting.SP_InsertHLOCVAsk, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@AccountInstrumentGroupId", SqlDbType.VarChar).Value = model.AccountInstrumentGroupID;
                        cmd.Parameters.Add("@SeriesCode", SqlDbType.VarChar).Value = model.SeriesCode;
                        cmd.Parameters.Add("@HighPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.High);
                        cmd.Parameters.Add("@LowPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Low);
                        cmd.Parameters.Add("@OpenPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Open);
                        cmd.Parameters.Add("@ClosePrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Close);
                        cmd.Parameters.Add("@Volume", SqlDbType.Decimal).Value = Convert.ToDecimal(model.CurrentVolume);
                        cmd.Parameters.Add("@UpdateDt", SqlDbType.DateTime).Value = model.UpdateDt;
                        cmd.Parameters.Add("@TimeFrame", SqlDbType.VarChar).Value = timeFrame;
                        cmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tableName;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Insert HLOCVAsk into DB Error : {0}", ex.ToString());
            }
        }

        public void InsertLastChart(ChartRecordModel model, string timeFrame, string tableName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DBSetting.ConnectionName_CQChart))
                {
                    using (SqlCommand cmd = new SqlCommand(SPSetting.SP_InsertHLOCVLast, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@AccountInstrumentGroupId", SqlDbType.VarChar).Value = model.AccountInstrumentGroupID;
                        cmd.Parameters.Add("@SeriesCode", SqlDbType.VarChar).Value = model.SeriesCode;
                        cmd.Parameters.Add("@HighPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.High);
                        cmd.Parameters.Add("@LowPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Low);
                        cmd.Parameters.Add("@OpenPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Open);
                        cmd.Parameters.Add("@ClosePrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Close);
                        cmd.Parameters.Add("@Volume", SqlDbType.Decimal).Value = Convert.ToDecimal(model.CurrentVolume);
                        cmd.Parameters.Add("@UpdateDt", SqlDbType.DateTime).Value = model.UpdateDt;
                        cmd.Parameters.Add("@TimeFrame", SqlDbType.VarChar).Value = timeFrame;
                        cmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tableName;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Insert HLOCVLast into DB Error : {0}", ex.ToString());
            }
        }
        
        public void InsertLastChart1(ChartRecordModel model, string timeFrame, string tableName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DBSetting.ConnectionName_CQChart))
                {
                    using (SqlCommand cmd = new SqlCommand(SPSetting.SP_InsertHLOCVLast, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = $"UPDATE {tableName} SET " +
                            $"HighPrice=@HighPrice, LowPrice=@LowPrice, OpenPrice=@OpenPrice, ClosePrice=@ClosePrice, Volume=@Volume " +
                            $"WHERE SeriesCode=@SeriesCode and AccountInstrumentGroupId=@AccountInstrumentGroupId and " +
                            $"TimeFrame=@TimeFrame and UpdateDt=@UpdateDt " +
                            $"IF @@ROWCOUNT = 0 Begin INSERT INTO {tableName} VALUES(@SeriesCode, @HighPrice, @LowPrice, @OpenPrice, " +
                            $"@ClosePrice, @Volume, @UpdateDt, @AccountInstrumentGroupId, @TimeFrame) End";
                        cmd.Parameters.Add("@AccountInstrumentGroupId", SqlDbType.VarChar).Value = model.AccountInstrumentGroupID;
                        cmd.Parameters.Add("@SeriesCode", SqlDbType.VarChar).Value = model.SeriesCode;
                        cmd.Parameters.Add("@HighPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.High);
                        cmd.Parameters.Add("@LowPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Low);
                        cmd.Parameters.Add("@OpenPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Open);
                        cmd.Parameters.Add("@ClosePrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Close);
                        cmd.Parameters.Add("@Volume", SqlDbType.Decimal).Value = Convert.ToDecimal(model.CurrentVolume);
                        cmd.Parameters.Add("@UpdateDt", SqlDbType.DateTime).Value = model.UpdateDt;
                        cmd.Parameters.Add("@TimeFrame", SqlDbType.VarChar).Value = timeFrame;
                        cmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tableName;

                        conn.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Insert HLOCVLast into DB Error : {0}", ex.ToString());
            }
        }

        public void InsertSettlementLastChart(ChartFeedModel model, string timeFrame, string tableName, DateTime dt)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DBSetting.ConnectionName_CQChart))
                {
                    using (SqlCommand cmd = new SqlCommand(SPSetting.SP_InsertHLOCVLast, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@AccountInstrumentGroupId", SqlDbType.VarChar).Value = model.AccountInstrumentGrpId;
                        cmd.Parameters.Add("@SeriesCode", SqlDbType.VarChar).Value = model.SeriesCode;
                        cmd.Parameters.Add("@HighPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Price);
                        cmd.Parameters.Add("@LowPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Price);
                        cmd.Parameters.Add("@OpenPrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Price);
                        cmd.Parameters.Add("@ClosePrice", SqlDbType.Decimal).Value = Convert.ToDecimal(model.Price);
                        cmd.Parameters.Add("@Volume", SqlDbType.Decimal).Value = 0;
                        cmd.Parameters.Add("@UpdateDt", SqlDbType.DateTime).Value = dt;
                        cmd.Parameters.Add("@TimeFrame", SqlDbType.VarChar).Value = timeFrame;
                        cmd.Parameters.Add("@TableName", SqlDbType.VarChar).Value = tableName;

                        conn.Open();
                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Controller.logger.Error("Insert SettlementLastChart into DB Error : {0}", ex.ToString());
            }
        }
    }
}
