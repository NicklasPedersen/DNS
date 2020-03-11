using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security;
using System.Text;

namespace DNS
{
    /// <summary>
    /// Wrapper class for dns lookup and traceroute
    /// </summary>
    class DNS
    {
        /// <summary>
        /// Returns an ip from hostname if successful. If not it returns an error string.
        /// </summary>
        /// <returns>The first address associated with the hostname</returns>
        public string GetIpFromHostname(string hostname)
        {
            IPHostEntry ipHostEntry;
            try
            {
                // Tries to resolve the hostname
                ipHostEntry = Dns.Resolve(hostname);
            }
            catch (SocketException)
            {
                return "Unable to perform lookup - a socket error occured.";
            }
            catch (SecurityException)
            {
                return "Unable to perform lookup - permission denied.";
            }
            catch (Exception)
            {
                return "An unspecified error occured.";
            }

            if (ipHostEntry.AddressList.Length > 0)
            {
                // If there is any addresses associated with the hostname we return the first
                return ipHostEntry.AddressList[0].ToString();
            }
            return "No information found.";
        }
        /// <summary>
        /// Iterates through each node and pings the next.
        /// </summary>
        /// <param name="ipAddressOrHostName"></param>
        /// <returns>The results from tracing an address or hostname</returns>
        public string Traceroute(string ipAddressOrHostName)
        {
            IPAddress ipAddress = Dns.GetHostEntry(ipAddressOrHostName).AddressList[0];
            StringBuilder traceResults = new StringBuilder();

            using (Ping pingSender = new Ping())
            {
                PingOptions pingOptions = new PingOptions();
                Stopwatch stopWatch = new Stopwatch();
                byte[] bytes = new byte[32];

                pingOptions.DontFragment = true;
                pingOptions.Ttl = 1;
                int maxHops = 30;

                traceResults.AppendLine(
                    string.Format(
                        "Tracing route to {0} over a maximum of {1} hops:",
                        ipAddress,
                        maxHops));

                traceResults.AppendLine();

                for (int hop = 1; hop < maxHops + 1; hop++)
                {
                    stopWatch.Reset();
                    stopWatch.Start();

                    PingReply pingReply = pingSender.Send(
                        ipAddress,
                        5000,
                        new byte[32], pingOptions);

                    stopWatch.Stop();

                    traceResults.AppendLine(
                        string.Format("{0}\t{1} ms\t{2}",
                        hop,
                        stopWatch.ElapsedMilliseconds,
                        pingReply.Address));

                    if (pingReply.Status == IPStatus.Success)
                    {
                        traceResults.AppendLine();
                        traceResults.AppendLine("Trace complete."); break;
                    }

                    pingOptions.Ttl++;
                }
            }
            return traceResults.ToString();
        }
    }
}
