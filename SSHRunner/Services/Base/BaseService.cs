namespace SSHRunner.Services.Base
{
    internal abstract class BaseService
    {
        public bool ServiceStatus = false;

        public abstract void ServiceStart();
        public abstract void ServiceStop();
        public abstract void CheckServiceStatus();

        public string GetIndicator()
        {
            return ServiceStatus ? "Lime" : "DarkRed";
        }
    }
}
