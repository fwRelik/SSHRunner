using fwRelik.SSHSetup.Enums;
using fwRelik.SSHSetup.Extensions;
using SSHRunner.Helper;
using SSHRunner.Services.Base;
using System;
using System.Windows;

namespace SSHRunner.Services
{
    internal class SSHService : BaseService
    {
        public SSHServiceControl Service { get; private set; } = new();

        public override void CheckServiceStatus()
        {
            try
            {
                Service.GetServiceStatus();
                ServiceStatus = (Service.ServiceStatus == SSHServiceState.Running);
            }
            catch (Exception ex) { ErrorHandler.ServiceNotFound(ex); }
}

        public override void ServiceStart()
        {
            try
            {
                Service.Start();
                ServiceStatus = (Service.ServiceStatus == SSHServiceState.Running);
            }
            catch (Exception ex) { ErrorHandler.ServiceNotFound(ex); }
        }

        public override void ServiceStop()
        {
            try
            {
                Service.Stop();
                ServiceStatus = (Service.ServiceStatus == SSHServiceState.Running);
            }
            catch (Exception ex) { ErrorHandler.ServiceNotFound(ex); }
        }
    }
}
