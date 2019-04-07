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

        // Убираем водяной знак (watermark). Требуется перезагрузка.
        // Водяной знак рисует процесс explorer.exe (если его прибить, то водяного знака не будет, впрочем как и рабочего стола)
        void RemoveWatermark()
        {
            RegistryKey key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform\Activation");
            if (key != null)
            {
                key.SetValue("Manual", 0); // Переключение значения туда-обратно помогает спрятать водяной знак
                key.SetValue("Manual", 1);
                key.SetValue("DownlevelActivation", 2);
                key.Close();
            }

            key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform");
            if (key != null)
            {
                key.SetValue("SkipRearm", 1);
                key.Close();
            }

            // Мы не можем напрямую использовать Registry.CurrentUser, так как сервис запускается от админа,
            // а у обычного пользователя другой CurrentUser. Поэтому перебираем всех пользователей
            string[] users = Registry.Users.GetSubKeyNames();
            foreach (string user in users)
            {
                // Вместо HKEY_CURRENT_USER\Control Panel\Desktop перебираем HKEY_USERS\*\Control Panel\Desktop
                key = Registry.Users.CreateSubKey(user + @"\Control Panel\Desktop");
                if (key != null)
                {
                    key.SetValue("PaintDesktopVersion", 1);
                    key.SetValue("PaintDesktopVersion", 0);
                    key.Close();
                }
                // Можно просто выставить PaintDesktopVersion в 1. Надпись с информацией о текущей версии ОС
                // заменит водяной знак. Эта надпись будет на рабочем столе, но не будет
                // перекрывать другие окна. С этим можно жить
            }

            // Блокируем запуск службы C:\WINDOWS\system32\sppsvc.exe
            // (Защита программного обеспечения / Software Protection / Microsoft Software Protection Platform Service)
            key = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\sppsvc");
            if (key != null)
            {
                key.SetValue("Start", 4);
                key.Close();
            }
        }

        protected override void OnStart(string[] args)
        {
            timer.Enabled = true;
            RemoveWatermark();
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
