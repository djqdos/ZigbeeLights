using BarRaider.SdTools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Models.HomeAssistant;
using WebSocketSharp;
using Logger = BarRaider.SdTools.Logger;
using WebSocket = WebSocketSharp.WebSocket;
using Models.PluginSettings;

namespace ZigbeeLights
{
    [PluginActionId("com.zigbeelights.sourceswitcher")]
    public class SwitchSource : PluginBase
    {
        #region Private Members

        private SourceSwitcherSettings settings;

        private WebSocket websocket = null;

        private const string imageOff = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAAOsAAADrASts18IAAAKOSURBVFhHvZbNS5RRFIdnxo/UTEJQpE0f0ge0sGgh0iJBVy78B4QKWrgKaSG0adcmgqBtGeLC/0CoVS3aSAtbCCKiFLbwg8ysCExifH7XM9i87zhz753GHzyc95x77n3P3Pd+TCZW+Xx+FFbgLdy18PGIF/bDF7hpfIIea66teNF5WIQde7HQ8zJctjRvZc16i5e8wJyEpy5wqHH4lc1mRw/cGokCHsEWvEmg2JileStmBp5jLsGUCxzqDiwxA0FF5MyGaBNUQH8CxdQWpJgCWmELPidQTG1BCiqA6a/HDMKyCxRLsUHL8VboDDwD/cofcC6BYmpTTm3Er/sIQ+ampDblmOsl7xlg4DpMB+y4QGl9hy5ygz5Dkeicg9T2JHYPNqDJQinR1gzrUN3dwACugH/sddBxq+1WVsqBb3DF/Kpmow3aYRUeWriiyH0AC9ACpy0cJwaYhBkIOjXJn4aX5saJAepB03nBQt6iTzd8NTdODHBLBZgbLPpuwkVzS+rIbUhHtfXCnAvEaRZ6GasOSn7CcueA7nyd70vOi9MadEKzHBWhYvRc0JEFcK3+xKzDNReI0w2YZyz9Uckbfw+aPES1V2EPTlnIW/Rpgl1ot1CcGOA1BJ9s9LkPH8wNF521cFqhB7ah25oqityz1kd9NRPed44THRpAC8adYtgJmCv45UTOGZiFx+a7o7hgK4rE1GWE3wivQBfNiIVTom0YdGE9gQYLO+HroiraAVLo8TqAmQBdybr3tUV1UPWBLqo/cJuVrv3vJe8CeLm+Yxvsgg4obU+hf0Hv4R3MwwnIUcRv7P8VRSQ/jRZqalpDFLQ6dZDYY0FaWKlZTBZatRiw0R69RL4WclUzc0zKZPYBRGvp0KEYt1oAAAAASUVORK5CYII=";
        private const string imageOn = "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAAOsAAADrASts18IAAAL1SURBVFhHrZc7aBRBGMe/u+R8nRcTMfGB4COJ2ogRCQppIloZsLFUMI0EbBTBIoWFtkLAwsYm2tgpWKiNaMRGEb2IWCQkl8MH0Qu+H0mMyfr/z+ze5c7Z3ZnFP/zm5vt2Z/fbmflm5sTzvKT0gXFwH/T6PmeMTgu6wRvQ5TMBdgHTvZGkWDhqC7gL1oNPdECrwRToASN02CpJAFdAFlxUVkVnwQ/QpyxLJQngHDgNniqrok5wAVxSlqWSBMAXbAPXlFXRcTAKTinLUmn/10UlwAC6a6CP15yUJICV4CMo1kAfrznJNYB6cBCMKata9PEa77GWawADgF/5DWyugT5e4z3Wcp2EeXnb0yE/7/hmjbKHRDbeHkZtt3bEy6UH6kCzLHzVlknzX1iuA9bDYAqAvpSuVqkXZGTmmbZMms2zZNtjrNjIFMCC/0sFgbBLB+T1/hbxZrTHJG9acM9a1DgPdihfTG+EDUEwMXKA6/wtmepvkOkh5YwU7ymdaULtJlgBIlPTZhIOgmYZSWOjcZiw273rKH+BE8oOUVwA7L6SFFqbZK6gPbbKtIpsHePitEY7zIrLgi5Vur6cmhtnyfnUzkqYogLgtb3gubKS6THgM5jCpsyKDIB7PruQO1xSTYIWsFxZOggGU1ZUAN/Be9ChrGTaA14CHlQ42cg8KCtuDnCX65QUs9FRqWUsd4KIlSs+gFfgnuSOaMtFq1T2vQDBudGoqDTkWHHskE8yJIW2Rn9mx6t+E1oVuTHwoMJD6m+weIUtK6wHMoANuA7wK24gp/OSbkQ1RnUb+PInqF0GbPsHBM/6R6YA6GMjdo3a3qCTIC/tnz9I7qj2mJQ9LNL2jseyh+C88ulnUfyoqgxQ4hA4cABMgGEw6JX6PW+Sf5C8q6AIRsE+YGprxOVAwp5pALOAiwvTk3CzeQQeAKbcUsB7uQ/EyvVExIVkcYOgS6ty20VhkzBMtdFyYpmWWOOya5JtDywBTCVb8cMYRGzPuA7Bf5bIXzYL1HdiiIaFAAAAAElFTkSuQmCC";


        private HttpClient client = new HttpClient();



        #endregion
        public SwitchSource(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                this.settings = SourceSwitcherSettings.CreateDefaultSettings();
            }
            else
            {
                this.settings = payload.Settings.ToObject<SourceSwitcherSettings>();
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
            SelectSource();
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
            //Logger.Instance.LogMessage(TracingLevel.INFO, e.Data);

            //StateChanged stateChanged = JsonConvert.DeserializeObject<StateChanged>(e.Data) as StateChanged;
            //if(stateChanged != null && stateChanged._event.data.entity_id != null)
            //{
            //    if(stateChanged._event.data.entity_id == this.settings.EntityId)
            //    {

            //        // set the image to show on
            //        if(stateChanged._event.data.new_state.state == "on")
            //        {
            //            Connection.SetImageAsync(imageOn, true);
            //        }

            //        if(stateChanged._event.data.new_state.state == "off")
            //        {
            //            Connection.SetImageAsync(imageOff, true);
            //        }
            //    }
            //}

        }

        private void GetStates()
        {
            //var url = $"http://{this.settings.HomeAssistantUrl}:{this.settings.HomeAssistantPort}/api/states";

            //var result = client.GetAsync(url).Result.Content.ReadAsAsync<List<State>>().Result;

            //if (result != null)
            //{
            //    var currentState = result.Where(x => x.entity_id == this.settings.EntityId).FirstOrDefault();
            //    if (currentState != null)
            //    {
            //        if (currentState.state == "on") { Connection.SetImageAsync(imageOn, true); }

            //        if (currentState.state == "off") { Connection.SetImageAsync(imageOff, true); }
            //    }
            //}
        }



        private void SelectSource()
        {
            var url = $"http://{this.settings.HomeAssistantUrl}:{this.settings.HomeAssistantPort}/api/services/{this.settings.ServiceDomain}/{this.settings.Service}";

            var data = new
            {
                entity_id = this.settings.EntityId,
                source = this.settings.Source
            };

            var response = client.PostAsJsonAsync(url, data).Result;
        }
        #endregion
    }
}