using NetworkConveyer.Interfaces;
using NetworkConveyer.Objects;
using NetworkConveyer.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConveyer
{
    public class EventHandlers
    {
        public delegate void SimplexListenerHandler(ISimplexListener listener);
        public delegate void DuplexListenerHandler(IDuplexListener listener);
        public delegate void LongTimeConnectorHandler(ILongTimeConnector connector);
        public delegate void OneTimeConnectorHandler(IOneTimeConnector connector);
        public delegate void PackageHandler(ConveyPackage package);
        public delegate void NoArgsHandler();
    }
}
