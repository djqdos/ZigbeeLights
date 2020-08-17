using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelnetService;

namespace ZigbeeLights
{
    public static class TelnetStaticClient
    {
        public static TelnetEventService telnetClient { get; set; }
    }
}
