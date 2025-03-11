using CliNet.Cores.Services;
using CliNet.Models.Commands;
using Common.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

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

        public string IpAddress
        {
            get;
            set;
        }

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
                TcpListener listener = new TcpListener(IPAddress.Parse(IpAddress), Port);
                listener.Start();

                token.Register(listener.Stop);

                while (token.IsCancellationRequested == false)
                {
                    using(TcpClient client = listener.AcceptTcpClient())
                    {
                        token.Register(client.Close);

                        ListenAndResponse(client, token);
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

            PacketInfo receivedPacket = JsonConvert.DeserializeObject<PacketInfo>(buffer);
            if (receivedPacket != null)
            {
                if (ContainerService.Instance.TryResolveType<PacketInfo>(receivedPacket.Name, out Type type))
                {
                    receivedObject = JsonConvert.DeserializeObject(buffer, type);
                }
            }

            return receivedObject;
        }

        private void ListenAndResponse(TcpClient client, CancellationToken token)
        {
            using (NetworkStream stream = client.GetStream())
            {
                token.Register(stream.Close);

                if (stream.CanRead == false)
                {
                    return;
                }

                byte[] receivedBuffer = new byte[BUFFER_SIZE];
                int receivedLength = stream.Read(receivedBuffer, 0, receivedBuffer.Length);

                string requestString = Encoding.Default.GetString(receivedBuffer, 0, receivedLength);
                Console.WriteLine($"클라이언트로부터 받은 요청:\n {requestString}");

                object request = ParsePacket(requestString);
                if (request != null)
                {
                    object response = Request?.Invoke(request);
                    if (response != null)
                    {
                        string responseString = JsonConvert.SerializeObject(response);
                        Console.WriteLine($"클라이언트로 보내는 응답:\n {responseString}");

                        byte[] sendBuffer = Encoding.Default.GetBytes(responseString);
                        stream.Write(sendBuffer, 0, sendBuffer.Length);
                    }
                }
            }
        }

        #endregion
    }
}
