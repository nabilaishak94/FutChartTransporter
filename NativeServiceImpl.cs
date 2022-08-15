using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;

namespace FutChartTransporter_DotCore
{
    public class NativeServiceImpl : ServiceBase
    {
        Controller cs = new Controller();
        protected override void OnStart(string[] args)
        {
            // The service has been started
            cs.Init();

        }

        protected override void OnStop()
        {
            // The service has been stopped
            cs.Done();
        }

        protected override void OnPause()
        {
            // The service has been paused
            base.OnPause();
        }
    }
}
