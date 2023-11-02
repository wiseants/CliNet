using CliNet.Interfaces;
using CommandLine;
using Common.Tools;
using MiscUtil;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CliNet.Cores.Commands
{
    [Verb("upload", HelpText = "파일 업로드 시작.")]
    internal class UploadCommand : IAction
    {
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
        } = 30251;

        [Option('f', "file", Required = false, HelpText = "업로드 할 파일 전체 경로.")]
        public string FileFullPath
        {
            get;
            set;
        }

        public int Action()
        {
            using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ServerIpAddress), Port);
                sock.Connect(endPoint);

                Console.WriteLine("업로드를 시작합니다.");

                // S 전송.
                sock.Send(Encoding.UTF8.GetBytes("S"), SocketFlags.None);

                byte[] receiverBuff = new byte[8192];
                int receivedLength = sock.Receive(receiverBuff);

                Console.WriteLine(Encoding.Default.GetString(receiverBuff));

                // (5) 소켓 닫기
                sock.Close();
            }

            return 0;
        }
    }
}
