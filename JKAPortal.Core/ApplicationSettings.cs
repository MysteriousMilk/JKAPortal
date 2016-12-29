using JKAPortal.API.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JKAPortal.Core
{
    [DataContract]
    [KnownType(typeof(JKASettings))]
    public class ApplicationSettings : IApplicationSettings
    {
        private static string _DataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\JKAPortal\";

        [DataMember]
        public IJKASettings JediAcademy
        {
            get;
            set;
        }

        public ApplicationSettings()
        {
            JediAcademy = new JKASettings();
        }

        public static ApplicationSettings Load()
        {
            string jsonString = File.ReadAllText(_DataPath + "Settings.json");
            return JsonConvert.DeserializeObject<ApplicationSettings>(jsonString);
        }

        public static bool Exists()
        {
            if (!Directory.Exists(_DataPath))
                return false;

            if (!File.Exists(_DataPath + "Settings.json"))
                return false;

            return true;
        }

        public void Save()
        {
            if (!Directory.Exists(_DataPath))
                Directory.CreateDirectory(_DataPath);

            using (var stream = new FileStream(_DataPath + "Settings.json", FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine(JsonConvert.SerializeObject(this, Formatting.Indented));
                    writer.Flush();
                }
            }
        }
    }

    [DataContract]
    public class JKASettings : IJKASettings
    {
        [DataMember]
        public string ExecutablePath
        {
            get;
            set;
        }

        [DataMember]
        public string GameDataPath
        {
            get;
            set;
        }

        public JKASettings()
        {
            ExecutablePath = string.Empty;
            GameDataPath = string.Empty;
        }
    }
}
