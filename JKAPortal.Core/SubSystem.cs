using JKAPortal.API;
using JKAPortal.API.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Reflection;

namespace JKAPortal.Core
{
    public class SubSystem : ISubSystem
    {
        //#region Singleton Instance
        //private static readonly Lazy<SubSystem> lazy = new Lazy<SubSystem>(
        //    () => new SubSystem()
        //    );

        //public static ISubSystem Instance
        //{
        //    get
        //    {
        //        return lazy.Value;
        //    }
        //}
        //#endregion
        public List<DirectoryCatalog> ComponentDirectories
        {
            get;
            internal set;
        }

        /// <summary>
        /// Data used by the application that needs to be loaded
        /// at runtime.
        /// </summary>
        public IApplicationData Data
        {
            get;
            internal set;
        }
        
        /// <summary>
        /// Application specific settings.
        /// </summary>
        public IApplicationSettings Settings
        {
            get;
            internal set;
        }

        public IComponentManager ComponentManager
        {
            get;
            internal set;
        }

        public IThemeManager ThemeManager
        {
            get;
            internal set;
        }

        public IQ3Utilities Utilities
        {
            get;
            set;
        }

        /// <summary>
        /// Flag that indicates is the <see cref="ApplicationSettings"/> have been reset.
        /// </summary>
        public bool SettingsReset
        {
            get;
            set;
        }

        public SubSystem()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += OnAssemblyResolve;
        }

        public void Initialize()
        {
            ComponentManager = new ComponentManager();
            ThemeManager = new ThemeManager();
            Utilities = new Q3Utilities();

            // locate component directories
            ComponentDirectories = new List<DirectoryCatalog>();
            foreach (var path in Directory.EnumerateDirectories(@".\Components", "*", SearchOption.TopDirectoryOnly))
            {
                ComponentDirectories.Add(new DirectoryCatalog(path));
            }

            LoadDependencies();

            ThemeManager.ApplyDefaultTheme();

            // check to see if there are settings to be loaded
            if (!ApplicationSettings.Exists())
            {
                SettingsReset = true;

                // since no settings were found, create defaults
                // and save them
                Settings = new ApplicationSettings();
                Settings.Save();
            }
            else
                Settings = ApplicationSettings.Load();

            // load the data
            Data = new ApplicationData();
        }

        private void LoadDependencies()
        {
            var catalog = new AggregateCatalog();

            DirectoryCatalog themeCatalog = new DirectoryCatalog("Themes");
            catalog.Catalogs.Add(themeCatalog);

            foreach (var directory in ComponentDirectories)
                catalog.Catalogs.Add(directory);

            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(ComponentManager);
            container.ComposeParts(ThemeManager);
        }

        private Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;

            string folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileName = new AssemblyName(args.Name).Name + ".dll";
            string assemblyPath = Path.Combine(folderPath, fileName);

            try
            {
                if (File.Exists(assemblyPath))
                {
                    assembly = Assembly.LoadFrom(assemblyPath);
                }
                else
                {
                    foreach (var directory in ComponentDirectories)
                    {
                        assemblyPath = Path.Combine(directory.FullPath, fileName);

                        if (File.Exists(assemblyPath))
                        {
                            assembly = Assembly.LoadFrom(assemblyPath);
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                assembly = null;
            }

            return assembly;
        }
    }
}
