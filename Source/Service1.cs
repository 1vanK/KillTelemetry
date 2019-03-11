using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;
using System.IO;
using Microsoft.Win32;

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

            // Убираем водяной знак (watermark). Требуется перезагрузка
            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform\Activation");
            if (key != null)
            {
                key.SetValue("Manual", 1);
                key.SetValue("DownlevelActivation", 2);
                key.Close();
            }
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
