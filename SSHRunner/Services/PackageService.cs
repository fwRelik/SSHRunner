using fwRelik.SSHSetup.Extensions;
using SSHRunner.Helper;
using SSHRunner.Services.Base;
using System;
using System.Threading;

namespace SSHRunner.Services
{
    internal class PackageService : BaseService
    {
        public PackageControl Service { get; private set; } = new();

        public void Install()
        {
            new Thread(() =>
            {
                try
                {
                    Service.PackageManagment();
                    ServiceStatus = true;
                }
                catch (Exception ex) { ErrorHandler.ServiceNotFound(ex); }
            }).Start();
        }

        public void UnInstall()
        {
            new Thread(() =>
            {
                try
                {
                    Service.PackageManagment(false);
                    ServiceStatus = false;
                }
                catch (Exception ex) { ErrorHandler.ServiceNotFound(ex); }
            }).Start();
        }

        public void CheckServiceState()
        {
            ServiceStatus = Service.GetPackagesState().AllPackageInstalling;
        }

        public override void CheckServiceStatus()
        {
            try
            {
                ServiceStatus = Service.CheckPackageForInitializaitonValue();
            }
            catch (Exception ex) { ErrorHandler.ServiceNotFound(ex); }
        }
    }
}
