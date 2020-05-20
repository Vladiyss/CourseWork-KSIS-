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
        public static IPAddress GetHostsBroadcastIPAddress()
        {
            IPHostEntry MyHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IPaddress = MyHost.AddressList.FirstOrDefault(addr => addr.AddressFamily == AddressFamily.InterNetwork);
            byte[] IPaddressBytes = IPaddress.GetAddressBytes();
            IPaddressBytes[IPaddressBytes.Length - 1] = 255;
            IPAddress broadcastIPaddress = new IPAddress(IPaddressBytes);

            return broadcastIPaddress;
        }

        public static IPAddress GetHostsIPAddress()
        {
            IPHostEntry MyHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IPaddress = MyHost.AddressList.FirstOrDefault(addr => addr.AddressFamily == AddressFamily.InterNetwork);

            return IPaddress;
        }

        public static int RetrieveMessageType(string messageType)
        {
            int i = 1;
            bool flag = true;
            while ((i <= 7) && flag)
            {
                int result = String.Compare(Message.MessageType[i], messageType);
                if (0 == result)
                    flag = false;
                else
                    ++i;
            }
            return i;
        }
    }
}
