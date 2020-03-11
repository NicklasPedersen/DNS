using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace DNS
{
    class PingProtocol
    {
        public string LocalPing()
        {
            // Ping's the local machine.
            Ping pingSender = new Ping();
            IPAddress address = IPAddress.Loopback;
            PingReply reply = pingSender.Send(address);

            if (reply.Status == IPStatus.Success)
            {
                StringBuilder b = new StringBuilder();
                b.AppendLine("Address: " + reply.Address.ToString());
                b.AppendLine("RoundTrip time: " + reply.RoundtripTime);
                b.AppendLine("Time to live: " + reply.Options.Ttl);
                b.AppendLine("Don't fragment: " + reply.Options.DontFragment);
                b.AppendLine("Buffer size: " + reply.Buffer.Length);
                return b.ToString();
            }
            else
            {
                return reply.Status.ToString();
            }
        }
    }
}
