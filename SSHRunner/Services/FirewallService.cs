using fwRelik.SSHSetup.Extensions;
using SSHRunner.Helper;
using SSHRunner.Services.Base;
using System;

namespace SSHRunner.Services
{
    internal class FirewallService : BaseService
    {
        public FirewallRuleControl Service { get; private set; } = new();

        public override void CheckServiceStatus()
        {
            try
            {
                ServiceStatus = Service.GetFirewallRule().FirewallRuleStatus;
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
