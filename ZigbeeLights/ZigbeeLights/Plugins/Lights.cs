using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

using Models.HomeAssistant;
using WebSocketSharp;
using Logger = BarRaider.SdTools.Logger;
using WebSocket = WebSocketSharp.WebSocket;
using Models.PluginSettings;

namespace ZigbeeLights
{
    [PluginActionId("com.zigbeelights.lights")]
    public class Lights : PluginBase
    {


        #region Private Members

        private LightsPluginSettings settings;

        private WebSocket websocket = null;        

        private HttpClient client = new HttpClient();



        #endregion
        public Lights(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                this.settings = LightsPluginSettings.CreateDefaultSettings();
            }
            else
            {
                this.settings = payload.Settings.ToObject<LightsPluginSettings>();
            }            

            SetupWebSocket();            
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this.settings.AuthorizationToken);
            GetStates();
        }


        public override void Dispose()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"Destructor called");
        }

        public override void KeyPressed(KeyPayload payload)
        {
            ToggleLight();
            Logger.Instance.LogMessage(TracingLevel.INFO, "Key Pressed");
            
        }

        public override void KeyReleased(KeyPayload payload) { }

        public override void OnTick() { }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        #region Private Methods

        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));
        }

        private void SetupWebSocket()
        {
            var url = $"ws://{this.settings.HomeAssistantUrl}:{this.settings.HomeAssistantPort}/api/websocket";
            websocket = new WebSocket(url);
            websocket.OnMessage += Websocket_OnMessage;
            websocket.OnOpen += Websocket_OnOpen;

            websocket.Connect();

            // send auth stuff here..
            var authData = new
            {
                type = "auth",
                access_token = this.settings.AuthorizationToken
            };
            var authDataString = JsonConvert.SerializeObject(authData);
            websocket.Send(authDataString);


            // Send Subscribe info
            var subscribeData = new
            {
                id = 18,
                type = "subscribe_events",
                event_type = "state_changed"
            };
            var subscribeDataString = JsonConvert.SerializeObject(subscribeData);
            websocket.Send(subscribeDataString);            
        }

        private void Websocket_OnOpen(object sender, EventArgs e)
        {

        }

        private void Websocket_OnMessage(object sender, MessageEventArgs e)
        {            
            Logger.Instance.LogMessage(TracingLevel.INFO, e.Data);

            StateChanged stateChanged = JsonConvert.DeserializeObject<StateChanged>(e.Data) as StateChanged;
            if(stateChanged != null && stateChanged._event.data.entity_id != null)
            {
                if(stateChanged._event.data.entity_id == this.settings.EntityId)
                {

                    // set the image to show on
                    if(stateChanged._event.data.new_state.state == "on")
                    {
                        Connection.SetImageAsync(this.settings.ImageOn, null, true);
                    }

                    if(stateChanged._event.data.new_state.state == "off")
                    {
                        Connection.SetImageAsync(this.settings.ImageOff, null, true);
                    }
                }
            }

        }

        private void GetStates()
        {
            var url = $"http://{this.settings.HomeAssistantUrl}:{this.settings.HomeAssistantPort}/api/states";

            var result = client.GetAsync(url).Result.Content.ReadAsAsync<List<State>>().Result;

            if (result != null)
            {
                var currentState = result.Where(x => x.entity_id == this.settings.EntityId).FirstOrDefault();
                if (currentState != null)
                {
                    if (currentState.state == "on") { Connection.SetImageAsync(this.settings.ImageOn, null, true); }

                    if (currentState.state == "off") { Connection.SetImageAsync(this.settings.ImageOff, null, true); }
                }
            }
        }



        private void ToggleLight()
        {
            var url = $"http://{this.settings.HomeAssistantUrl}:{this.settings.HomeAssistantPort}/api/services/{this.settings.ServiceDomain}/{this.settings.Service}";

            var data = new
            {
                entity_id = this.settings.EntityId,
                //is_volume_muted = false
            };

            var response = client.PostAsJsonAsync(url, data);
        }
        #endregion
    }
}