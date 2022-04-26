using AustinHarris.JsonRpc;
using Common.Network;
using Common.Templates;
using NLog;
using System;
using System.Linq;

namespace CliNet.Cores.Managers
{
    public class RpcServiceManager : Singleton<RpcServiceManager>
    {
        #region Fields

        private readonly JsonRpcServer _rpcServer = new JsonRpcServer();

        #endregion

        #region Constructors

        public RpcServiceManager()
        {
            _rpcServer.Services = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(JsonRpcService).IsAssignableFrom(p) && p.IsAbstract == false)
                .Select(type => (JsonRpcService)Activator.CreateInstance(type)).ToList();
        }

        #endregion

        #region Public methods

        public void Start(int port)
        {
            _rpcServer.Start(port);

            LogManager.GetCurrentClassLogger().Info("Start {0} services.", _rpcServer.Services.Count);
        }

        public void Stop()
        {
            _rpcServer.Stop();

            LogManager.GetCurrentClassLogger().Info("Stop services.");
        }

        public void Release()
        {
            Stop();
        }

        #endregion
    }
}
