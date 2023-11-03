// https://www.csharpstudy.com/net/article/12

using Common.Interfaces;
using NLog;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Tools
{
    /// <summary>
    /// UDP 멀티캐스트 리스너 클래스.
    /// </summary>
    public class UdpListener : IThreadable, IDisposable
    {
        #region Events

        /// <summary>
        /// 데이터 받기 이벤트.
        /// </summary>
        public event ReceivedHandler Received;
        public event Action<int> Finished;

        #endregion

        #region Fields

        public static readonly int ABORT_DELAY_MS = 100;
        public static readonly int THREAD_DELAY_MS = 50;

        private Thread _listenThread;
        private CancellationTokenSource _tokenSource;

        #endregion

        #region Constructors

        public UdpListener()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// IP 주소.
        /// </summary>
        public string IpAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 포트번호.
        /// </summary>
        public int PortNo
        {
            get;
            set;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 리스너 시작.
        /// </summary>
        public void Start()
        {
            Start(IpAddress, PortNo);
        }

        /// <summary>
        /// 리스너 시작.
        /// </summary>
        /// <param name="ipAddress">IP 주소.</param>
        /// <param name="portNo">포트번호.</param>
        public void Start(string ipAddress, int portNo)
        {
            Stop();

            if (string.IsNullOrEmpty(ipAddress))
            {
                return;
            }

            _listenThread = new Thread(ListenProcAsync);
            _listenThread.Start(new Tuple<string, int>(ipAddress, portNo));
        }

        /// <summary>
        /// 리스너 중지.
        /// </summary>
        public void Stop()
        {
            if (_listenThread != null)
            {
                _tokenSource.CancelAfter(ABORT_DELAY_MS);
                _tokenSource.Dispose();
                _tokenSource = null;

                _listenThread.Abort();
                Thread.Sleep(ABORT_DELAY_MS);
                _listenThread = null;
            }
        }

        #endregion

        #region Event handlers

        private void ListenProcAsync(object param)
        {
            if (param is Tuple<string, int> == false)
            {
                return;
            }

            Tuple<string, int> paramTuple = param as Tuple<string, int>;

            using (UdpClient udp = new UdpClient())
            {
                try
                {
                    IPEndPoint localEP = new IPEndPoint(IPAddress.Any, paramTuple.Item2);
                    udp.Client.Bind(localEP);

                    IPAddress multicastIP = IPAddress.Parse(paramTuple.Item1);
                    udp.JoinMulticastGroup(multicastIP);

                    _tokenSource = new CancellationTokenSource();

                    ReceivedLoop(udp, _tokenSource.Token);
                }
                catch (ThreadAbortException) { }
                catch (Exception ex)
                {
                    LogManager.GetCurrentClassLogger().Error("Exception occurred. Message({0})", ex.Message);
                }
            }

            Finished.Invoke(0);
        }

        private void ReceivedLoop(UdpClient udp, CancellationToken cancellationToken)
        {
            while (true)
            {
                bool isRecieved = false;
                udp.ReceiveAsync().WithCancellation(cancellationToken).ContinueWith(x =>
                {
                    if (x != null && x.Status != TaskStatus.Faulted)
                    {
                        if (x.Result != null && x.Result.Buffer != null && x.Result.Buffer.Length > 0)
                        {
                            Received?.Invoke(this, x.Result.Buffer);
                        }
                    }

                    isRecieved = true;
                });

                while (isRecieved == false)
                {
                    Thread.Sleep(THREAD_DELAY_MS);
                }
            }
        }

        #endregion

        #region IDisposable implementations

        public void Dispose()
        {
            Stop();
        }

        #endregion

    }
}