using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Common.Tools
{
    public class UdpListener
    {
        #region Events

        public ReceivedHandler Received;

        #endregion

        #region Fields

        private readonly Thread _listenThread;

        #endregion

        #region Constructors

        public UdpListener()
        {
            _listenThread = new Thread(ListenProc);
        }

        #endregion

        #region Properties

        public bool IsRunning
        {
            get;
            private set;
        }

        public string IpAddress
        {
            get;
            set;
        }

        public int PortNo
        {
            get;
            set;
        }

        #endregion

        #region Public methods

        public void Start()
        {
            Start(IpAddress, PortNo);
        }

        public void Start(string ipAddress, int portNo)
        {
            _listenThread.Start(new Tuple<string, int>(ipAddress, portNo));
        }

        public void Stop()
        {
            IsRunning = false;
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
                // (2) UDP 로컬 IP/포트에 바인딩            
                // udp.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
                IPEndPoint localEP = new IPEndPoint(IPAddress.Any, paramTuple.Item2);
                udp.Client.Bind(localEP);

                // (3) Multicast 그룹에 Join
                IPAddress multicastIP = IPAddress.Parse(paramTuple.Item1);
                udp.JoinMulticastGroup(multicastIP);

                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

                IsRunning = true;
                while (IsRunning)
                {
                    // (4) Multicast 수신
                    byte[] buff = udp.Receive(ref remoteEP);
                    if (buff != null && buff.Length > 0)
                    {
                        Received?.Invoke(this, buff);
                    }
                }
            }
        }

        #endregion
    }
}