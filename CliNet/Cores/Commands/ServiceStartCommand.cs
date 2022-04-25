using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;
using System;

namespace CliNetCore.Cores.Commands
{
    [Verb("service", HelpText = "Start service.")]
    public class ServiceStartCommand : IAction
    {
        #region Properties

        public bool IsValid => true;

        [Option('p', "port", Required = true, HelpText = "Service port number.")]
        public int Port { get; set; }

        #endregion

        #region Public methods

        public int Action()
        {
            RpcServiceManager.Instance.Start(Port);

            return 1;
        }

        #endregion
    }
}
