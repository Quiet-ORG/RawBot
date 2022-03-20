using Newtonsoft.Json;
using System.IO;

namespace RawBot.Configuration
{
    public class ConfigurationLoader
    {
        public const string ConfigPath = "bot.json";

        public static Config Load()
        {
            return JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath));
        }
    }
}
