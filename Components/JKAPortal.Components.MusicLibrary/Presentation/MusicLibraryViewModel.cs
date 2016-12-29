using Caliburn.Micro;
using JKAPortal.API;
using JKAPortal.API.Core;
using JKAPortal.Components.Data;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.Win32;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;

namespace JKAPortal.Components.Presentation
{
    [Export(typeof(IView))]
    public class MusicLibraryViewModel : Screen, IView
    {
        private ISubSystem _SubSystem;
        private IComponent _Component;

        public string IconKind
        {
            get { return "Music"; }
        }

        public MusicLibraryViewModel()
        {
            DisplayName = "Music Library";

            _SubSystem = IoC.Get<ISubSystem>();
            _Component = _SubSystem.ComponentManager.GetInstance(typeof(MusicLibraryComponent));

            string dbPath = _SubSystem.ComponentManager.GetProgramDataDirectory(_Component).FullName;
            string dbFile = "MusicLibraryDB.db3";

            if (!File.Exists(dbPath + dbFile))
            {
                if (Directory.Exists(dbPath))
                    Directory.CreateDirectory(dbPath);

                using (var conn = new SQLiteConnection(new SQLitePlatformWin32(), dbPath + dbFile))
                {
                    conn.CreateTable<Artist>(CreateFlags.AutoIncPK);
                    conn.CreateTable<Album>(CreateFlags.AutoIncPK);
                    conn.CreateTable<Song>(CreateFlags.AutoIncPK);
                }
            }

            using (var conn = new SQLiteConnection(new SQLitePlatformWin32(), dbPath + dbFile))
            {
                var result = conn.Table<Artist>().Where(a => a.Name.Equals("Trivium"));
            }
        }
    }
}
