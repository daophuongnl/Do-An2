using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Model.Data
{
    public static class ConfigUtil
    {
        private static bool isLoadConfig { get; set; } = false;
        private static string? path { get; set; }
        private static Config? config { get; set; }

        public static T Get<T>(string configPath) where T: Config
        {
            isLoadConfig = true;

            T? config = null;
            try
            {
                config = File_Util.ReadTxtFile(configPath)!.JsonDeserialize<T>();
            }
            catch
            {

            }

            if (config == null)
            {
                config = (T)Activator.CreateInstance(typeof(T), new object[] { });
            }

            ConfigUtil.config = config;

            isLoadConfig = false;
            path = configPath;

            return config;
        }

        public static void Save(this Config q)
        {
            if (isLoadConfig) return;

            var configString = JsonConvert.SerializeObject(q, Formatting.Indented);
            File_Util.WriteTxtFile(path!, configString, true);
        }

        public static void Save()
        {
            if (isLoadConfig) return;

            config!.Save();
        }
    }
}
