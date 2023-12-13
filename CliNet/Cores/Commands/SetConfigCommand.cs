using CliNet.Cores.Managers;
using CliNet.Models.Commands;
using CommandLine;
using Common.Tools;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CliNet.Cores.Commands
{
    [Verb("set.config", HelpText = "설정 쓰기 명령을 보냅니다.")]
    internal class SetConfigCommand : Interfaces.IAction
    {
        #region Fields

        private readonly int BUFFER_SIZE = 1024;

        #endregion

        #region Properties

        public bool IsValid => true;

        [Option('a', "address", Required = false, HelpText = "서버 IP 주소.")]
        public string ServerIpAddress
        {
            get;
            set;
        } = IPAddressTool.LocalIpAddress;

        [Option('p', "port", Required = false, HelpText = "서버 포트 번호.")]
        public int Port
        {
            get;
            set;
        } = 15300;

        [Option('t', "timeout", Required = false, HelpText = "타임아웃 시간(ms)")]
        public int Timeout
        {
            get;
            set;
        } = 2000;

        [Option('l', "listen.type", Required = false, HelpText = "받기 스트리밍 타입. 0:유니캐스트, 1:멀티캐스트")]
        public int ListenType
        {
            get;
            set;
        } = 0;

        [Option('i', "listen.port", Required = false, HelpText = "받기 스트리밍 포트번호.")]
        public int ListenPortNo
        {
            get;
            set;
        } = 0;

        [Option('s', "send.type", Required = false, HelpText = "보내기 스트리밍 타입. 0:유니캐스트, 1:멀티캐스트")]
        public int SendType
        {
            get;
            set;
        } = 0;


        [Option('n', "send.ip", Required = false, HelpText = "보내기 스트리밍 IP 주소.")]
        public string SendIpAddress
        {
            get;
            set;
        } = "127.0.0.1";

        [Option('d', "send.port", Required = false, HelpText = "보내기 스트리밍 포트번호.")]
        public int SendPortNo
        {
            get;
            set;
        } = 0;

        #endregion

        #region Public methods

        public int Action()
        {
            try
            {
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ServerIpAddress), Port);
                    sock.Connect(endPoint);
                    sock.SendTimeout = Timeout;
                    sock.ReceiveTimeout = Timeout;

                    SetConfigRequestInfo requestInfo = new SetConfigRequestInfo()
                    {
                        SeqNo = SequenceManager.Instance.GetNext(),
                        ListenType = ListenType,
                        ListenPortNo = ListenPortNo,
                        SendType = SendType,
                        SendIpAddress = SendIpAddress,
                        SendPortNo = SendPortNo,
                    };

                    string request = JsonConvert.SerializeObject(requestInfo);
                    Console.WriteLine($"보낸 명령:\n {request}");

                    sock.Send(Encoding.ASCII.GetBytes(request), SocketFlags.None);

                    byte[] receiverBuff = new byte[BUFFER_SIZE];
                    int receivedLength = sock.Receive(receiverBuff);

                    string response = Encoding.Default.GetString(receiverBuff, 0, receivedLength);
                    Console.WriteLine($"받은 명령:\n {response}");

                    // 소켓 닫기.
                    sock.Close();
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"예외 발생: {ex.Message}");
            }

            return 0;
        }

        #endregion
    }
}
