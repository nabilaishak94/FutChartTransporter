using System;
using System.Collections.Generic;
using System.Text;
using FutChartTransporter_DotCore.Models.CQMobile;
using FutChartTransporter_DotCore.Repos.CQMobile;

namespace FutChartTransporter_DotCore.Managers.CQMobile
{
    public class InstrumentScheduleManager
    {
        private static readonly string LOG_CATEGORY = typeof(InstrumentScheduleManager).FullName;

        public List<InstrumentScheduleModel> GetInstrumentScheduleList()
        {
            InstrumentScheduleRepo repo = new InstrumentScheduleRepo();
            return repo.GetInstrumentScheduleList();
        }
    }
}
