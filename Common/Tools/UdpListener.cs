// https://www.csharpstudy.com/net/article/12

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Common.Tools
{
    /// <summary>
    /// UDP 멀티캐스트 리스너 클래스.
    /// </summary>
    public class UdpListener : IDisposable
    {
        #region Events

        /// <summary>
        /// 데이터 받기 이벤트.
        /// </summary>
        public ReceivedHandler Received;

        #endregion

        #region Fields

        private Thread _listenThread;

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

            _listenThread = new Thread(ListenProc);
            _listenThread.Start(new Tuple<string, int>(ipAddress, portNo));
        }

        /// <summary>
        /// 리스너 중지.
        /// </summary>
        public void Stop()
        {
            if (_listenThread != null)
            {
                _listenThread.Abort();
                _listenThread = null;
            }
        }

        #endregion

        #region Event handlers

        private void ListenProc(object param)
        {
            if (param is Tuple<string, int> == false)
            {
                return;
            }

            Tuple<string, int> paramTuple = param as Tuple<string, int>;

            using (UdpClient udp = new UdpClient())
            {
                IPEndPoint localEP = new IPEndPoint(IPAddress.Any, paramTuple.Item2);
                udp.Client.Bind(localEP);

                IPAddress multicastIP = IPAddress.Parse(paramTuple.Item1);
                udp.JoinMulticastGroup(multicastIP);

                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

                while (true)
                {
                    byte[] buff = udp.Receive(ref remoteEP);
                    if (buff != null && buff.Length > 0)
                    {
                        Received?.Invoke(this, buff);
                    }
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