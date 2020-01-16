using NetworkConveyer.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConveyer.Interfaces
{
    public interface IDuplexListener
    {
        ConveyEndPoint LocalEndPoint { get; }
        void StartListener();
        void Send(ConveyPackage package);
        void StopListener();

        event EventHandlers.SimplexListenerHandler OnListenerStarted;
        event EventHandlers.SimplexListenerHandler OnListenerStopped;
        event EventHandlers.PackageHandler OnPackageReceived;
        event EventHandlers.PackageHandler OnPackageSent;
    }
}
