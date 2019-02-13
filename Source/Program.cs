/*
    https://metanit.com/sharp/tutorial/21.1.php
    https://metanit.com/sharp/tutorial/21.2.php
*/

using System.ServiceProcess;

namespace KillTelemetry
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
