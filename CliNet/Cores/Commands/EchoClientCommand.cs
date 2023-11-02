using CliNet.Interfaces;
using CommandLine;
using Common.Tools;
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

        [Option('a', "address", Required = false, HelpText = "Server IP address.")]
        public string ServerIpAddress
        {
            get;
            set;
        } = IPAddressTool.LocalIpAddress;

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
            if (string.IsNullOrEmpty(ServerIpAddress))
            {
                Console.WriteLine("IP is empty.");
                return 0;
            }

            Channel channel = new Channel(ServerIpAddress, Port, ChannelCredentials.Insecure);

            try
            {
                var reply = new FriendGreeter.FriendGreeterClient(channel).GetInfo(new InfoRequest { Key = Key });
                if (reply != null)
                {
                    Console.WriteLine("Key: {4}, Lat: {0}, Lng: {1}, Alt: {2}, Head:{3}", 
                        reply.Latitude,
                        reply.Longitude,
                        reply.Altitude, 
                        reply.Heading,
                        reply.Key);
                }
            }
            catch (RpcException ex)
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