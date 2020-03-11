using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security;
using System.Text;

namespace DNS
{
    class DHCP
    {
        public string GetHostnameFromIp(string Ip)
        {
            string hostname = "";
            try
            {
                IPHostEntry ipHostEntry = Dns.GetHostByAddress(Ip);
                hostname = ipHostEntry.HostName;
            }
            catch (FormatException)
            {
                hostname = "Please specify a valid IP address.";
                return hostname;
            }
            catch (SocketException)
            {
                hostname = "Unable to perform lookup - a socket error occured.";
                return hostname;
            }
            catch (SecurityException)
            {
                hostname = "Unable to perform lookup - permission denied.";
                return hostname;
            }
            catch (Exception)
            {
                hostname = "An unspecified error occured.";
                return hostname;
            }
            return hostname;
        }
        public string GetDisplayDhcpServerAddressesString()
        {
            StringBuilder b = new StringBuilder();
            b.Append("DHCP Servers");
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPAddressCollection addresses = adapter.GetIPProperties().DhcpServerAddresses;
                if (addresses.Count > 0)
                {
                    b.Append(adapter.Description);
                    foreach (IPAddress address in addresses)
                    {
                        b.AppendLine("  Dhcp Address ............................ : " + address.ToString());
                    }
                    b.AppendLine();
                }
            }
            return b.ToString();
        }
    }
}
