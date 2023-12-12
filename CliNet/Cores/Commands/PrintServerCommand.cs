using CliNet.Models;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CliNet.Cores.Commands
{
    [Verb("print", HelpText = "트래커 테스트를 위한 프린트 서버 시작.")]
    internal class PrintServerCommand : Interfaces.IAction
    {
        private Socket _sock;
        private Dictionary<string, Action<IAsyncResult>> _commandMap = new Dictionary<string, Action<IAsyncResult>>();

        public PrintServerCommand() 
        {
            _commandMap.Add("S", OnChangeUpdateMode);
            _commandMap.Add("E", OnChangeTrackMode);
            _commandMap.Add("U", OnChangeDownload);
        }

        public bool IsValid => true;

        [Option('p', "port", Required = false, HelpText = "서버 포트 번호.")]
        public int Port
        {
            get;
            set;
        } = 30251;

        public int Action()
        {
            try
            {
                _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, Port);

                _sock.Bind(serverEP);
                _sock.Listen(10);
                _sock.BeginAccept(AcceptCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = _sock.EndAccept(ar);
                AsyncObject obj = new AsyncObject(1920 * 1080 * 3)
                {
                    WorkingSocket = client
                };
                client.BeginReceive(obj.Buffer, 0, 1920 * 1080 * 3, 0, DataReceived, obj);

                _sock.BeginAccept(AcceptCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void DataReceived(IAsyncResult ar)
        {
            if (ar.AsyncState is AsyncObject asyncObject)
            {
                int receivedLength = asyncObject.WorkingSocket.EndReceive(ar);

                byte[] buffer = new byte[receivedLength];

                Array.Copy(asyncObject.Buffer, 0, buffer, 0, receivedLength);

                string command = Encoding.Default.GetString(buffer, 0, receivedLength);

                if (_commandMap.TryGetValue(command, out Action<IAsyncResult> action))
                {
                    action(ar);
                }
                else
                {
                    Console.WriteLine($"잘못된 명령: {command}");
                }
            }
        }

        private void OnChangeUpdateMode(IAsyncResult ar)
        {
            Console.WriteLine($"업데이트 모드로 진입.");

            if (ar.AsyncState is AsyncObject asyncObject)
            {
                // R 전송.
                asyncObject.WorkingSocket.Send(Encoding.UTF8.GetBytes("R"), SocketFlags.None);
            }
        }

        private void OnChangeTrackMode(IAsyncResult ar)
        {
            Console.WriteLine($"트래커 모드로 진입.");

            if (ar.AsyncState is AsyncObject asyncObject)
            {
                // R 전송.
                asyncObject.WorkingSocket.Send(Encoding.UTF8.GetBytes("R"), SocketFlags.None);
            }
        }

        private void OnChangeDownload(IAsyncResult ar)
        {
            Console.WriteLine($"다운로드를 시작합니다.");

            if (ar.AsyncState is AsyncObject asyncObject)
            {
                asyncObject.WorkingSocket.Send(Encoding.UTF8.GetBytes("R"), SocketFlags.None);

                while (true)
                {
                    byte[] receiverBuff = new byte[1920 * 1080 * 3];
                    int receivedLength = asyncObject.WorkingSocket.Receive(receiverBuff);

                    string result = Encoding.Default.GetString(receiverBuff, 0, receivedLength);
                    if (result.Equals("F"))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"데이터 수신: {receivedLength}바이트");

                        asyncObject.WorkingSocket.Send(Encoding.UTF8.GetBytes("R"), SocketFlags.None);
                    }
                }
            }

            Console.WriteLine($"다운로드를 종료합니다.");
        }
    }
}
