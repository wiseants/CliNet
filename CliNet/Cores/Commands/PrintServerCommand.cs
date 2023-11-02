using AustinHarris.JsonRpc;
using CliNet.Interfaces;
using CliNet.Models;
using CommandLine;
using Common.Network;
using Elasticsearch.Net;
using MiscUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CliNet.Cores.Commands
{
    [Verb("print", HelpText = "입력되는 문자열을 출력하는 서버 시작.")]
    internal class PrintServerCommand : IAction
    {
        private Socket _sock;

        public bool IsValid => true;

        [Option('p', "port", Required = false, HelpText = "서버 포트 번호.")]
        public int Port
        {
            get;
            set;
        } = 30251;

        public int Action()
        {
            try
            {
                _sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint serverEP = new IPEndPoint(IPAddress.Any, Port);

                _sock.Bind(serverEP);
                _sock.Listen(10);
                _sock.BeginAccept(AcceptCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = _sock.EndAccept(ar);
                AsyncObject obj = new AsyncObject(1920 * 1080 * 3);
                obj.WorkingSocket = client;
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

                Console.WriteLine(command);

                // R 전송.
                asyncObject.WorkingSocket.Send(Encoding.UTF8.GetBytes("R"), SocketFlags.None);
            }
        }
    }
}
