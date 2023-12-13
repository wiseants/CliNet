using CliNet.Models.Commands;
using Common.Interfaces;
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

        private readonly int BUFFER_SIZE = 1024;
        private readonly Dictionary<string, Action<PacketInfo>> REQUEST_COMMAND_MAP = new Dictionary<string, Action<PacketInfo>>()
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

        /// <summary>
        /// 동작 켜기/끄기
        /// true:켜기, false:끄기.
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        } = false;

        /// <summary>
        /// 영상 스트림 받기 타입
        /// 0:유니캐스트, 1:멀티캐스트
        /// </summary>
        public int ListenType
        {
            get;
            set;
        } = 0;

        /// <summary>
        /// 영상 스트림 받기 포트 번호.
        /// </summary>
        public int ListenPortNo
        {
            get;
            set;
        } = 0;

        /// <summary>
        /// 가공된 영상 스트림 보내기 타입
        /// 0:유니캐스트, 1:멀티캐스트
        /// </summary>
        public int SendType
        {
            get;
            set;
        } = 0;


        /// <summary>
        /// 가공된 영상 스트림 보내기 IP 주소.
        /// </summary>
        public string SendIpAddress
        {
            get;
            set;
        } = "127.0.0.1";

        /// <summary>
        /// 가공된 영상 스트림 보내기 포트 번호.
        /// </summary>
        public int SendPortNo
        {
            get;
            set;
        } = 0;

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
                        if (REQUEST_COMMAND_MAP.TryGetValue(receivedPacket.Name, out Action<PacketInfo> command))
                        {
                            command(receivedPacket);
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

        private static void RunSetEnable(PacketInfo request)
        {
            Console.WriteLine("RunSetEnable");
        }

        private static void RunGetConfig(PacketInfo request)
        {
            Console.WriteLine("RunGetConfig");
        }

        private static void RunSetConfig(PacketInfo request)
        {
            Console.WriteLine("RunSetConfig");
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
