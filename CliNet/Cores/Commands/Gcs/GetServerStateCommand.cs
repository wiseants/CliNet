using CliNet.Cores.Implementations;
using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CliNet.Models.Commands.AiModule;
using CommandLine;
using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using Grpc.Core;

namespace CliNet.Cores.Commands.Gcs
{
    [Verb("get.gcs.state", HelpText = "GCS 상태 정보 요청.")]
    internal class GetServerStateCommand : IAction
    {
        #region Fields

        private readonly int BUFFER_SIZE = 1024;

        #endregion

        #region Constructors

        public GetServerStateCommand() 
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
                    NetworkStream stream = client.GetStream();
                    byte[] buffer = new byte[BUFFER_SIZE];

                    GetServerStateInfo requestInfo = new GetServerStateInfo();

                    string request = JsonConvert.SerializeObject(requestInfo);
                    Console.WriteLine($"보낸 명령:\n {request}");

                    byte[] dataArray = Encoding.UTF8.GetBytes(request);
                    stream.Write(dataArray, 0, dataArray.Length);

                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"받은 명령:\n {receivedMessage}");
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
