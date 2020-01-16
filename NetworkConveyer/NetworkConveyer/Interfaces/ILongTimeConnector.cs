using NetworkConveyer.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConveyer.Interfaces
{
    public interface ILongTimeConnector
    {
        ConveyEndPoint RemoteEndPoint { get; }
        void Open();
        void Close();
        void Send(ConveyPackage package);

        event EventHandlers.LongTimeConnectorHandler OnConnected;
        event EventHandlers.LongTimeConnectorHandler OnDisconnected;
        event EventHandlers.PackageHandler OnPackageSent;
        event EventHandlers.PackageHandler OnPackageReceived;
    }
}
