using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKAPortal.Model.Q3
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
}
