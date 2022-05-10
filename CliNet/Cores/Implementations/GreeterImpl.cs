using Grpc.Core;
using Helloworld;
using System;
using System.Threading.Tasks;

namespace CliNet.Cores.Implementations
{
    public class GreeterImpl : Greeter.GreeterBase
    {
        #region Override methods

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            Console.WriteLine("Received {0}.", request.Name);

            return Task.FromResult(new HelloReply { Message = "Echo: " + request.Name });
        }

        #endregion
    }
}