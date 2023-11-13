using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Common.Tools;
using System.IO;

namespace CliNet.Cores.Implementations
{
    public class PrintServer : IThreadable
    {
        public event Action<int> Finished;

        private readonly Thread _thread;

        public PrintServer() 
        {
            _thread = new Thread(ThreadProc);
        }

        public string ServerIpAddress
        {
            get;
            set;
        }

        public int Port
        {
            get;
            set;
        }

        public string FileFullPath
        {
            get;
            set;
        }

        public int BlockSize
        {
            get;
            set;
        }

        public void Start()
        {
            _thread.Start();
        }

        public void Stop()
        {
            _thread.Abort();
        }

        private void ThreadProc()
        {
            try
            {
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ServerIpAddress), Port);
                    sock.Connect(endPoint);

                    Console.WriteLine("업로드 가능 여부를 확인합니다.");

                    // U 전송.
                    sock.Send(Encoding.UTF8.GetBytes("U"), SocketFlags.None);
                    Console.WriteLine(">> U");

                    string result;
                    byte[] receiverBuff = new byte[128];
                    // R 받음.
                    int receivedLength = sock.Receive(receiverBuff);

                    result = Encoding.Default.GetString(receiverBuff, 0, receivedLength);
                    Console.WriteLine($"<< {result}");

                    if (result.Equals("R"))
                    {
                        Console.WriteLine("업로드를 시작합니다.");

                        byte[] fileBuffer = File.ReadAllBytes(FileFullPath);

                        List<byte[]> bufferCollection = new List<byte[]>();

                        int index = 0;
                        while (index < fileBuffer.Length)
                        {
                            int length = (fileBuffer.Length - index) < BlockSize ? (fileBuffer.Length - index) : BlockSize;

                            bufferCollection.Add(fileBuffer.Skip(index).Take(length).ToArray());

                            index += length;
                        }

                        foreach (byte[] buffer in bufferCollection)
                        {
                            // 데이터 전송.
                            sock.Send(buffer, SocketFlags.None);
                            Console.WriteLine($"데이터 송신: {buffer.Length}바이트");

                            // R 받음.
                            receivedLength = sock.Receive(receiverBuff);
                            result = Encoding.Default.GetString(receiverBuff, 0, receivedLength);
                            Console.WriteLine($"<< {result}");
                        }

                        // F 전송.
                        sock.Send(Encoding.UTF8.GetBytes("F"), SocketFlags.None);
                        Console.WriteLine(">> F");

                        Console.WriteLine("업로드를 종료합니다.");
                    }
                    else
                    {
                        Console.WriteLine($"실패: {result}");
                    }

                    // 소켓 닫기.
                    sock.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예외 발생: {ex.Message}");
            }

            Finished?.Invoke(0);
        }
    }
}
