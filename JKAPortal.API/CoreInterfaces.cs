using JKAPortal.API.Q3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace JKAPortal.API.Core
{
    public interface IQ3Utilities
    {
        T GetInstance<T>();
        IQ3Server GetServer(IPEndPoint serverIp);
        string GetUnformattedName(string formattedName);
        string ResolveName(string name);
    }

    public interface IJKASettings
    {
        string ExecutablePath { get; set; }
        string GameDataPath { get; set; }
    }

    public interface IApplicationSettings
    {
        IJKASettings JediAcademy { get; set; }

        void Save();
    }

    public interface IApplicationData
    {
        IThumbnail DefaultThumbnail { get; set; }
        List<IThumbnail> MapThumbnails { get; set; }
    }

    public interface IThumbnail
    {
        BitmapImage Image { get; set; }
        string Name { get; set; }
    }

    public interface IComponentManager
    {
        List<IComponent> Components { get; }

        IComponent GetInstance(Type componentType);

        DirectoryInfo GetProgramDataDirectory(IComponent component);
        DirectoryInfo GetUserDataDirectory(IComponent component);
        DirectoryInfo GetComponentExectuingDirectory(IComponent component);
    }

    public interface IThemeManager
    {
        List<ITheme> Themes { get; }

        void ApplyDefaultTheme();
        void ApplyTheme(string themeName);
        void ApplyTheme(ITheme theme);
    }

    public interface ISubSystem
    {
        IComponentManager ComponentManager { get; }
        IApplicationData Data { get; }
        IApplicationSettings Settings { get; }
        bool SettingsReset { get; set; }
        IThemeManager ThemeManager { get; }
        IQ3Utilities Utilities { get; set; }

        void Initialize();
    }
}
