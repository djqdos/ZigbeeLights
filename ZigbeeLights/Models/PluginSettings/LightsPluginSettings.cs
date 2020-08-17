using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.PluginSettings
{
    public class LightsPluginSettings
    {
        public static LightsPluginSettings CreateDefaultSettings()
        {
            LightsPluginSettings instance = new LightsPluginSettings();
            instance.HomeAssistantUrl = string.Empty;
            instance.Service = string.Empty;
            instance.ServiceData = string.Empty;
            instance.ServiceDomain = string.Empty;
            instance.AuthorizationToken = string.Empty;
            instance.ImageOff = string.Empty;
            instance.ImageOn = string.Empty;
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


        [JsonProperty(PropertyName = "imageOff")]
        public string ImageOff { get; set; }

        [JsonProperty(PropertyName = "imageOn")]
        public string ImageOn { get; set; }
    }
}
