using Caliburn.Micro;
using JKAPortal.API.Core;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace JKAPortal.Presentation.Shell
{
    [Export(typeof(SettingsViewModel))]
    public class SettingsViewModel : Screen
    {
        private ISubSystem _SubSystem;

        #region ExecutablePath
        private string _exePath = string.Empty;
        public string ExecutablePath
        {
            get { return _exePath; }
            set
            {
                if (value != _exePath)
                {
                    _exePath = value;
                    NotifyOfPropertyChange();

                    AutoSetGameDataDirectory();
                }
            }
        }
        #endregion

        #region GameDataDirectory
        private string _gameDataDir = string.Empty;
        public string GameDataDirectory
        {
            get { return _gameDataDir; }
            set
            {
                if (value != _gameDataDir)
                {
                    _gameDataDir = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        #endregion

        public SettingsViewModel()
        {
            _SubSystem = IoC.Get<ISubSystem>();
            RefreshSettings();
        }

        public void RefreshSettings()
        {
            ExecutablePath = _SubSystem.Settings.JediAcademy.ExecutablePath;
            GameDataDirectory = _SubSystem.Settings.JediAcademy.GameDataPath;
        }

        public void Return()
        {
            _SubSystem.Settings.JediAcademy.ExecutablePath = ExecutablePath;
            _SubSystem.Settings.JediAcademy.GameDataPath = GameDataDirectory;
            _SubSystem.Settings.Save();

            TryClose();
        }

        private void AutoSetGameDataDirectory()
        {
            if (!string.IsNullOrEmpty(GameDataDirectory))
                return;

            FileInfo exeFile = new FileInfo(ExecutablePath);
            DirectoryInfo gameDataDir = exeFile.Directory.Parent.EnumerateDirectories().Where(
                dir => dir.Name.Equals("GameData")).FirstOrDefault();

            if (gameDataDir != null)
                GameDataDirectory = gameDataDir.FullName;
        }
    }
}
