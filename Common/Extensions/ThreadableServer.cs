using Common.Interfaces;
using Grpc.Core;

namespace Common.Extensions
{
    public class ThreadableServer : Server, IThreadable
    {
        public void Stop()
        {
            ShutdownAsync().Wait();
        }
    }
}
