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
    [Verb("sendto", HelpText = "카메라의 영상 스트림 받기 명령.")]
    internal class SendToCommand : Interfaces.IAction
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

        [Option('r', "trip.address", Required = true, HelpText = "요청을 보내는 카메라 트립 IP 주소.")]
        public string TripIpAddress
        {
            get;
            set;
        } = IPAddressTool.LocalIpAddress;

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

                    SendToRequestInfo requestInfo = new SendToRequestInfo()
                    {
                        SeqNo = SequenceManager.Instance.GetNext(),
                        TripIpAddress = TripIpAddress
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
