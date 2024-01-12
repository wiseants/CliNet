using CliNet.Models.Commands;
using Common.Interfaces;
using Common.Tools;
using MiscUtil;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CliNet.Cores.Implementations
{
    public class TrackerModuleServer : IThreadable
    {
        #region Events

        public event Action<int> Finished;

        #endregion

        #region Fields

        private static readonly string IS_ENALBE_KEY = "IsEnable";
        private static readonly string LISTEN_TYPE = "ListenType";
        private static readonly string LISTEN_PORT_NO = "ListenPortNo";
        private static readonly string SEND_TYPE = "SendType";
        private static readonly string SEND_IP_ADDRESS = "SendIpAddress";
        private static readonly string SEND_PORT_NO = "SendPortNo";

        private readonly int BUFFER_SIZE = 1024;

        private readonly Dictionary<string, Action<string>> REQUEST_COMMAND_MAP = new Dictionary<string, Action<string>>()
        {
            { "SetEnable", RunSetEnable },
            { "GetConfig", RunGetConfig },
            { "SetConfig", RunSetConfig },
        };
        private readonly Dictionary<string, Func<PacketInfo, object>> RESPONSE_BUILDER_MAP = new Dictionary<string, Func<PacketInfo, object>>()
        {
            { "SetEnable", BuildSetEnableResponse },
            { "GetConfig", BuildGetConfigResponse },
            { "SetConfig", BuildSetConfigResponse },
        };

        private readonly Thread _thread;
        private Socket _sock;

        #endregion

        #region Constructors

        public TrackerModuleServer() 
        {
            _thread = new Thread(ThreadProc);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 서버 포트 번호.
        /// </summary>
        public int Port
        {
            get;
            set;
        }

        #endregion

        #region Public methods

        public void Start()
        {
            _thread.Start();
        }

        public void Stop()
        {
            _sock?.Close();
            _sock = null;

            _thread.Abort();
        }

        #endregion

        #region Event handlers

        private void ThreadProc()
        {
            using (UdpClient udpClient = new UdpClient(Port))
            {
                udpClient.Client.SendTimeout = 100;
                udpClient.Client.ReceiveTimeout = 150;

                while(true)
                {
                    try
                    {
                        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);
                        byte[] receivedBuffer = udpClient.Receive(ref endPoint);

                        string request = Encoding.Default.GetString(receivedBuffer, 0, receivedBuffer.Length);
                        Console.WriteLine($"받은 명령:\n {request}");

                        receivedBuffer = Encoding.UTF8.GetBytes("{response}");
                        int sentByteNumber = udpClient.Send(receivedBuffer, receivedBuffer.Length, endPoint);
                        if (sentByteNumber != receivedBuffer.Length)
                        {
                        }

                        Console.WriteLine($"보낸 사이즈:\n {sentByteNumber}");
                    }
                    catch { }

                    Thread.Sleep(100);
                }
            }

            Finished?.Invoke(0);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// SetEnable 요청 메소드.
        /// </summary>
        /// <param name="request"></param>
        private static void RunSetEnable(string request)
        {
            SetEnableRequestInfo requestInfo = JsonConvert.DeserializeObject<SetEnableRequestInfo>(request);
            if (requestInfo != null)
            {
                AppConfiguration.SetAppConfig(IS_ENALBE_KEY, requestInfo.IsEnable.ToString());
            }
        }

        /// <summary>
        /// GetConfig 요청 메소드.
        /// </summary>
        /// <param name="request"></param>
        private static void RunGetConfig(string request)
        {
        }

        /// <summary>
        /// SetConfig 요청 메소드.
        /// </summary>
        /// <param name="request"></param>
        private static void RunSetConfig(string request)
        {
            SetConfigRequestInfo requestInfo = JsonConvert.DeserializeObject<SetConfigRequestInfo>(request);
            if (requestInfo != null)
            {
                AppConfiguration.SetAppConfig(LISTEN_TYPE, requestInfo.ListenType.ToString());
                AppConfiguration.SetAppConfig(LISTEN_PORT_NO, requestInfo.ListenPortNo.ToString());
                AppConfiguration.SetAppConfig(SEND_TYPE, requestInfo.SendType.ToString());
                AppConfiguration.SetAppConfig(SEND_IP_ADDRESS, requestInfo.SendIpAddress.ToString());
                AppConfiguration.SetAppConfig(SEND_PORT_NO, requestInfo.SendPortNo.ToString());
            }
        }

        /// <summary>
        /// SetEnable 응답 모델 빌더.
        /// </summary>
        /// <param name="request">요청 패킷.</param>
        /// <returns>응답 객체.</returns>
        private static object BuildSetEnableResponse(PacketInfo request)
        {
            SetEnableResponseInfo result = new SetEnableResponseInfo()
            {
                SeqNo = request.SeqNo,
                ReturnCode = 1,
            };

            return result;
        }

        /// <summary>
        /// GetConfig 응답 모델 빌더.
        /// </summary>
        /// <param name="request">요청 패킷.</param>
        /// <returns>응답 객체.</returns>
        private static object BuildGetConfigResponse(PacketInfo request)
        {
            GetConfigResponseInfo result = new GetConfigResponseInfo()
            {
                SeqNo = request.SeqNo,
                IsEnable = Convert.ToBoolean(AppConfiguration.GetAppConfig(IS_ENALBE_KEY)),
                ListenType = Convert.ToInt32(AppConfiguration.GetAppConfig(LISTEN_TYPE)),
                ListenPortNo = Convert.ToInt32(AppConfiguration.GetAppConfig(LISTEN_PORT_NO)),
                SendType = Convert.ToInt32(AppConfiguration.GetAppConfig(SEND_TYPE)),
                SendIpAddress = AppConfiguration.GetAppConfig(SEND_IP_ADDRESS),
                SendPortNo = Convert.ToInt32(AppConfiguration.GetAppConfig(SEND_PORT_NO)),
                ReturnCode = 1,
            };

            return result;
        }

        /// <summary>
        /// SetConfig 응답 모델 빌더.
        /// </summary>
        /// <param name="request">요청 패킷.</param>
        /// <returns>응답 객체.</returns>
        private static object BuildSetConfigResponse(PacketInfo request)
        {
            SetConfigResponseInfo result = new SetConfigResponseInfo()
            {
                SeqNo = request.SeqNo,
                ReturnCode = 1,
            };

            return result;
        }

        #endregion
    }
}
