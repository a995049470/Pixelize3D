using System;

namespace Lost.Runtime.Footstone.Core
{
    public class ServiceEventArgs : EventArgs
    {
        public ServiceEventArgs(Type serviceType, object serviceInstance)
        {
            ServiceType = serviceType;
            Instance = serviceInstance;
        }

        public Type ServiceType { get; private set; }

        public object Instance { get; private set; }
    }
}



