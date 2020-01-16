using NetworkConveyer;
using NetworkConveyer.Interfaces;
using NetworkConveyer.Objects;
using System;
using System.Net.Sockets;
using System.Text;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var remote = "10.172.44.33:10083";
            Console.WriteLine("Started UDP Connector To " + remote);
            var client = new UDPConnector(remote);
            client.OnPackageSent += onSent;
            while (true)
            {
                Console.Write("> ");
                var text = Console.ReadLine();
                if (text.StartsWith("exit"))
                {
                    break;
                }else if (text.StartsWith("HEX"))
                {
                    var hexmap = text.Substring(3).Trim();
                    var package = new ConveyPackage();
                    package.ParseHexMap(hexmap);
                    client.Send(package).Wait();
                }
                else
                {
                    var bytes = Encoding.UTF8.GetBytes(text);
                    var package = new ConveyPackage(bytes);
                    client.Send(package).Wait();
                }
            }
        }

        private static void onSent(ConveyPackage package)
        {
            Console.WriteLine("Package Sent.");
        }

    }
}
