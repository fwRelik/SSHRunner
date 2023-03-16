using fwRelik.SSHSetup.Extensions;
using SSHRunner.Helper;
using SSHRunner.Services.Base;
using System;
using System.Threading;

namespace SSHRunner.Services
{
    internal class FirewallService : BaseService
    {
        public FirewallRuleControl Service { get; private set; } = new();

        public void Set()
        {
            new Thread(() =>
            {
                try
                {
                    Service.SetFirewallRule();
                    ServiceStatus = true;
                }
                catch (Exception ex) { ErrorHandler.HasAlready(ex); }
            }).Start();
        }

        public void Remove()
        {
            new Thread(() =>
            {
                try
                {
                    Service.RemoveFirewallRule();
                    ServiceStatus = false;
                }
                catch (Exception ex) { ErrorHandler.HasAlready(ex); }
            }).Start();
        }

        public override void CheckServiceStatus()
        {
            try
            {
                ServiceStatus = Service.GetFirewallRule().FirewallRuleStatus;
            }
            catch (Exception ex) { ErrorHandler.ServiceNotFound(ex); }
        }
    }
}
