using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using FutChartTransporter_DotCore.Models.CQMobile;
using FutChartTransporter_DotCore.Repos.CQMobile;
using FutChartTransporter_DotCore.Common;

namespace FutChartTransporter_DotCore.Managers.CQMobile
{
    public class ContinuousChartMappingManager
    {
        public List<ContinuousChartMappingModel> GetContinuousChartMappingList()
        {
            ContinuousChartMappingRepo repo = new ContinuousChartMappingRepo();
            List<ContinuousChartMappingModel> list = repo.GetContinuousChartMappingList();
            return list.Where(x => x.TransporterCode == RabbitMQSetting.TransporterCode).ToList();
        }
    }
}
