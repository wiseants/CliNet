using Common.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CliNet.Cores.Implementations
{
    public class TrackerModuleServer : IThreadable
    {
        #region Events

        public event Action<int> Finished;

        #endregion

        #region Fields

        public static readonly int TIMEOUT_MS = 500;
        public static readonly int SEND_INTERVAL_MS = 250;

        private readonly Thread _thread;
        private UdpClient _udpClient;

        #endregion

        #region Constructors

        public TrackerModuleServer() 
        {
            _thread = new Thread(ThreadProc);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 서버 포트 번호.
        /// </summary>
        public int Port
        {
            get;
            set;
        }

        #endregion

        #region Public methods

        public void Start()
        {
            _thread.Start();
        }

        public void Stop()
        {
            _thread.Abort();

            Thread.Sleep(TIMEOUT_MS);
        }

        #endregion

        #region Event handlers

        private void ThreadProc()
        {
            try
            {
                _udpClient = new UdpClient(Port);

                _udpClient.Client.SendTimeout = TIMEOUT_MS;
                _udpClient.Client.ReceiveTimeout = TIMEOUT_MS;

                _ = _udpClient.BeginReceive(new AsyncCallback(ReceivedProc), null);

                while (true)
                {
                    Thread.Sleep(SEND_INTERVAL_MS);
                }
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine($"서버를 종료합니다.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예외 발생: {ex.Message}");
            }

            Finished?.Invoke(0);
        }

        private void ReceivedProc(IAsyncResult res)
        {
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 8000);
            byte[] buffer = _udpClient.EndReceive(res, ref remoteIpEndPoint);

            Console.WriteLine(Encoding.Default.GetString(buffer));

            byte[] sendBuffer = new byte[buffer.Length];
            Array.Copy(buffer, sendBuffer, 0);

            int sentByteNumber = _udpClient.Send(sendBuffer, sendBuffer.Length, remoteIpEndPoint);

            Console.WriteLine($"받은 버퍼 길이({buffer.Length}), 보낸 길이({sentByteNumber})");

            _ = _udpClient.BeginReceive(new AsyncCallback(ReceivedProc), null);
        }

        #endregion

        #region Private methods

        #endregion
    }
}
