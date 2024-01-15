using Common.Interfaces;
using System;
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
                using (UdpClient udpClient = new UdpClient(Port))
                {
                    udpClient.Client.SendTimeout = TIMEOUT_MS;
                    udpClient.Client.ReceiveTimeout = TIMEOUT_MS;

                    _ = udpClient.ReceiveAsync().ContinueWith(x =>
                    {
                        if (x == null)
                        {
                            NLog.LogManager.GetCurrentClassLogger().Error("받은 결과 비었음.");
                            return;
                        }

                        if (x.IsFaulted)
                        {
                            NLog.LogManager.GetCurrentClassLogger().Error("받은 결과 문제 발생.");
                            return;
                        }

                        Console.WriteLine(Encoding.Default.GetString(x.Result.Buffer));

                        byte[] sendBuffer = new byte[x.Result.Buffer.Length];
                        Array.Copy(x.Result.Buffer, sendBuffer, 0);

                        int sentByteNumber = udpClient.Send(sendBuffer, sendBuffer.Length, x.Result.RemoteEndPoint);

                        Console.WriteLine($"받은 버퍼 길이({x.Result.Buffer.Length}), 보낸 길이({sendBuffer.Length})");
                    });

                    while (true)
                    {
                        Thread.Sleep(SEND_INTERVAL_MS);
                    }
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

        #endregion

        #region Private methods

        #endregion
    }
}
