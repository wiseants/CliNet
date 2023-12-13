using CliNet.Models.Commands;
using Common.Interfaces;
using Common.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CliNet.Cores.Implementations
{
    public class AiModuleServer : IThreadable
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

        public AiModuleServer() 
        {
            _thread = new Thread(ThreadProc);
        }

        #endregion

        #region Properties

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

        #region Private methods

        private void ThreadProc()
        {
            try
            {
                _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, Port);

                _sock.Bind(serverEP);
                _sock.Listen(10);

                do
                {
                    Socket client = _sock.Accept();

                    byte[] buffer = new byte[BUFFER_SIZE];
                    int receivedLength = client.Receive(buffer);

                    string request = Encoding.Default.GetString(buffer, 0, receivedLength);
                    Console.WriteLine($"받은 명령:\n {request}");

                    string response = "{}";

                    PacketInfo receivedPacket = JsonConvert.DeserializeObject<PacketInfo>(request);
                    if (receivedPacket != null)
                    {
                        if (REQUEST_COMMAND_MAP.TryGetValue(receivedPacket.Name, out Action<string> command))
                        {
                            command(request);
                        }

                        if (RESPONSE_BUILDER_MAP.TryGetValue(receivedPacket.Name, out Func<PacketInfo, object> builder))
                        {
                            response = JsonConvert.SerializeObject(builder(receivedPacket));
                        }
                    }

                    client.Send(Encoding.ASCII.GetBytes(response), SocketFlags.None);
                    Console.WriteLine($"보낸 명령:\n {response}");
                }
                while (_sock != null);
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine($"서버를 종료합니다.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예외 발생: {ex.Message}");
            }

            Finished?.Invoke(0);
        }

        private static void RunSetEnable(string request)
        {
            SetEnableRequestInfo requestInfo = JsonConvert.DeserializeObject<SetEnableRequestInfo>(request);
            if (requestInfo != null)
            {
                AppConfiguration.SetAppConfig(IS_ENALBE_KEY, requestInfo.IsEnable.ToString());
            }
        }

        private static void RunGetConfig(string request)
        {
        }

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

        private static object BuildSetEnableResponse(PacketInfo request)
        {
            SetEnableResponseInfo result = new SetEnableResponseInfo()
            {
                SeqNo = request.SeqNo,
                ReturnCode = 1,
            };

            return result;
        }

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
