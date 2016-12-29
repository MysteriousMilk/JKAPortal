using JKAPortal.API.Core;
using JKAPortal.API.Q3;
using JKAPortal.Model.Q3;
using System;
using System.Net;

namespace JKAPortal.Core
{
    public sealed class Q3Utilities : IQ3Utilities
    {
        public T GetInstance<T>()
        {
            Type instanceType = typeof(T);

            if (instanceType.Equals(typeof(IQ3MasterServer)))
            {
                IQ3MasterServer instance = new Q3MasterServer();
                return (T)instance;
            }
            else if (instanceType.Equals(typeof(IQ3Server)))
            {
                IQ3Server instance = new Q3Server();
                return (T)instance;
            }
            else if (instanceType.Equals(typeof(IQ3Player)))
            {
                IQ3Player instance = new Q3Player();
                return (T)instance;
            }

            return default(T);
        }

        public IQ3Server GetServer(IPEndPoint serverIp)
        {
            return Q3Server.GetServer(serverIp);
        }

        public string GetUnformattedName(string formattedName)
        {
            return Model.Q3.Q3Utilities.GetUnformattedName(formattedName);
        }

        public string ResolveName(string name)
        {
            return Model.Q3.Q3Utilities.ResolveName(name);
        }
    }
}
