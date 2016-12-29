using Caliburn.Micro;
using JKAPortal.API;
using JKAPortal.API.Core;
using JKAPortal.API.Q3;
using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace JKAPortal.Components.Presentation
{
    [Export(typeof(IView))]
    public class ServerBrowserViewModel : Screen, IView
    {
        private ISubSystem _SubSystem;
        private CancellationTokenSource _cts;
        private IQ3MasterServer _masterServer;
        private bool _waitingToRefresh = false;
        private bool _serverListLoaded = false;

        #region SelectedMasterServer
        private string _selectedMasterServer = string.Empty;

        public string SelectedMasterServer
        {
            get { return _selectedMasterServer; }
            set
            {
                if (value != _selectedMasterServer)
                {
                    if (_cts != null)
                        _cts.Cancel();

                    _selectedMasterServer = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        #endregion

        #region SelectedServer
        private ServerInfoViewModel _selectedServer = null;

        public ServerInfoViewModel SelectedServer
        {
            get { return _selectedServer; }
            set
            {
                if (value != _selectedServer)
                {
                    _selectedServer = value;
                    NotifyOfPropertyChange();

                    if (_selectedServer != null)
                        NotifyOfPropertyChange(() => SelectedServer.Name);
                }
            }
        }
        #endregion

        public string IconKind
        {
            get
            {
                return "NetworkServerConnecting";
            }
        }

        public string LastRefreshed
        {
            get;
            set;
        }

        public bool HasServerInfo
        {
            get
            {
                return SelectedServer != null;
            }
        }

        public BindableCollection<ServerInfoViewModel> Servers
        {
            get;
            set;
        }

        public BindableCollection<string> MasterServers
        {
            get;
            set;
        }

        public ServerBrowserViewModel()
        {
            _SubSystem = IoC.Get<ISubSystem>();

            DisplayName = "Server Browser";

            MasterServers = new BindableCollection<string>();
            Servers = new BindableCollection<ServerInfoViewModel>();
            _cts = new CancellationTokenSource();

            MasterServers.Add("masterjk3.ravensoft.com");
            MasterServers.Add("master.jkhub.org");
            MasterServers.Add("All");

            SelectedMasterServer = "masterjk3.ravensoft.com";

            _masterServer = _SubSystem.Utilities.GetInstance<IQ3MasterServer>();
            _masterServer.Ping("masterjk3.ravensoft.com");
            LoadServerList(_masterServer);            
        }

        public void RefreshAll()
        {
            SelectedServer = null;
            NotifyOfPropertyChange(() => HasServerInfo);

            // cancel current async operation (if it is going on).
            if (!_serverListLoaded)
            {
                _waitingToRefresh = true;
                _cts.Cancel();
            }
            else
            {
                RefreshServers();
            }
        }

        private void RefreshServers()
        {
            _serverListLoaded = false;

            _masterServer = _SubSystem.Utilities.GetInstance<IQ3MasterServer>();
            _masterServer.Ping(SelectedMasterServer);
            LoadServerList(_masterServer);
        }

        private async void LoadServerList(IQ3MasterServer masterServer)
        {
            Servers.Clear();

            LastRefreshed = string.Format("{0:M/d/yyyy h:mm tt}", DateTime.Now);
            NotifyOfPropertyChange(() => LastRefreshed);

            if (_cts.IsCancellationRequested)
                _cts = new CancellationTokenSource();

            try
            {
                await Task.Run(() =>
                {
                    Parallel.ForEach(masterServer.RegisteredServers, ip =>
                    {
                        if (_cts.Token.IsCancellationRequested)
                            _cts.Token.ThrowIfCancellationRequested();

                        IQ3Server server = _SubSystem.Utilities.GetServer(ip);

                        if (server != null)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                ServerInfoViewModel serverInfo = new ServerInfoViewModel(server);
                                Servers.Add(serverInfo);
                                NotifyOfPropertyChange(() => Servers.Count);

                                if (SelectedServer == null)
                                    SelectedServer = serverInfo;

                                NotifyOfPropertyChange(() => HasServerInfo);
                            });
                        }
                    });
                }, _cts.Token);

                _serverListLoaded = true;
            }
            catch (AggregateException)
            {
                if (_waitingToRefresh)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _waitingToRefresh = false;
                        RefreshServers();
                    });
                }
                else
                {
                    _serverListLoaded = true;
                }
            }
        }
    }
}
