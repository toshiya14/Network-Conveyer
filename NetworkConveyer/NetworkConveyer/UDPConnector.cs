using NetworkConveyer.Interfaces;
using NetworkConveyer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using _G = NetworkConveyer.GlobalSettings;

namespace NetworkConveyer
{
    public class UDPConnector : IOneTimeConnector
    {
        public ConveyEndPoint RemoteEndPoint { get; private set; }

        public event EventHandlers.PackageHandler OnPackageSent;
        public event EventHandlers.PackageHandler OnPackageResponsed;

        public UDPConnector(string remote)
        {
            SetRemote(remote);
        }

        public void SetRemote(string raw)
        {
            RemoteEndPoint = new ConveyEndPoint(raw, EndPointType.IP);
        }

        public async Task Send(ConveyPackage package)
        {
            var socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            package.Target = RemoteEndPoint;
            await Task.Factory.StartNew(()=> {
                var p = 0;
                while (p<package.Raw.Length)
                {
                    var byteCount = Math.Min(package.Raw.Length - p, _G.LISTENER_BUFFER_SIZE);
                    var byteToSend = package.Raw.Skip(p).Take(byteCount).ToArray();
                    p += byteCount;
                    socket.SendTo(byteToSend, RemoteEndPoint.ToIPEndPoint());
                }
                OnPackageSent?.Invoke(package);
            });
        }

        public async Task Broadcast(ConveyPackage package)
        {
            var socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            var iep = new IPEndPoint(IPAddress.Broadcast, RemoteEndPoint.ToIPEndPoint().Port);
            package.Target = new ConveyEndPoint(iep);
            await Task.Factory.StartNew(() =>
            {
                socket.EnableBroadcast = true;
                socket.SendTo(package.Raw, package.Target.ToIPEndPoint());
                OnPackageSent?.Invoke(package);
            });
        }
    }
}
