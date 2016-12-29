using Caliburn.Micro;
using System.ComponentModel.Composition;

namespace JKAPortal.Presentation.Shell
{
    [Export(typeof(ShellViewModel))]
    public class ShellViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private IScreen _prevScreen;
        private IScreen _activeContent;
        public IScreen ActiveContent
        {
            get { return _activeContent; }
            set
            {
                if (value != _activeContent)
                {
                    _activeContent = value;
                    NotifyOfPropertyChange();
                }
            }
        }

        public WorkspaceViewModel Workspace
        {
            get;
            set;
        }

        public SettingsViewModel Settings
        {
            get;
            set;
        }

        public ShellViewModel()
        {
            Workspace = new WorkspaceViewModel();
            Workspace.Activated += OnWorkspaceActivated;
            Workspace.Deactivated += OnWorkspaceDeactivated;

            Settings = new SettingsViewModel();
            Settings.Activated += OnWorkspaceActivated;
            Settings.Deactivated += OnWorkspaceDeactivated;

            ActiveContent = Workspace;
        }

        private void OnWorkspaceActivated(object sender, ActivationEventArgs e)
        {
            _prevScreen = ActiveContent;

            if (sender is IScreen)
                ActiveContent = (IScreen)sender;
        }

        private void OnWorkspaceDeactivated(object sender, DeactivationEventArgs e)
        {
            if (_prevScreen != null)
            {
                ActiveContent = _prevScreen;
                _prevScreen = null;
            }
        }

        public void ActivateSettings()
        {
            if (!ReferenceEquals(ActiveContent, Settings))
                ActivateItem(Settings);
        }
    }
}
