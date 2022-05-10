using CliNet.Interfaces;
using CommandLine;
using Common.Tools;
using Grpc.Core;
using Helloworld;
using System;
using System.Net;

namespace CliNetCore.Cores.Commands
{
    [Verb("increase", HelpText = "Increase number")]
    public class IncreaseCommand : IAction
    {
        public bool IsValid => true;

        [Option('p', "port", Required = false, HelpText = "Service port number.")]
        public int Port
        {
            get;
            set;
        } = 8055;

        [Option('t', "target", Required = true, HelpText = "target number.")]
        public int TargetNumber
        {
            get;
            set;
        }

        public int Action()
        {
            Channel channel = new Channel("127.0.0.1:30051", ChannelCredentials.Insecure);

            var client = new Greeter.GreeterClient(channel);
            string user = "you";

            var reply = client.SayHello(new HelloRequest { Name = user });

            Console.WriteLine(reply.Message);

            return 0;
        }
    }
}
