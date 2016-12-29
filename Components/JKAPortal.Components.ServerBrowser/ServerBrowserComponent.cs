using JKAPortal.API;
using JKAPortal.Components.Presentation;
using System.ComponentModel.Composition;

namespace JKAPortal.Components
{
    [Export(typeof(IComponent))]
    public class ServerBrowserComponent : IComponent
    {
        private ServerBrowserViewModel _PrimaryView = null;

        public IView PrimaryView
        {
            get
            {
                if (_PrimaryView == null)
                    _PrimaryView = new ServerBrowserViewModel();

                return _PrimaryView;
            }
        }

        public ServerBrowserComponent()
        {

        }
    }
}
