using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkConveyer
{
    public static class GlobalSettings
    {
        public const uint LISTENER_BUFFER_SIZE = 1024;
        public const int LISTENER_WAITTIME_WHILE_FREE = 500;
        public const int LISTENER_DEFAULT_PORT = 44193;
    }
}
