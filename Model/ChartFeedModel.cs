using System;
using System.Collections.Generic;
using System.Text;

namespace FutChartTransporter_DotCore.Models
{
    public class ChartFeedModel
    {
        public string SeriesCode { get; set; }
        public string AccountInstrumentGrpId { get; set; }
        public decimal Price { get; set; }
        public string Volume { get; set; }
        public string PriceType { get; set; }
        public string UpdatedDt { get; set; }
        public string MessageId { get; set; }
        public string SeqNo { get; set; }

        public override string ToString()
        {
            return $"ChartFeedModel SeriesCode:{SeriesCode} AccountInstrumentGrpId:{AccountInstrumentGrpId} Price:{Price.ToString()} " +
                $"Volume:{Volume} PriceType:{PriceType} UpdatedDt:{UpdatedDt}";
        }

        public string ChartFeedStr()
        {
            return $"{SeriesCode}|{AccountInstrumentGrpId}|{PriceType}|{UpdatedDt}";
        }
    }
}
