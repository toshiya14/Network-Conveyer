using NetworkConveyer;
using NetworkConveyer.Interfaces;
using NetworkConveyer.Objects;
using System;
using System.Text;

namespace ConsoleTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var local = "0.0.0.0:10083";
            Console.WriteLine("Started UDP Server @ " + local);
            var server = new UDPListener(local);
            server.OnListenerStarted += onServerStarted;
            server.OnListenerStopped += onServerStopped;
            server.OnPackageReceived += onMessageReceived;
            server.StartListener();
            Console.WriteLine("Press any key to stop");
            Console.ReadKey();
            server.StopListener().Wait();
            Environment.Exit(0);
        }

        private static void onMessageReceived(ConveyPackage package)
        {
            Console.WriteLine($"Received package from: {package.Source.ToIPEndPoint()}\n  * Size: {package.Raw.Length}\n  * String: {Encoding.UTF8.GetString(package.Raw)}\n  * Hexmap: {package.ToHexmap()}\n\n");
        }

        private static void onServerStopped(ISimplexListener listener)
        {
            Console.WriteLine("Listener stopped.");
        }

        private static void onServerStarted(ISimplexListener listener)
        {
            Console.WriteLine("Listener started successfully.");
        }
    }
}
