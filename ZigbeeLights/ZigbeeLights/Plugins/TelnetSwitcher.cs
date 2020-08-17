using BarRaider.SdTools;
using Models.PluginSettings;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TelnetService;

namespace ZigbeeLights
{
    [PluginActionId("com.richb.telnetswitcher")]
    public class TelnetSwitcher : PluginBase
    {

        private TelnetSwitcherSettings settings;
        

        public TelnetSwitcher(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                this.settings = TelnetSwitcherSettings.CreateDefaultSettings();
            }
            else
            {
                this.settings = payload.Settings.ToObject<TelnetSwitcherSettings>();
            }            
            
            var cancellationTokenSource = new CancellationTokenSource();
        }

        public override void Dispose()
        {
            
        }

        public override void KeyPressed(KeyPayload payload)
        {
            if (TelnetStaticClient.telnetClient == null || !TelnetStaticClient.telnetClient.IsActive)
            {
                TelnetStaticClient.telnetClient = null;
                TelnetStaticClient.telnetClient = new TelnetEventService(this.settings.AVAmpIP, int.Parse(this.settings.AVAmpPort));
            }

            TelnetStaticClient.telnetClient.SendCommand(settings.AVAmpCommand);
        }

        public override void KeyReleased(KeyPayload payload)
        {            
        }

        public override void OnTick()
        {
            
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload)
        {
            
        }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            SaveSettings();
        }


        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));
        }
    }
}
