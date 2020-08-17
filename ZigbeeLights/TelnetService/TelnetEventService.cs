using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TentacleSoftware.Telnet;

namespace TelnetService
{
    public class TelnetEventService
    {        
        private TelnetClient client;
        private readonly string _ipaddress;
        private readonly int _port;

        public bool IsActive = false;

        // Define a new event handler
        public EventHandler<TelnetMessageEventArgs> TelnetMessageRecieved;


        /// <summary>
        /// Constructor for the TelnetEventService
        /// </summary>
        /// <param name="IPAddress"></param>
        /// <param name="port"></param>
        public TelnetEventService(string IPAddress, int port)
        {
            _ipaddress = IPAddress;
            _port = port;

            client = new TelnetClient(_ipaddress, _port, TimeSpan.FromSeconds(3.0), CancellationToken.None);
            client.MessageReceived += OnMessageRecieved;
            client.ConnectionClosed += OnHandleConnectionClosed;

            //Open telnet connection to Amp
            client.Connect();
            IsActive = true;
            if (TelnetMessageRecieved != null)
            {
                TelnetMessageRecieved(this, new TelnetMessageEventArgs() { Message = "CONNECTED" });
            }

            Debug.WriteLine("connected");
        }

        /// <summary>
        /// Send command to AV Reciever
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task SendCommand(string command)
        {
            await client.Send(command);
        }



        /// <summary>
        /// Outgoing message to Streamdeck client
        /// </summary>
        /// <param name="msg"></param>
        protected virtual void OnTelnetMessageRecieved(string msg)
        {
            if (TelnetMessageRecieved != null)
            {
                TelnetMessageRecieved(this, new TelnetMessageEventArgs() { Message = msg });
            }
        }

        /// <summary>
        /// Incoming message from AMP Telnet
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnMessageRecieved(object sender, string e)
        {
            Debug.WriteLine($"From AMP: {e}");

            // raise event to outgoing
            OnTelnetMessageRecieved(e);
        }

        /// <summary>
        /// AMP closed connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnHandleConnectionClosed(object sender, EventArgs e)
        {
            Debug.WriteLine($"Connection Closed");
            IsActive = false;
            TelnetMessageRecieved(this, new TelnetMessageEventArgs() { Message = "CLOSED" });
        }
    }
}
