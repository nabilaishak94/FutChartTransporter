using System;
using System.Collections.Generic;
using System.Text;
using FutChartTransporter_DotCore.Models.CQMobile;
using FutChartTransporter_DotCore.Repos.CQMobile;

namespace FutChartTransporter_DotCore.Managers.CQMobile
{
    public class TimeZoneManager
    {
        public List<TimeZoneModel> GetTimeZoneList()
        {
            TimeZoneRepo repo = new TimeZoneRepo();
            return repo.GetTimeZoneList();
        }
    }
}
