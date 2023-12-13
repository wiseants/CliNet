using CliNet.Models;
using Common.Interfaces;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CliNet.Cores.Implementations
{
    public class AiModuleServer : IThreadable
    {
        #region Events

        public event Action<int> Finished;

        #endregion

        #region Fields

        private readonly int BUFFER_SIZE = 1024;

        private readonly Thread _thread;
        private Socket _sock;

        #endregion

        #region Constructors

        public AiModuleServer() 
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
            _thread.Start();
        }

        public void Stop()
        {
            _sock?.Close();
            _sock = null;

            _thread.Abort();
        }

        #endregion

        #region Private methods

        private void ThreadProc()
        {
            try
            {
                _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, Port);

                _sock.Bind(serverEP);
                _sock.Listen(10);

                do
                {
                    Socket client = _sock.Accept();

                    AsyncObject obj = new AsyncObject(BUFFER_SIZE)
                    {
                        WorkingSocket = client
                    };
                    client.BeginReceive(obj.Buffer, 0, BUFFER_SIZE, 0, DataReceived, obj);
                }
                while (_sock != null && _sock.Connected);
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

                Console.WriteLine($"받은 명령:\n {command}");
            }
        }

        #endregion
    }
}
