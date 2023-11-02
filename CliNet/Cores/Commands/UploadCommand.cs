using CliNet.Interfaces;
using CommandLine;
using Common.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CliNet.Cores.Commands
{
    [Verb("upload", HelpText = "업로드를 진행합니다.")]
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

        [Option('f', "file", Required = false, HelpText = "업로드할 파일 전체 경로.")]
        public string FileFullPath
        {
            get;
            set;
        } = @"D:\test_app_2.bin";

        [Option('b', "block", Required = false, HelpText = "업로드시 한번에 전송하는 블럭 사이즈.")]
        public int BlockSize
        {
            get;
            set;
        } = 1024;

        public int Action()
        {
            string message;

            try
            {
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ServerIpAddress), Port);
                    sock.Connect(endPoint);

                    Console.WriteLine("업로드 가능 여부를 확인합니다.");

                    // U 전송.
                    sock.Send(Encoding.UTF8.GetBytes("U"), SocketFlags.None);

                    string result;
                    byte[] receiverBuff = new byte[128];
                    // R 받음.
                    int receivedLength = sock.Receive(receiverBuff);

                    result = Encoding.Default.GetString(receiverBuff, 0, receivedLength);
                    message = result.Equals("R") ? "업로드를 시작합니다." : $"실패: {result}";

                    byte[] fileBuffer = File.ReadAllBytes(FileFullPath);
                    var a = fileBuffer.Take(100).ToArray();

                    List<byte[]> bufferCollection = new List<byte[]>();

                    int remainLength = fileBuffer.Length;
                    while(remainLength > 0)
                    {
                        int length = remainLength < BlockSize ? remainLength : BlockSize;
                        bufferCollection.Add(fileBuffer.Take(length).ToArray());

                        remainLength -= length;
                    }

                    foreach (byte[] buffer in bufferCollection)
                    {
                        // 데이터 전송.
                        sock.Send(buffer, SocketFlags.None);

                        // R 받음.
                        receivedLength = sock.Receive(receiverBuff);
                        result = Encoding.Default.GetString(receiverBuff, 0, receivedLength);

                        Console.WriteLine(result.Equals("R") ? $"{buffer.Length}바이트 전송 성공." : $"실패: {result}");
                    }

                    // F 전송.
                    sock.Send(Encoding.UTF8.GetBytes("F"), SocketFlags.None);

                    // 소켓 닫기.
                    sock.Close();
                }
            }
            catch (Exception ex) 
            {
                message = $"예외 발생: {ex.Message}";
            }

            Console.WriteLine(message);

            return 0;
        }
    }
}
