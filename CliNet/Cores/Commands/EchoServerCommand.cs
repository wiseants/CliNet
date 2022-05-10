﻿using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;
using Common.Extensions;
using Grpc.Core;
using Helloworld;
using System;
using System.Threading.Tasks;

namespace CliNet.Cores.Commands
{
    public class GreeterImpl : Greeter.GreeterBase
    {
        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = "Echo: " + request.Name });
        }
    }

    [Verb("echoserver", HelpText = "Run Echo-server.")]
    public class EchoServerCommand : IAction
    {
        #region Properties

        public bool IsValid => true;

        [Option('i', "identifier", Required = false, HelpText = "Listener Identifier")]
        public string Key
        {
            get;
            set;
        } = "default";

        [Option("stop", Required = false, HelpText = "Stop UDP listener.")]
        public bool IsStop
        {
            get;
            set;
        } = false;


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
            if (IsStop)
            {
                ThreadManager.Instance.Remove(Key);

                Console.WriteLine("Stop a UDP-Bypass.");

                return 0;
            }

            ThreadableServer server = new ThreadableServer
            {
                Services = { Greeter.BindService(new GreeterImpl()) },
                Ports = { new ServerPort("127.0.0.1", Port, ServerCredentials.Insecure) }
            };

            ThreadManager.Instance.Add(Key, server);

            Console.WriteLine("Start a UDP-Bypass.");

            return 0;
        }

        #endregion
    }
}
