using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.HomeAssistant
{

    public class State
    {
        public KeyValuePair<string, string> attributes { get; set; }
        public StateContext context { get; set; }
        public string entity_id { get; set; }
        public DateTime last_changed { get; set; }
        public DateTime last_updated { get; set; }
        public string state { get; set; }
    }

    public class StateAttributes
    {
        
        public string friendly_name { get; set; }
        public int gps_accuracy { get; set; }
        public string id { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string source { get; set; }
        public string user_id { get; set; }
    }

    public class StateContext
    {
        public string id { get; set; }
        public object parent_id { get; set; }
        public object user_id { get; set; }
    }

}
