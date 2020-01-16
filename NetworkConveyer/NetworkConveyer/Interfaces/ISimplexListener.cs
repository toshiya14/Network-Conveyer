using NetworkConveyer.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConveyer.Interfaces
{
    public interface ISimplexListener
    {
        ConveyEndPoint LocalEndPoint { get; }
        void StartListener();
        Task StopListener();

        event EventHandlers.SimplexListenerHandler OnListenerStarted;
        event EventHandlers.SimplexListenerHandler OnListenerStopped;
        event EventHandlers.PackageHandler OnPackageReceived;
    }
}
