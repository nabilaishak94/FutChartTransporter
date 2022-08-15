using System;
using System.Collections.Generic;
using System.Text;

namespace FutChartTransporter_DotCore.Models.CQMobile
{
    public class ContinuousChartMappingModel
    {
        public string SubscribeCode { set; get; }
        public string SeriesCode { set; get; }
        public string TransporterCode { set; get; }
        public string ComputationCode { set; get; }
        public string PlatformSubscriptionCode { set; get; }
        public string RoutingKey { set; get; }
    }
}
