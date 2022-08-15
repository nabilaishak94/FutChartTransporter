using System;
using System.ServiceProcess;

namespace FutChartTransporter_DotCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Controller cs = new Controller();
            cs.Init();
            Console.Read();

            //ServiceBase.Run(new NativeServiceImpl());
        }
    }
}
