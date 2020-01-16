using NetworkConveyer.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using _G = NetworkConveyer.GlobalSettings;

namespace NetworkConveyer.Tasks
{
    class ListenerTask : LongTimeTask
    {
        protected override string logContextName => _logContext;

        public event EventHandlers.PackageHandler OnMessageReceived;

        private string _logContext;
        private Socket listenSocket;

        public ListenerTask(Socket listenSocket, string logContextName = "ListenerTask")
        {
            this._logContext = logContextName;
            this.listenSocket = listenSocket;
        }

        public override void StartTask()
        {
            base.StartTask();

            queueTask(() => {
                var buffer = new byte[_G.LISTENER_BUFFER_SIZE];
                var epReceiveFrom = listenSocket.LocalEndPoint;
                var freeStateLogged = false;

                while (!taskToken.IsCancellationRequested)
                {
                    try
                    {
                        if (listenSocket.ProtocolType == ProtocolType.Tcp)
                        {
                            if (!listenSocket.Connected)
                            {
                                if (!freeStateLogged)
                                {
                                    Trace.TraceInformation("No client connected now.");
                                    freeStateLogged = true;
                                }
                                Task.Delay(_G.LISTENER_WAITTIME_WHILE_FREE).Wait();
                                continue;
                            }

                            if (freeStateLogged)
                            {
                                Trace.TraceInformation("One or more clients connected.");
                                freeStateLogged = false;
                            }
                        }

                        int recvCount;
                        if (listenSocket.ProtocolType == ProtocolType.Udp)
                        {
                            recvCount = listenSocket.ReceiveFrom(buffer, ref epReceiveFrom);
                        }
                        else
                        {
                            recvCount = listenSocket.Receive(buffer);
                            epReceiveFrom = listenSocket.RemoteEndPoint;
                        }
                        
                        if (recvCount > 0 && buffer != null)
                        {
                            var iep = epReceiveFrom as IPEndPoint;
                            var src = iep.Address.ToString() + ":" + iep.Port;
                            Trace.TraceInformation($"Received message from {src}");
                            var package = new ConveyPackage(buffer.Take(recvCount).ToArray());
                            package.Source = new ConveyEndPoint(iep);
                            OnMessageReceived?.Invoke(package);
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError($"[{ex.Source}] {ex.Message}\n{ex.StackTrace}");
                    }
                }

                listenSocket.Close();
            });
        }

    }
}
