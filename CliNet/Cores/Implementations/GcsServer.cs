using CliNet.Cores.Services;
using CliNet.Models.Commands;
using CliNet.Models.Commands.AiModule;
using Common.Interfaces;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public event Action<string> OccuredMessage;

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
                    TcpClient client = listener.AcceptTcpClient();

                    _ = Task.Run(() =>
                    {
                        byte[] buffer = new byte[BUFFER_SIZE];

                        NetworkStream stream = client.GetStream();

                        token.Register(client.Close);

                        if (stream.CanRead)
                        {
                            int receivedLength;
                            string receivedData = string.Empty;

                            while ((receivedLength = stream.Read(buffer, 0, buffer.Length)) != 0 && token.IsCancellationRequested == false)
                            {
                                receivedData = Encoding.ASCII.GetString(buffer, 0, receivedLength);

                                ParsePacket(receivedData);

                                OccuredMessage?.Invoke(receivedData);
                            }
                            client.Close();
                        }

                        return Task.CompletedTask;
                    }, token);
                }
            }
            catch (SocketException) { }
            catch (Exception ex)
            {
                Console.WriteLine($"예외 발생: {ex.Message}");
            }

            Finished?.Invoke(0);
        }

        private void ParsePacket(string buffer)
        {
            PacketInfo receivedPacket = JsonConvert.DeserializeObject<PacketInfo>(buffer);
            if (receivedPacket != null)
            {
                if (ContainerService.Instance.TryResolveType(receivedPacket.Name, out Type type))
                {
                }
            }
        }

        #endregion
    }
}
