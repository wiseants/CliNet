using Common.Interfaces;
using Grpc.Core;
using System;

namespace Common.Extensions
{
    public class ThreadableServer : Server, IThreadable
    {
        public event Action<int> Finished;

        public void Stop()
        {
            ShutdownAsync().Wait();
        }
    }
}
