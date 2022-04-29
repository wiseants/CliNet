// https://github.com/Astn/JSON-RPC.NET/wiki/Getting-Started-(Sockets) 인용.

using AustinHarris.JsonRpc;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace Common.Network
{
    /// <summary>
    ///  Json 포멧을 이용한 RPC 서버 클래스.
    /// </summary>
    public class JsonRpcServer
    {
        #region Fields

        private SocketListener socketListener = null;
        private CancellationTokenSource tokenSource = null;

        #endregion

        #region Properties

        public bool IsRunning => socketListener != null && socketListener.Server.IsBound == true;

        public ICollection<JsonRpcService> Services
        {
            get;
            set; 
        } = new List<JsonRpcService>();

        #endregion

        #region Public methods

        public void Start(int port)
        {
            if (IsRunning == true)
            {
                Console.WriteLine("Service already running.");
                return;
            }

            var rpcResultHandler = new AsyncCallback(state =>
            {
                var async = ((JsonRpcStateAsync)state);
                var result = async.Result;
                var writer = ((StreamWriter)async.AsyncState);

                writer.WriteLine(result);
                writer.FlushAsync();
            });

            socketListener = new SocketListener(IPAddress.Parse("127.0.0.1"), port);
            tokenSource = new CancellationTokenSource();

            socketListener.StartAsync((writer, line) =>
            {
                var async = new JsonRpcStateAsync(rpcResultHandler, writer)
                {
                    JsonRpc = line
                };

                JsonRpcProcessor.Process(async, writer);
            }, tokenSource.Token);

            LogManager.GetCurrentClassLogger().Info("Service started. PortNo({0})", port);
        }

        public void Stop()
        {
            if (IsRunning == false)
            {
                Console.WriteLine("Service is not running.");
                return;
            }

            tokenSource.Cancel();

            LogManager.GetCurrentClassLogger().Info("Service stopped.");
        }

        #endregion
    }
}