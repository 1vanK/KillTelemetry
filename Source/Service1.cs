using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using System.IO;

namespace KillTelemetry
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer;

        public Service1()
        {
            InitializeComponent();

            timer = new Timer(1000);
            timer.Elapsed += OnTimedEvent;
        }

        protected override void OnStart(string[] args)
        {
            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            timer.Enabled = false;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if (process.ProcessName == "CompatTelRunner")
                {
                    try { process.Kill(); } catch { }
                    //File.AppendAllText(@"c:\temp\KillTelemetry.txt", DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff") + Environment.NewLine);
                }
            }
        }
    }
}
