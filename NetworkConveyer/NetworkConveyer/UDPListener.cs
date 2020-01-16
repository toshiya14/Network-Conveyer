using NetworkConveyer.Interfaces;
using NetworkConveyer.Objects;
using NetworkConveyer.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkConveyer
{
    public class UDPListener : ISimplexListener
    {
        public ConveyEndPoint LocalEndPoint { get; private set; }

        public event EventHandlers.SimplexListenerHandler OnListenerStarted;
        public event EventHandlers.SimplexListenerHandler OnListenerStopped;
        public event EventHandlers.PackageHandler OnPackageReceived;

        private Socket listenSocket;
        private ListenerTask listenerTask;

        public UDPListener(string localIP)
        {
            SetLocal(localIP);
        }

        public void SetLocal(string raw)
        {
            var cep = new ConveyEndPoint(raw, EndPointType.IP);
            LocalEndPoint = cep;
        }
        public void StartListener()
        {
            if(listenerTask != null && listenerTask.IsTaskBusy())
            {
                Trace.TraceWarning("Listener task is busy, nothing to do.");
                return;
            }
            listenSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            listenSocket.Bind(LocalEndPoint.ToIPEndPoint());

            listenerTask = new ListenerTask(listenSocket, "UDPListener");
            listenerTask.TaskStarted += () => {
                OnListenerStarted?.Invoke(this);
            };
            listenerTask.TaskStopped += () => {
                OnListenerStopped?.Invoke(this);
            };
            listenerTask.OnMessageReceived += p =>
            {
                OnPackageReceived?.Invoke(p);
            };
            listenerTask.StartTask();
        }
        public async Task StopListener()
        {
            if (listenerTask == null || !listenerTask.IsTaskBusy())
            {
                Trace.TraceWarning("Listener task is not running, nothing to do.");
                return;
            }
            await listenerTask.StopTask();
        }
    }
}
