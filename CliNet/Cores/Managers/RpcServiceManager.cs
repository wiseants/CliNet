using AustinHarris.JsonRpc;
using Common.Network;
using NLog;
using System;
using System.Linq;

namespace CliNet.Cores.Managers
{
    public class RpcServiceManager : JsonRpcServer
    {
        #region Fields

        private static RpcServiceManager _instance = null;
        private static readonly object lockObject = new object();

        #endregion

        #region Properties

        /// <summary>
        /// 싱글톤 인스턴스.
        /// </summary>
        public static RpcServiceManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (lockObject)
                    {
                        _instance = new RpcServiceManager
                        {
                            Services = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(s => s.GetTypes())
                                .Where(p => typeof(JsonRpcService).IsAssignableFrom(p) && p.IsAbstract == false)
                                .Select(type => (JsonRpcService)Activator.CreateInstance(type)).ToList()
                        };

                        LogManager.GetCurrentClassLogger().Info("Found {0} services.", Instance.Services.Count);
                    }
                }

                return _instance;
            }
        }

        #endregion

        #region Public methods

        public void Release()
        {
            Instance.Stop();
        }

        #endregion
    }
}
