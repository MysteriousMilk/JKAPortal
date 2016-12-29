using Caliburn.Micro;
using JKAPortal.API.Core;
using JKAPortal.API.Q3;
using System;
using System.Linq;

namespace JKAPortal.Components.Presentation
{
    public class ServerInfoViewModel : PropertyChangedBase
    {
        private IQ3Server _server;
        private ISubSystem _SubSystem;

        public string Name
        {
            get
            {
                return _server.FormattedName;
            }
        }

        public long Ping
        {
            get
            {
                return _server.Ping;
            }
        }

        public int Players
        {
            get
            {
                return _server.PlayerCount;
            }
        }

        public string Map
        {
            get
            {
                return _server.MapName;
            }
        }

        public JKAGameType GameType
        {
            get
            {
                if (_server.HasCvar("g_gametype"))
                    return (JKAGameType)(Convert.ToInt32(_server.GetCvar("g_gametype")));
                return JKAGameType.Unknown;
            }
        }

        public int MaxClients
        {
            get
            {
                if (_server.HasCvar("sv_maxclients"))
                    return Convert.ToInt32(_server.GetCvar("sv_maxclients"));
                return 0;
            }
        }

        public string Version
        {
            get
            {
                if (_server.HasCvar("version"))
                    return _server.GetCvar("version").ToString();
                return "Unknown Game Version";
            }
        }

        public IThumbnail MapThumbnail
        {
            get;
            set;
        }

        public BindableCollection<IQ3Player> PlayerCollection
        {
            get;
            set;
        }

        public ServerInfoViewModel(IQ3Server server)
        {
            _SubSystem = IoC.Get<ISubSystem>();
            _server = server;

            PlayerCollection = new BindableCollection<IQ3Player>();
            PlayerCollection.AddRange(_server.Players);

            string thumbName = _server.MapName.Split('/').LastOrDefault();
            if (!string.IsNullOrEmpty(thumbName))
                MapThumbnail = _SubSystem.Data.MapThumbnails.Where(t => t.Name.Equals(thumbName)).FirstOrDefault();

            if (MapThumbnail == null)
                MapThumbnail = _SubSystem.Data.DefaultThumbnail;
        }
    }
}
