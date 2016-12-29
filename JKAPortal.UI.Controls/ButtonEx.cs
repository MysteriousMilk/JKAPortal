using MahApps.Metro.IconPacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace JKAPortal.UI.Controls
{
    public class ButtonEx : Button
    {
        private ContentControl _iconControl;
        //public string ButtonText
        //{
        //    get { return (string)GetValue(ButtonTextProperty); }
        //    set { SetValue(ButtonTextProperty, value); }
        //}

        //public static readonly DependencyProperty ButtonTextProperty =
        //    DependencyProperty.Register(
        //        "ButtonText",
        //        typeof(string),
        //        typeof(ButtonEx),
        //        new UIPropertyMetadata("Browse")
        //        );

        //public Geometry IconGeometry
        //{
        //    get { return (Geometry)GetValue(IconGeometryProperty); }
        //    set { SetValue(IconGeometryProperty, value); }
        //}

        //public static readonly DependencyProperty IconGeometryProperty =
        //    DependencyProperty.Register(
        //        "IconGeometry",
        //        typeof(Geometry),
        //        typeof(ButtonEx),
        //        new UIPropertyMetadata(null, IconVisualChanged)
        //        );

        //public SolidColorBrush IconBrush
        //{
        //    get { return (SolidColorBrush)GetValue(IconBrushProperty); }
        //    set { SetValue(IconBrushProperty, value); }
        //}

        //public static readonly DependencyProperty IconBrushProperty =
        //    DependencyProperty.Register(
        //        "IconBrush",
        //        typeof(SolidColorBrush),
        //        typeof(ButtonEx),
        //        new UIPropertyMetadata()
        //        );

        public PackIconModernKind ModernIconKind
        {
            get { return (PackIconModernKind)GetValue(ModernIconKindProperty); }
            set { SetValue(ModernIconKindProperty, value); }
        }

        public static readonly DependencyProperty ModernIconKindProperty =
            DependencyProperty.Register(
                "ModernIconKind",
                typeof(PackIconModernKind),
                typeof(ButtonEx),
                new UIPropertyMetadata()
                );

        public int IconSize
        {
            get { return (int)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register(
                "IconSize",
                typeof(int),
                typeof(ButtonEx),
                new UIPropertyMetadata(18)
                );

        static ButtonEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ButtonEx), new FrameworkPropertyMetadata(typeof(ButtonEx))
                );
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //_iconControl = (ContentControl)GetTemplateChild("PART_Icon");
            //UpdateIconVisual();
        }

        //public void UpdateIconVisual()
        //{
        //    if (_iconControl == null)
        //        return;

        //    //_iconRect.OpacityMask = new VisualBrush() { Visual = IconVisual };
        //    InvalidateVisual();
        //}

        //private static void IconVisualChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ButtonEx btn = (ButtonEx)d;
        //    btn.UpdateIconVisual();
        //}
    }
}
