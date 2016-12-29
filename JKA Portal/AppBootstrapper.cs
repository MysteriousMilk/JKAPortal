using Caliburn.Micro;
using JKAPortal.Presentation.Shell;
using JKAPortal.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Windows;
using System.Reflection;
using System.Dynamic;
using JKAPortal.API.Core;
using System.IO;
using JKAPortal.API;

namespace JKAPortal
{
    public class AppBootstrapper : BootstrapperBase
    {
        private CompositionContainer container;
        private SubSystem _SubSystem;

        public object CompositionHost { get; private set; }

        public AppBootstrapper()
        {
            _SubSystem = new SubSystem();
            _SubSystem.Initialize();

            Initialize();
        }

        protected override void Configure()
        {
            container = new CompositionContainer(new AggregateCatalog(
                AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()
                ));

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue<ISubSystem>(_SubSystem);

            batch.AddExportedValue(container);

            container.Compose(batch);

            TypeMappingConfiguration config = new TypeMappingConfiguration();
            config.DefaultSubNamespaceForViewModels = "Presentation";
            config.DefaultSubNamespaceForViews = "UI";
            config.IncludeViewSuffixInViewModelNames = false;

            ViewLocator.ConfigureTypeMappings(config);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            dynamic settings = new ExpandoObject();
            settings.Height = 650;
            settings.Width = 900;
            settings.SizeToContent = SizeToContent.Manual;
            settings.Title = "JKA Portal";

            DisplayRootViewFor<ShellViewModel>(settings);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            yield return Assembly.GetExecutingAssembly();
            yield return Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "JKAPortal.UI.dll");
            yield return Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "JKAPortal.Presentation.dll");

            foreach (IComponent c in _SubSystem.ComponentManager.Components)
            {
                yield return c.GetType().Assembly;
            }
        }
    }
}
