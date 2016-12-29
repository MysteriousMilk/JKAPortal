using JKAPortal.API;
using JKAPortal.API.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JKAPortal.Core
{
    public class ThemeManager : IThemeManager
    {
        private List<ResourceDictionary> _AppStartupDictionaries;

        [ImportMany]
        public List<ITheme> Themes
        {
            get;
            private set;
        }

        public ThemeManager()
        {
            _AppStartupDictionaries = new List<ResourceDictionary>();
            
            if (Application.Current.Resources.MergedDictionaries.Count > 0)
            {
                foreach (ResourceDictionary dict in Application.Current.Resources.MergedDictionaries)
                    _AppStartupDictionaries.Add(dict);
            }
        }

        public void ApplyDefaultTheme()
        {
            if (Themes.Count > 0)
                ApplyTheme(Themes[0]);
        }

        public void ApplyTheme(string themeName)
        {
            ITheme theme = Themes.Find(t => t.Name.Equals(themeName));

            if (theme == null)
            {
                Trace.WriteLine("No theme found with the name " + themeName + ".");
                return;
            }

            ApplyTheme(theme);
        }

        public void ApplyTheme(ITheme theme)
        {
            Application.Current.Resources.MergedDictionaries.Clear();

            foreach (ResourceDictionary dict in _AppStartupDictionaries)
                Application.Current.Resources.MergedDictionaries.Add(dict);

            foreach (Uri uri in theme.Resources)
            {
                ResourceDictionary dict = new ResourceDictionary() { Source = uri };
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
        }
    }
}
