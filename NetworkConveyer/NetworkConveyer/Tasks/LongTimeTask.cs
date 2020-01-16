using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NetworkConveyer;

namespace NetworkConveyer.Tasks
{
    internal abstract class LongTimeTask
    {
        public event EventHandlers.NoArgsHandler TaskStarted;
        public event EventHandlers.NoArgsHandler TaskStopped;

        protected bool isTaskBusy { get; private set; }
        protected abstract string logContextName { get; }
        protected CancellationToken taskToken => listenTaskTokenSource.Token;

        private Task currentListenTask;
        private CancellationTokenSource listenTaskTokenSource;

        public async Task StopTask()
        {
            if (!isTaskBusy)
            {
                Trace.TraceWarning("Task is not started, nothing to do.");
            }

            listenTaskTokenSource.Cancel();
            await currentListenTask;

            isTaskBusy = false;
            TaskStopped?.Invoke();
        }

        public virtual void StartTask()
        {
            if (isTaskBusy)
            {
                Trace.TraceWarning("Task already started, nothing to do.");
                return;
            }
            else
            {
                listenTaskTokenSource = new CancellationTokenSource();
                isTaskBusy = true;
                TaskStarted?.Invoke();
            }
        }

        public bool IsTaskBusy() => isTaskBusy;

        protected void queueTask(Action action)
        {
            if(listenTaskTokenSource == null)
            {
                listenTaskTokenSource = new CancellationTokenSource();
            }
            currentListenTask = Task.Factory.StartNew(action, listenTaskTokenSource.Token);
        }
    }
}
