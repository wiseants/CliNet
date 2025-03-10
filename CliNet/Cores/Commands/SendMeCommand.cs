using CommandLine;
using System;
using System.Net.Sockets;
using System.Text;

namespace CliNet.Cores.Commands
{
    [Verb("sendme", HelpText = "카메라의 영상 스트림 받기 명령.")]
    internal class SendMeCommand : Interfaces.IAction
    {
        #region Fields

        private readonly int BUFFER_SIZE = 1024;

        #endregion

        #region Properties

        public bool IsValid => true;


        [Option('r', "trip.address", Required = false, HelpText = "요청을 보내는 카메라 트립 IP 주소.")]
        public string TripIpAddress
        {
            get;
            set;
        } = "192.168.4.31";

        [Option('p', "port", Required = false, HelpText = "서버 포트 번호.")]
        public int Port
        {
            get;
            set;
        } = 11024;

        [Option('t', "timeout", Required = false, HelpText = "타임아웃 시간(ms)")]
        public int Timeout
        {
            get;
            set;
        } = 2000;


        #endregion

        #region Public methods

        public int Action()
        {
            try
            {
                const int LOCAL_PORT_NO = 10555; // 통신 로컬 포트 번호.
                const int SEND_PORT_NO = 10554; // 보내기 포트 번호.

                UdpClient client = new UdpClient(LOCAL_PORT_NO); // UDP 클라이언트 객체.

                byte[] sendBytes = Encoding.ASCII.GetBytes($"300.300.300.300 {Port}"); // 미리 약속된 스트림.
                client.Send(sendBytes, sendBytes.Length, TripIpAddress, SEND_PORT_NO);
                client.Close();
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
