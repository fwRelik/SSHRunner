using System;
using fwRelik.SSHSetup.Extensions;
using SSHRunner.Services.Base;

namespace SSHRunner.Services
{
    internal class NetworkService : BaseService
    {
        public NetworkInfo Service { get; private set; } = new();

        public override void CheckServiceStatus()
        {
            ServiceStatus = Service.GetNetworkConnectionStatus();
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
