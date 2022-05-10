using Grpc.Core;
using Helloworld;
using System.Threading.Tasks;

namespace CliNet.Cores.Implementations
{
    public class GreeterImpl : Greeter.GreeterBase
    {
        #region Override methods

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = "Echo: " + request.Name });
        }

        #endregion
    }
}