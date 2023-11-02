﻿using CliNet.Interfaces;
using CommandLine;
using Common.Tools;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CliNet.Cores.Commands
{
    [Verb("change.update", HelpText = "업데이트 모드로 변경합니다.")]
    internal class ChangeUpdateCommand : IAction
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

        public int Action()
        {
            string message;

            try
            {
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ServerIpAddress), Port);
                    sock.Connect(endPoint);

                    Console.WriteLine("명령을 전송합니다.");

                    // S 전송.
                    sock.Send(Encoding.UTF8.GetBytes("S"), SocketFlags.None);

                    byte[] receiverBuff = new byte[128];
                    int receivedLength = sock.Receive(receiverBuff);

                    string result = Encoding.Default.GetString(receiverBuff, 0, receivedLength);
                    message = result.Equals("R") ? "업데이트 모드로 변경을 성공했습니다." : $"실패: {result}";

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