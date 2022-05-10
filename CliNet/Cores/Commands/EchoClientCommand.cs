using CliNet.Interfaces;
using CommandLine;
using Friend;
using Grpc.Core;
using System;

namespace CliNet.Cores.Commands
{
    [Verb("echo", HelpText = "Echo by GRPC")]
    public class EchoClientCommand : IAction
    {
        #region Properties

        public bool IsValid => true;

        [Option('i', "identifier", Required = false, HelpText = "Request Identifier")]
        public string Key
        {
            get;
            set;
        } = "default";

        [Option('p', "port", Required = false, HelpText = "Service port number.")]
        public int Port
        {
            get;
            set;
        } = 30051;

        #endregion

        #region Public methods

        public int Action()
        {
            Channel channel = new Channel("127.0.0.1", Port, ChannelCredentials.Insecure);

            try
            {
                var reply = new FriendGreeter.FriendGreeterClient(channel).GetInfo(new InfoRequest { Key = Key });
                if (reply != null)
                {
                    Console.WriteLine("Lat: {0}, Lng: {1}, Alt: {2}, Head:{3}", reply.Latitude, reply.Longitude, reply.Altitude, reply.Heading);
                }
            }
            catch (RpcException)
            {
                Console.WriteLine("Could not call.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred. Message({0})", ex.Message);
            }

            return 0;
        }

        #endregion
    }
}