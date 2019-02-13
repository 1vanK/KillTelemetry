using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;

namespace KillTelemetry
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        ServiceInstaller serviceInstaller;
        ServiceProcessInstaller processInstaller;

        public Installer1()
        {
            InitializeComponent();

            serviceInstaller = new ServiceInstaller();
            processInstaller = new ServiceProcessInstaller();

            processInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.ServiceName = "KillTelemetry";
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
