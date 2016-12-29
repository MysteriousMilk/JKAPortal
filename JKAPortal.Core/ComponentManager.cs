using JKAPortal.API;
using JKAPortal.API.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JKAPortal.Core
{
    public class ComponentManager : IComponentManager
    {
        [ImportMany]
        public List<IComponent> Components
        {
            get;
            private set;
        }

        public ComponentManager()
        {
            
        }

        public IComponent GetInstance(Type componentType)
        {
            if (Components == null)
                return null;

            return Components.Find(c => c.GetType().Equals(componentType));
        }

        public DirectoryInfo GetProgramDataDirectory(IComponent component)
        {
            var assemblyName = new AssemblyName(component.GetType().Assembly.FullName);

            string dir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                "\\JKAPortal\\Components\\" + assemblyName.Name + "\\";

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return new DirectoryInfo(dir);
        }

        public DirectoryInfo GetUserDataDirectory(IComponent component)
        {
            var assemblyName = new AssemblyName(component.GetType().Assembly.FullName);

            string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                "\\JKAPortal\\Components\\" + assemblyName.Name + "\\";

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            return new DirectoryInfo(dir);
        }

        public DirectoryInfo GetComponentExectuingDirectory(IComponent component)
        {
            return new DirectoryInfo(component.GetType().Assembly.Location);
        }
    }
}
