using System;
using System.Collections.Generic;
using System.Text;
using FutChartTransporter_DotCore.Models.CQMobile;
using FutChartTransporter_DotCore.Repos.CQMobile;


namespace FutChartTransporter_DotCore.Managers.CQMobile
{
    public class TransporterSubscribeMapManager
    {
        public List<TransporterSubscribeMapModel> GetTransporterSubscribeMapList()
        {
            TransporterSubscribeMapRepo repo = new TransporterSubscribeMapRepo();
            return repo.GetTransporterSubMap();
        }
    }
}
