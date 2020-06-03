using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace CommonInformation
{
    public static class CommonInfo
    {
        private const int BroadcastNumber = 255;
        public static IPAddress GetHostsBroadcastIPAddress()
        {
            IPHostEntry MyHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IPaddress = MyHost.AddressList.FirstOrDefault(addr => addr.AddressFamily == AddressFamily.InterNetwork);
            byte[] IPaddressBytes = IPaddress.GetAddressBytes();
            IPaddressBytes[IPaddressBytes.Length - 1] = BroadcastNumber;
            IPAddress broadcastIPaddress = new IPAddress(IPaddressBytes);

            return broadcastIPaddress;
        }

        public static IPAddress GetHostsIPAddress()
        {
            IPHostEntry MyHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IPaddress = MyHost.AddressList.FirstOrDefault(addr => addr.AddressFamily == AddressFamily.InterNetwork);

            return IPaddress;
        }
    }
}
