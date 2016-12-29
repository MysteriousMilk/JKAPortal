using JKAPortal.API.Q3;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace JKAPortal.Model.Q3
{
    public class Q3MasterServer : IQ3MasterServer
    {
        private const int _DefaultPort = 29060;

        public List<IPEndPoint> RegisteredServers
        {
            get;
            set;
        }

        public Q3MasterServer()
        {

        }

        public Q3MasterServer(string hostname)
        {
            Ping(hostname);
        }

        public void Ping(string hostname)
        {
            RegisteredServers = new List<IPEndPoint>();
            GetRegisteredServers(hostname);
        }

        public IEnumerable<IQ3Server> GetServers()
        {
            foreach(IPEndPoint ip in RegisteredServers)
            {
                IQ3Server server = Q3Server.GetServer(ip);

                if (server != null)
                    yield return server;
            }
        }

        private void GetRegisteredServers(string hostname)
        {
            IPEndPoint serveraddr = GetAddress(hostname, _DefaultPort);
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);

            byte[] buffer;
            byte[] ip = new byte[6];
            int index;
            int count;

            byte[] request = Encoding.ASCII.GetBytes("\u00FF\u00FF\u00FF\u00FFgetservers 26 full empty");

            using (var client = new UdpClient(_DefaultPort))
            {
                client.Send(request, request.Length, serveraddr);

                do
                {
                    buffer = client.Receive(ref sender);

                    index = 0;
                    do
                    {
                        index++;
                    }
                    while (buffer[index] != '\0');

                    /*
                    byte[] data = new byte[buffer.Length - index];
                    for (int i = 0; i < data.Length; i++)
                        data[i] = buffer[i + index];*/

                    index += 2;

                    count = 0;
                    for (int i = index; i < buffer.Length - 3; i++)
                    {
                        ip[count] = buffer[i];
                        count++;

                        if (count == 5)
                        {
                            i++;
                            ip[count] = buffer[i];
                            RegisteredServers.Add(DecodeIPAddress(ip));
                            count = 0;
                            i++;
                        }
                    }

                    int len = buffer.Length;
                    if (buffer[len - 3] == 'E' && buffer[len - 2] == 'O' && buffer[len - 1] == 'F')
                        break;
                }
                while (buffer.Length > 0);
            }
        }

        private IPEndPoint GetAddress(string hostname, int port)
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(hostname);
            IPAddress[] addr = ipEntry.AddressList;
            return new IPEndPoint(addr[0], port);
        }

        private IPEndPoint DecodeIPAddress(byte[] data)
        {
            if (data.Length != 6)
                throw new ArgumentException("IP data must be exactly 6 bytes long.");

            int port;
            byte[] addrBytes = new byte[4];

            addrBytes[0] = data[0];
            addrBytes[1] = data[1];
            addrBytes[2] = data[2];
            addrBytes[3] = data[3];
            port = (data[4] * 256) + data[5];

            return new IPEndPoint(new IPAddress(addrBytes), port);
        }
    }
}
