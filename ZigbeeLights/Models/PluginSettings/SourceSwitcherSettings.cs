using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.PluginSettings
{
    public class SourceSwitcherSettings
    {
        public static SourceSwitcherSettings CreateDefaultSettings()
        {
            SourceSwitcherSettings instance = new SourceSwitcherSettings();
            instance.HomeAssistantUrl = "192.168.1.102";
            instance.HomeAssistantPort = "8123";
            instance.Service = "select_source";
            instance.ServiceData = string.Empty;
            instance.ServiceDomain = "media_player";
            instance.AuthorizationToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJkMTJiYjllMjM3ZTc0MjJjYmNmN2U0YzQ1MjExODI3NyIsImlhdCI6MTU5MTU1MTA0MiwiZXhwIjoxOTA2OTExMDQyfQ.kOoOhKPMjw5uHZT8axIUFiTblaQarJnZdR7pJ39xWKU";
            instance.Source = "BD";
            instance.EntityId = "media_player.pioneer_vsx_325";
            return instance;
        }


        [JsonProperty(PropertyName = "homeAssistantUrl")]
        public string HomeAssistantUrl { get; set; }

        [JsonProperty(PropertyName = "homeAssistantPort")]
        public string HomeAssistantPort { get; set; }

        [JsonProperty(PropertyName = "authorizationToken")]
        public string AuthorizationToken { get; set; }

        [JsonProperty(PropertyName = "serviceDomain")]
        public string ServiceDomain { get; set; }

        [JsonProperty(PropertyName = "service")]
        public string Service { get; set; }

        [JsonProperty(PropertyName = "serviceData")]
        public string ServiceData { get; set; }

        [JsonProperty(PropertyName = "entityId")]
        public string EntityId { get; set; }

        [JsonProperty(PropertyName = "source")]
        public string Source { get; set; }
    }
}
