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

        public int Action()
        {
            Channel channel = new Channel("127.0.0.1", Port, ChannelCredentials.Insecure);

            var client = new Greeter.GreeterClient(channel);

            var reply = client.SayHello(new HelloRequest { Name = Message });

            Console.WriteLine(reply.Message);

            return 0;
        }
    }
}
