using NetworkConveyer.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConveyer.Interfaces
{
    public interface IOneTimeConnector
    {
        ConveyEndPoint RemoteEndPoint { get; }
        Task Send(ConveyPackage package);

        event EventHandlers.PackageHandler OnPackageSent;
        event EventHandlers.PackageHandler OnPackageResponsed;
    }
}
