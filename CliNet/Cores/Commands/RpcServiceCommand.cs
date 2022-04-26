using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;
using System;

namespace CliNetCore.Cores.Commands
{
    [Verb("rpcservice", HelpText = "Start RPC service.")]
    public class ServiceStartCommand : IAction
    {
        #region Properties

        public bool IsValid => true;

        [Option('p', "port", Required = false, HelpText = "Service port number.")]
        public int Port
        { 
            get;
            set;
        } = 8055;

        [Option("stop", Required = false, HelpText = "Stop RPC service.")]
        public bool IsStop
        {
            get;
            set;
        } = false;

        #endregion

        #region Public methods

        public int Action()
        {
            if (IsStop)
            {
                RpcServiceManager.Instance.Stop();
            }
            else
            {
                RpcServiceManager.Instance.Start(Port);
            }

            Console.WriteLine("{0} a RPC-Service.", IsStop ? "Stop" : "Start");

            return 1;
        }

        #endregion
    }
}
