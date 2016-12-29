using Caliburn.Micro;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace JKAPortal.Utilities.MVVM.Actions
{
    public class CloseTabItemAction : TriggerAction<DependencyObject>
    {
        protected override void Invoke(object parameter)
        {
            //TabControl.Items.Remove(TabItem);
            if (TabItem.DataContext is IScreen)
                (TabItem.DataContext as IScreen).TryClose();
        }

        public static readonly DependencyProperty TabControlProperty =
            DependencyProperty.Register(
                "TabControl",
                typeof(TabControl),
                typeof(CloseTabItemAction),
                new PropertyMetadata(default(TabControl))
                );

        public TabControl TabControl
        {
            get { return (TabControl)GetValue(TabControlProperty); }
            set { SetValue(TabControlProperty, value); }
        }

        public static readonly DependencyProperty TabItemProperty =
            DependencyProperty.Register(
                "TabItem",
                typeof(TabItem),
                typeof(CloseTabItemAction),
                new PropertyMetadata(default(TabItem))
                );

        public TabItem TabItem
        {
            get { return (TabItem)GetValue(TabItemProperty); }
            set { SetValue(TabItemProperty, value); }
        }
    }
}