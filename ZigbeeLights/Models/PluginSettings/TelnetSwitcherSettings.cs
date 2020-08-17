using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.PluginSettings
{
    public class TelnetSwitcherSettings
    {
        public static TelnetSwitcherSettings CreateDefaultSettings()
        {
            TelnetSwitcherSettings instance = new TelnetSwitcherSettings();
            instance.AVAmpIP = string.Empty;
            instance.AVAmpPort = string.Empty;
            instance.AVAmpCommand = string.Empty;
            return instance;
        }

        [JsonProperty(PropertyName = "avampIP")]
        public string AVAmpIP { get; set; }

        [JsonProperty(PropertyName = "avampPort")]
        public string AVAmpPort { get; set; }

        [JsonProperty(PropertyName = "avampCommand")]
        public string AVAmpCommand { get; set; }
    }
}
