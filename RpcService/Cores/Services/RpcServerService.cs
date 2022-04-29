using AustinHarris.JsonRpc;
using Common.Network;
using NLog;
using NLog.Config;
using System;
using System.Data;
using System.Linq;
using System.ServiceProcess;

namespace RpcService.Cores.Services
{
    public partial class RpcServerService : ServiceBase
    {
        #region Fields

        public readonly string LOG_LAYOUT = "${longdate} | ${uppercase:${level}} | ${logger} | ${message}";
        public readonly string TRACE_LAYOUT = "${uppercase:${level}} | ${message} | ${logger}";
        public readonly string SRTM_CACHE_FOLDER_NAME = "srtm_caches";

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
            ConfigurationForLog();

            if (args == null || args.Length <= 0)
            {
                LogManager.GetCurrentClassLogger().Error("Null parameter.");
                return;
            }

            if (int.TryParse(args[0], out int port) == false)
            {
                LogManager.GetCurrentClassLogger().Error("Invalid parameter. Parameter({0})", args[0]);
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

        #region Private methods

        /// <summary>
        /// 로그 셋팅.
        /// </summary>
        private void ConfigurationForLog()
        {
            var config = new LoggingConfiguration();

            // 파일 로그 룰.
            config.AddRule(LogLevel.Info, LogLevel.Fatal, new NLog.Targets.FileTarget()
            {
                FileName = string.Format(@"{0}${{shortdate}}.log", Constant.LOG_FOLDER_PATH),
                Layout = LOG_LAYOUT
            });

            LogManager.Configuration = config;
        }

        #endregion
    }
}