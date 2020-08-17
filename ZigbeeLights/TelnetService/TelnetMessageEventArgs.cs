using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelnetService
{
    /// <summary>
    /// The value that is recieved via telnet from AV Receiver
    /// </summary>
    public class TelnetMessageEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
}
