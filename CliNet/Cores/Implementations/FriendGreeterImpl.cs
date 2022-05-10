using Friend;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace CliNet.Cores.Implementations
{
    public class FriendGreeterImpl : FriendGreeter.FriendGreeterBase
    {
        #region Override methods

        public override Task<InfoReply> GetInfo(InfoRequest request, ServerCallContext context)
        {
            Console.WriteLine("Received Key: {0}.", request.Key);

            return Task.FromResult(new InfoReply 
            { 
                Key = request.Key,
                Latitude = 0.1,
                Longitude = 0.2,
                Altitude = 0.3,
                Heading = 0.4,
            });
        }

        #endregion
    }
}