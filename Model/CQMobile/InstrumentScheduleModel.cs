using System;
using System.Collections.Generic;
using System.Text;

namespace FutChartTransporter_DotCore.Models.CQMobile
{
    public class InstrumentScheduleModel
    {
        public string InstrumentCode { set; get; }
        public string Open { set; get; }
        public string Close { set; get; }
        public string DayLightSaving { set; get; }
        public string TimeZoneCode { set; get; }
        public string Day { set; get; }
        public string Date { set; get; }
    }
}
