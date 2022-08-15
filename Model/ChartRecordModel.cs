using System;
using System.Collections.Generic;
using System.Text;

namespace FutChartTransporter_DotCore.Models
{
    public class ChartRecordModel
    {
        public double Open { get; set; } //Open
        //public double CurrentClose = 0;
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public string LastTradeMsg { get; set; }
        public double PreviousVolume { get; set; }
        public double CurrentVolume { get; set; }
        public string SeriesCode { get; set; }
        public string AccountInstrumentGroupID { get; set; }
        public string PriceType { get; set; }
        public DateTime UpdateDt { get; set; }

        public ChartRecordModel()
        {

        }

        public ChartRecordModel(double price, double volume, string seriesCode, string accountInstrumentGroupID, string priceType)
        {
            //this.PreviousClose = 0;
            this.Open = price;
            this.High = price;
            this.Low = price;
            this.Close = price;
            this.CurrentVolume = volume;
            this.PreviousVolume = 0;
            this.SeriesCode = seriesCode;
            this.AccountInstrumentGroupID = accountInstrumentGroupID;
            this.PriceType = priceType;
        }
    }
}
