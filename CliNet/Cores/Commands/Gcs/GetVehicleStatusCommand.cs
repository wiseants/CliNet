using CliNet.Interfaces;
using CliNet.Models.Commands.AiModule;
using CommandLine;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;

namespace CliNet.Cores.Commands.Gcs
{
    [Verb("get.status", HelpText = "기체 정보 요청.")]
    internal class GetVehicleStatusCommand : IAction
    {
        #region Fields

        private readonly int BUFFER_SIZE = 1024;

        #endregion

        #region Constructors

        public GetVehicleStatusCommand() 
        {
        }

        #endregion

        #region Properties

        public bool IsValid => true;

        [Option('i', "ip", Required = false, HelpText = "서버 IP 주소.")]
        public string IpAddress
        {
            get;
            set;
        } = "127.0.0.1";

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

        #endregion

        #region Public methods

        public int Action()
        {
            try
            {
                using (TcpClient client = new TcpClient(IpAddress, Port))
                {
                    RequestAndListen(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예외 발생: {ex.Message}");
            }

            return 0;
        }

        #endregion

        #region Private methods

        private void RequestAndListen(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                GetVehicleStatusInfo requestInfo = new GetVehicleStatusInfo();

                string requestString = JsonConvert.SerializeObject(requestInfo);
                Console.WriteLine($"서버로 보내는 요청:\n {requestString}");

                byte[] sendBuffer = Encoding.Default.GetBytes(requestString);
                stream.Write(sendBuffer, 0, sendBuffer.Length);

                byte[] receivedBuffer = new byte[BUFFER_SIZE];
                int receivedLength = stream.Read(receivedBuffer, 0, receivedBuffer.Length);

                string responseString = Encoding.Default.GetString(receivedBuffer, 0, receivedLength);
                Console.WriteLine($"서버로부터 받은 응답:\n {responseString}");
            }
        }

        #endregion
    }
}
