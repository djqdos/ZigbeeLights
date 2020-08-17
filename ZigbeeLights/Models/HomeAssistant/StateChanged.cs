using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.HomeAssistant
{
    public class StateChanged
    {
        [JsonProperty(PropertyName = "event")]
        public Event _event
        {
            get;
            set;
        }

        public int id
        {
            get;
            set;
        }

        public string type
        {
            get;
            set;
        }

        public StateChanged()
        {
        }
    }
    public class Data
    {
        public string entity_id
        {
            get;
            set;
        }

        public New_State new_state
        {
            get;
            set;
        }

        public Old_State old_state
        {
            get;
            set;
        }

        public Data()
        {
        }
    }
    public class Event
    {
        public Context context
        {
            get;
            set;
        }

        public Data data
        {
            get;
            set;
        }

        public string event_type
        {
            get;
            set;
        }

        public string origin
        {
            get;
            set;
        }

        public DateTime time_fired
        {
            get;
            set;
        }

        public Event()
        {
        }
    }
    public class Old_State
    {
        public Attributes attributes
        {
            get;
            set;
        }

        public Context context
        {
            get;
            set;
        }

        public string entity_id
        {
            get;
            set;
        }

        public DateTime last_changed
        {
            get;
            set;
        }

        public DateTime last_updated
        {
            get;
            set;
        }

        public string state
        {
            get;
            set;
        }

        public Old_State()
        {
        }
    }
    public class New_State
    {
        public Attributes attributes
        {
            get;
            set;
        }

        public Context context
        {
            get;
            set;
        }

        public string entity_id
        {
            get;
            set;
        }

        public DateTime last_changed
        {
            get;
            set;
        }

        public DateTime last_updated
        {
            get;
            set;
        }

        public string state
        {
            get;
            set;
        }

        public New_State()
        {
        }
    }
    public class Context
    {
        public string id
        {
            get;
            set;
        }

        public object parent_id
        {
            get;
            set;
        }

        public object user_id
        {
            get;
            set;
        }

        public Context()
        {
        }
    }
    public class Attributes
    {
        public string[] entity_id
        {
            get;
            set;
        }

        public string friendly_name
        {
            get;
            set;
        }

        public int order
        {
            get;
            set;
        }

        public Attributes()
        {
        }
    }
}
