using Caliburn.Micro;
using JKAPortal.API.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKAPortal.API
{
    public interface ITheme
    {
        string Name { get; }

        IEnumerable<Uri> Resources { get; }
    }

    public interface IComponent
    {
        IView PrimaryView { get; }

        //IWidget DashboardWidget { get; }
    }

    public interface IView : IScreen
    {
        string IconKind { get; }
    }

    public interface IWidget : INotifyPropertyChangedEx
    {

    }
}
