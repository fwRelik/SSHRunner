namespace SSHRunner.Models.Base
{
    internal abstract class BaseModel
    {
        public bool ServiceStatus = false;

        public abstract void ServiceStart();
        public abstract void ServiceStop();
        public abstract void CheckServiceStatus();

        protected string GetIndicator()
        {
            return ServiceStatus ? "Green" : "Red";
        }
    }
}
