using System.IO;
using Newtonsoft.Json;
namespace StorageData
{
    public class Configuration
    {
        private static Configuration _instance;

        public string PathForStoreImage { get; set; }
        
        public string DatabaseConnectionString { get; set; }

        public static Configuration GetConfiguration()
        {
            if (_instance != null)
                return _instance;

            var configFilePath = Path.Combine(new[]
            {
                Directory.GetCurrentDirectory(),
                "ConfigurationFiles",
                "ConfigurationFile.json"
            });

            string configurationString;
            using (var jsonString = new StreamReader(configFilePath))
            {
                configurationString = jsonString.ReadToEnd();
            }

            _instance = JsonConvert.DeserializeObject<Configuration>(configurationString);

            return _instance;
        }
    }
}
