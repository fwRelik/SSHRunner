using fwRelik.SSHSetup.Extensions;
using SSHRunner.Helper;
using SSHRunner.Services.Base;
using System;

namespace SSHRunner.Services
{
    internal class PackageService : BaseService
    {
        public PackageControl Service { get; private set; } = new();

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
            catch (Exception ex) { ErrorHandler.InsufficientRights(ex); }
        }

        public override void ServiceStart()
        {
            throw new NotImplementedException();
        }

        public override void ServiceStop()
        {
            throw new NotImplementedException();
        }
    }
}
