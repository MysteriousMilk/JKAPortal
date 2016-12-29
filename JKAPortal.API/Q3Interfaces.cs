using System.Collections.Generic;
using System.ComponentModel;
using System.Net;

namespace JKAPortal.API.Q3
{
    public enum JKAGameType
    {
        [Description("Unknown")]
        Unknown = -1,
        [Description("Free For All")]
        FFA,
        [Description("Holocron")]
        Holocron,
        [Description("Jedi Master")]
        JediMaster,
        [Description("Duel")]
        Duel,
        [Description("Power Duel")]
        PowerDuel,
        [Description("Single Player")]
        SinglePlayer,
        [Description("Team FFA")]
        TeamFFA,
        [Description("Siege")]
        Siege,
        [Description("Capture the Flag")]
        CTF
    }

    public interface IQ3MasterServer
    {
        List<IPEndPoint> RegisteredServers { get; set; }

        void Ping(string hostname);
        IEnumerable<IQ3Server> GetServers();
    }

    public interface IQ3Server
    {
        string FormattedName { get; set; }
        string MapName { get; set; }
        string Name { get; }
        long Ping { get; set; }
        int PlayerCount { get; }
        List<IQ3Player> Players { get; set; }

        void AddPlayer(IQ3Player player);
        void AddServerVariable(string cvar, object cvarValue);
        object GetCvar(string cvar);
        bool HasCvar(string cvar);
    }

    public interface IQ3Player
    {
        string FormattedName { get; set; }
        string Name { get; }
        int Ping { get; set; }
        int Score { get; set; }
    }
}
