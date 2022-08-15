using System;
using System.Collections.Generic;
using System.Text;

namespace FutChartTransporter_DotCore.Models.CQMobile
{
    public class TransporterSubscribeMapModel
    {
        public string TransporterCode { set; get; }
        public string InstrumentCode { set; get; }
        public string SeriesCode { set; get; }
        public string SubscribeQueue { set; get; }
    }
}
