using CliNet.Interfaces;
using CommandLine;
using Grpc.Core;
using Helloworld;
using System;

namespace CliNet.Cores.Commands
{
    [Verb("echo", HelpText = "Echo by GRPC")]
    public class EchoClientCommand : IAction
    {
        #region Properties

        public bool IsValid => true;

        [Option('p', "port", Required = false, HelpText = "Service port number.")]
        public int Port
        {
            get;
            set;
        } = 30051;

        [Option('m', "message", Required = true, HelpText = "Your message.")]
        public string Message
        {
            get;
            set;
        }

        #endregion

        #region Public methods

        public int Action()
        {
            Channel channel = new Channel("127.0.0.1", Port, ChannelCredentials.Insecure);

            try
            {
                HelloReply reply = new Greeter.GreeterClient(channel).SayHello(new HelloRequest { Name = Message });
                if (reply != null && string.IsNullOrEmpty(reply.Message) == false)
                {
                    Console.WriteLine(reply.Message);
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