using NetworkConveyer.Interfaces;
using NetworkConveyer.Objects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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
                socket.SendTo(package.Raw, RemoteEndPoint.ToIPEndPoint());
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
