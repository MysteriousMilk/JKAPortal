using Caliburn.Micro;
using JKAPortal.API;
using JKAPortal.Components.Presentation;
using System.ComponentModel.Composition;

namespace JKAPortal.Components
{
    [Export(typeof(IComponent))]
    public class MusicLibraryComponent : IComponent
    {
        private MusicLibraryViewModel _PrimaryView = null;

        public IView PrimaryView
        {
            get
            {
                if (_PrimaryView == null)
                    _PrimaryView = new MusicLibraryViewModel();

                return _PrimaryView;
            }
        }

        public MusicLibraryComponent()
        {

        }
    }
}
