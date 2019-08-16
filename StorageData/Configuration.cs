using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace StorageData
{
    public class Configuration
    {
        public string PathForStoreImage { get; set; }
        public string DatabaseConnectionString { get; set; }
        public Configuration GetConfiguration()
        {
            string configurationString;
            using (var jsonString = new StreamReader($"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}ConfigurationFiles{Path.DirectorySeparatorChar}ConfigurationFile.json"))
            {
                configurationString = jsonString.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<Configuration>(configurationString);
        }
    }
}
