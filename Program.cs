using System;
using System.Net;

namespace DNS
{
    class Program
    {
        static void Main()
        {
            
            string[] test = {
                "Get ips from hostname",
                "Get hostname from ip",
                "localping",
                "traceroute",
                "Get dhcp server",
                "Ipconfig",
                "Quit" 
            };
            int answer;
            do
            {
                Console.WriteLine("Press a button to continue");
                Console.ReadKey(true);
                Console.Clear();
                answer = GetOptionFromArray(test);
                DNS dns = new DNS();
                DHCP dhcp = new DHCP();
                PingProtocol ping = new PingProtocol();
                switch (answer)
                {
                    case 0: // Ips from hostname
                    {
                        Console.WriteLine("What hostname?");
                        string hostname = Console.ReadLine();

                        IPAddress[] array = Dns.GetHostAddresses(hostname);

                        foreach (IPAddress address in array)
                        {
                            Console.WriteLine(address);
                        }
                        Console.ReadKey();
                        break;
                    }
                    case 1: // Get hostname from ip
                        Console.WriteLine("What ip to get hostname from? ");
                        string ip = Console.ReadLine();
                        Console.WriteLine("start");
                        string name = dhcp.GetHostnameFromIp(ip);
                        Console.WriteLine(name);
                        Console.WriteLine("slut");
                        break;
                    case 2: // Localping
                        Console.WriteLine(ping.LocalPing());
                        break;
                    case 3: // Traceroute
                        Console.Write("What ip or hostname to traceroute? ");
                        string ipOrName = Console.ReadLine();
                        string route = dns.Traceroute(ipOrName);
                        Console.WriteLine("route*** " + route);
                        break;
                    case 4: // Get dhcp server
                        Console.WriteLine(dhcp.GetDisplayDhcpServerAddressesString());
                        break;
                    case 5: // Ipconfig
                        IPHostEntry hostInfo = Dns.GetHostByAddress("127.0.0.1");
                        // Get the IP address list that resolves to the host names contained in the 
                        // Alias property.
                        IPAddress[] addresses = hostInfo.AddressList;
                        // Get the alias names of the addresses in the IP address list.
                        string[] aliases = hostInfo.Aliases;

                        Console.WriteLine("Host name : " + hostInfo.HostName);
                        Console.WriteLine("\nAliases : ");
                        for (int index = 0; index < aliases.Length; index++)
                        {
                            Console.WriteLine(aliases[index]);
                        }
                        Console.WriteLine("\nIP address list : ");
                        for (int index = 0; index < addresses.Length; index++)
                        {
                            Console.WriteLine(addresses[index]);
                        }
                        break;
                }
            } while (answer != test.Length - 1);
        }
        // Gets an array of options, displays the options and then retuns the option that is chosen
        public static int GetOptionFromArray(string[] options)
        {
            // initial print of all options
            Console.SetCursorPosition(0, 0);
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine(options[i]);
            }
            ConsoleKeyInfo c;
            int currentOption = 0;
            int previousOption = 0;
            ConsoleColor previousColor = Console.ForegroundColor;
            ConsoleColor selectedColor = ConsoleColor.Red;

            do
            {
                Console.SetCursorPosition(0, previousOption);
                Console.ForegroundColor = previousColor;
                Console.WriteLine(options[previousOption]);
                Console.SetCursorPosition(0, currentOption);
                Console.ForegroundColor = selectedColor;
                Console.WriteLine(options[currentOption]);
                c = Console.ReadKey(true);
                previousOption = currentOption;
                if (c.Key == ConsoleKey.UpArrow && currentOption > 0) currentOption--;
                if (c.Key == ConsoleKey.DownArrow && currentOption < options.Length - 1) currentOption++;
            } while (c.Key != ConsoleKey.Enter);
            Console.ForegroundColor = previousColor;
            Console.SetCursorPosition(0, options.Length);
            return currentOption;
        }
    }
}
