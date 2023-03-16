using System;

namespace SSHRunner.Services.Base
{
    internal abstract class BaseService
    {
        public bool ServiceStatus = false;

        public virtual void ServiceStart() { throw new NotSupportedException(); }
        public virtual void ServiceStop() { throw new NotSupportedException(); }
        public abstract void CheckServiceStatus();

        public string GetIndicator()
        {
            return ServiceStatus ? "Lime" : "DarkRed";
        }
    }
}
