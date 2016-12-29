using Caliburn.Micro;
using JKAPortal.API;
using JKAPortal.API.Core;
using JKAPortal.Presentation.Component;
using System.ComponentModel.Composition;

namespace JKAPortal.Presentation.Shell
{
    [Export(typeof(WorkspaceViewModel))]
    public class WorkspaceViewModel : Conductor<IView>.Collection.OneActive
    {
        private ISubSystem _SubSystem;

        public WorkspaceViewModel()
        {
            _SubSystem = IoC.Get<ISubSystem>();

            Items.Add(new DashboardViewModel());
            foreach (IComponent component in _SubSystem.ComponentManager.Components)
                Items.Add(component.PrimaryView);

            ActivateItem(Items[0]);
        }
    }
}
