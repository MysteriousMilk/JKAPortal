using JKAPortal.API;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;

namespace JKAPortal.Theme.Cobalt
{
    [Export(typeof(ITheme))]
    public class CobaltTheme : ITheme
    {
        public string Name
        {
            get
            {
                return "Cobalt Theme";
            }
        }

        public IEnumerable<Uri> Resources
        {
            get
            {
                string asmName = Assembly.GetExecutingAssembly().ManifestModule.Name.Replace(".dll", "");

                yield return new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Controls.xaml");
                yield return new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Fonts.xaml");
                yield return new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Colors.xaml");
                yield return new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/Cobalt.xaml");
                yield return new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/BaseDark.xaml");

                yield return new Uri("pack://application:,,,/" + asmName + ";component/Themes/Cobalt.xaml");
            }
        }
    }
}
