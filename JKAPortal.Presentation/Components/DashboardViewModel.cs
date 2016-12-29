using Caliburn.Micro;
using JKAPortal.API;
using System.ComponentModel.Composition;

namespace JKAPortal.Presentation.Component
{
    [Export(typeof(IView))]
    public class DashboardViewModel : Screen, IView
    {
        public string IconKind
        {
            get
            {
                return "TilesNine";
            }
        }

        public DashboardViewModel()
        {
            DisplayName = "Portal Dashboard";
        }
    }
}
