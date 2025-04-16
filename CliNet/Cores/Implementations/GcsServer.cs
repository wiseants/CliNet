using CliNet.Cores.Services;
using CliNet.Models.Commands;
using Common;
using Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CliNet.Cores.Implementations
{
    public class GcsServer : IThreadable
    {
        #region Events

        public event Action<int> Finished;
        public event Func<object, object> Request;

        #endregion

        #region Fields

        private readonly int BUFFER_SIZE = 1024;

        private readonly Thread _thread;
        private CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Constructors

        public GcsServer() 
        {
            _thread = new Thread(ThreadProc);
        }

        #endregion

        #region Properties

        public int Port
        {
            get;
            set;
        }

        #endregion

        #region Public methods

        public void Start()
        {
            if (_cancellationTokenSource != null)
            {
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();

            _thread.Start();
        }

        public void Stop()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.CancelAfter(1000);
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }

        #endregion

        #region Private methods

        private void ThreadProc()
        {
            CancellationToken token = _cancellationTokenSource.Token;

            try
            {
                TcpListener listener = new TcpListener(IPAddress.Any, Port);
                listener.Start();

                token.Register(listener.Stop);

                while (token.IsCancellationRequested == false)
                {
                    using(TcpClient client = listener.AcceptTcpClient())
                    {
                        token.Register(client.Close);

                        ListenAndResponse(client);
                    }
                }
            }
            catch (SocketException) { }
            catch (Exception ex)
            {
                Console.WriteLine($"예외 발생: {ex.Message}");
            }

            Finished?.Invoke(0);
        }

        private object ParsePacket(string buffer)
        {
            object receivedObject = null;

            try
            {
                PacketInfo receivedPacket = JsonConvert.DeserializeObject<PacketInfo>(buffer);
                if (receivedPacket != null)
                {
                    if (ContainerService.Instance.TryResolveType<PacketInfo>(receivedPacket.Name, out Type type))
                    {
                        receivedObject = JsonConvert.DeserializeObject(buffer, type);
                    }
                }
            }
            catch 
            {
                receivedObject = null;
            }

            return receivedObject;
        }

        private void ListenAndResponse(TcpClient client)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            Stopwatch stopwatch = new Stopwatch();
            Task.Run(() => 
            {
                while (stopwatch.IsRunning)
                {
                    if (stopwatch.ElapsedMilliseconds > 2000)
                    {
                        cancellationTokenSource.Cancel();
                    }

                    Thread.Sleep(100);
                }
            });

            using (NetworkStream stream = client.GetStream())
            {
                token.Register(stream.Close);

                if (stream.CanRead == false)
                {
                    return;
                }

                stopwatch.Start();

                try
                {
                    while (token.IsCancellationRequested == false)
                    {
                        byte[] receivedBuffer = new byte[BUFFER_SIZE];
                        int receivedLength = stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                        if (receivedLength > 0)
                        {
                            stopwatch.Restart();
                        }

                        string requestString = Encoding.Default.GetString(receivedBuffer, 0, receivedLength);
                        Console.WriteLine($"클라이언트로부터 받은 요청:\n {requestString}");

                        object response = null;

                        object request = ParsePacket(requestString);
                        if (request != null)
                        {
                            response = (Request?.Invoke(request)) ?? new ResponsePacketInfo()
                            {
                                ResultCode = RequestResult.InvalidRequest
                            };
                        }
                        else
                        {
                            response = new ResponsePacketInfo()
                            {
                                ResultCode = RequestResult.ParsingError
                            };
                        }

                        string responseString = JsonConvert.SerializeObject(response);
                        Console.WriteLine($"클라이언트로 보내는 응답:\n {responseString}");

                        byte[] sendBuffer = Encoding.Default.GetBytes(responseString);
                        stream.Write(sendBuffer, 0, sendBuffer.Length);

                        Thread.Sleep(100);
                    }
                }
                catch (IOException) { }
                catch (Exception ex)
                {
                    Console.WriteLine($"예외 발생: {ex.Message}");
                }
            }

            stopwatch.Stop();
        }

        #endregion
    }
}
