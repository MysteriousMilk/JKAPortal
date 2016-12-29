using JKAPortal.API.Q3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace JKAPortal.Model.Q3
{
    public class Q3Server : IQ3Server
    {
        public string Name { get { return Q3Utilities.GetUnformattedName(FormattedName); } }
        public string FormattedName { get; set; }
        public string MapName { get; set; }
        public long Ping { get; set; }
        public List<IQ3Player> Players { get; set; }

        public int PlayerCount
        {
            get
            {
                if (Players != null)
                    return Players.Count;
                return 0;
            }
        }

        private Dictionary<string, object> _cvarTable;

        internal Q3Server(string formattedName)
        {
            _cvarTable = new Dictionary<string, object>();
            Players = new List<IQ3Player>();

            FormattedName = formattedName;
            MapName = string.Empty;
        }

        public Q3Server() : this(string.Empty)
        {

        }

        public static IQ3Server GetServer(IPEndPoint serverIp)
        {
            Stopwatch timer = new Stopwatch();
            Q3Server server = new Q3Server();
            string response = string.Empty;

            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] request = { 0xFF, 0xFF, 0xFF, 0xFF, 0x67, 0x65, 0x74, 0x73, 0x74, 0x61, 0x74, 0x75, 0x73, 0x0A };

            using (var client = new UdpClient())
            {
                client.Connect(serverIp);

                timer.Start();

                client.Client.SendTimeout = 5000;
                client.Send(request, request.Length);

                try
                {
                    client.Client.ReceiveTimeout = 750;
                    byte[] buffer = client.Receive(ref serverIp);

                    timer.Stop();

                    response += Encoding.UTF8.GetString(buffer);
                }
                catch (SocketException)
                {
                    return null;
                }
            }

            server.Ping = timer.ElapsedMilliseconds;

            string[] serverInfo = response.Split('\n');

            if (serverInfo.Length > 2)
            {
                for (int i = 2; i < serverInfo.Length; i++)
                {
                    string[] playerInfo = serverInfo[i].Split(' ');
                    if (playerInfo.Length == 3)
                    {
                        server.AddPlayer(new Q3Player(
                            playerInfo[2].Replace("\"", ""),
                            Convert.ToInt32(playerInfo[0]),
                            Convert.ToInt32(playerInfo[1])
                            ));
                    }
                }
            }
            else
                Trace.WriteLine("No Player Info found for server [" + serverIp.ToString() + "]");

            if (serverInfo.Length > 1)
            {
                string[] cvarInfo = serverInfo[1].Split('\\');

                string key = string.Empty;
                bool isKey = true;

                foreach (string cvar in cvarInfo)
                {
                    if (cvar == string.Empty)
                        continue;

                    if (isKey)
                        key = cvar;
                    else
                        server.AddServerVariable(key.ToLower(), cvar);

                    isKey = !isKey;
                }
            }
            else
                Trace.WriteLine("Invalid Server Info");

            return server;
        }

        public bool HasCvar(string cvar)
        {
            return _cvarTable.ContainsKey(cvar);
        }

        public object GetCvar(string cvar)
        {
            object cvarValue = null;
            _cvarTable.TryGetValue(cvar.ToLower(), out cvarValue);
            return cvarValue;
        }

        public void AddServerVariable(string cvar, object cvarValue)
        {
            if (_cvarTable.ContainsKey(cvar))
                throw new ArgumentException("Cannot add the same cvar twice.", "cvar");

            if (cvar.ToLower().Equals("sv_hostname"))
                FormattedName = Q3Utilities.ResolveName(cvarValue as string);

            if (cvar.Equals("mapname"))
                MapName = (string)cvarValue;

            _cvarTable.Add(cvar, cvarValue);
        }

        public void AddPlayer(IQ3Player player)
        {
            if (player == null)
                throw new ArgumentNullException("player");

            Players.Add(player);
        }
    }
}
