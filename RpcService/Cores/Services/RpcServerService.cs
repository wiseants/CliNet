using AustinHarris.JsonRpc;
using Common.Network;
using System;
using System.Data;
using System.Linq;
using System.ServiceProcess;

namespace RpcService.Cores.Services
{
    public partial class RpcServerService : ServiceBase
    {
        #region Fields

        private readonly JsonRpcServer _rpcServer = new JsonRpcServer();

        #endregion

        #region Constructors

        public RpcServerService()
        {
            InitializeComponent();
        }

        #endregion

        #region Override methods

        protected override void OnStart(string[] args)
        {
            if (args == null || args.Length <= 0)
            {
                return;
            }

            if (int.TryParse(args[0], out int port) == false)
            {
                return;
            }

            _rpcServer.Services = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(JsonRpcService).IsAssignableFrom(p) && p.IsAbstract == false)
                .Select(type => (JsonRpcService)Activator.CreateInstance(type)).ToList();

            _rpcServer.Start(port);
        }

        protected override void OnStop()
        {
            if (_rpcServer.IsRunning)
            {
                _rpcServer.Stop();
            }
        }

        #endregion
    }
}