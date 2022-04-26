using AustinHarris.JsonRpc;
using Common.Network;
using NLog;
using System;
using System.Net;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Tools
{
    /// <summary>
    /// RPC 도구.
    /// </summary>
    public static class RpcTool
    {
        #region Fields

        public static readonly int WAITING_DELAY_MS = 20;

        #endregion

        #region Public methods

        /// <summary>
        /// RPC 메소드 실행.
        /// </summary>
        /// <typeparam name="T">반환 타입.</typeparam>
        /// <param name="serviceEndpoint">서비스 엔드포인트.</param>
        /// <param name="command">메소드 이름.</param>
        /// <param name="args">메소드 파라미터.</param>
        /// <returns>반환 값.</returns>
        public static async Task<T> Command<T>(this IPEndPoint serviceEndpoint, string command, params object[] args)
        {
            return await Task.Run<T>(() =>
            {
                T taskResult = default;
                bool isFinished = false;

                if (serviceEndpoint == null || string.IsNullOrEmpty(command))
                {
                    return default;
                }

                try
                {
                    new JsonRpcClient(serviceEndpoint, Encoding.UTF8).Invoke<T>(command, args).Subscribe(new AnonymousObserver<JsonResponse<T>>(
                    x =>
                    {
                        taskResult = x.Result;
                    },
                    (ex) =>
                    {
                        LogManager.GetCurrentClassLogger().Error("Exception occurred. Message({0})", ex.Message);
                        isFinished = true;
                    },
                    () =>
                    {
                        isFinished = true;
                    })).Dispose();

                    while (isFinished == false)
                    {
                        Thread.Sleep(WAITING_DELAY_MS);
                    }
                }
                catch(Exception ex)
                {
                    LogManager.GetCurrentClassLogger().Error("Exception occurred. Message({0})", ex.Message);
                }

                return taskResult;
            });
        }

        /// <summary>
        /// RPC 결과를 반환하는 메소드 실행.
        /// </summary>
        /// <param name="serviceEndpoint">서비스 엔드포인트.</param>
        /// <param name="command">메소드 이름.</param>
        /// <param name="args">메소드 파라미터.</param>
        /// <returns>반환 값.</returns>
        public static async Task<bool> Command(this IPEndPoint serviceEndpoint, string command, params object[] args)
        {
            return await serviceEndpoint.Command<bool>(command, args);
        }

        #endregion
    }
}