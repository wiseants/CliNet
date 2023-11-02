using AustinHarris.JsonRpc;
using CliNet.Interfaces;
using CommandLine;
using Common.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CliNet.Cores.Commands
{
    [Verb("print", HelpText = "입력되는 문자열을 출력하는 서버 시작.")]
    internal class PrintServerCommand : IAction
    {
        public bool IsValid => true;

        [Option('p', "port", Required = false, HelpText = "서버 포트 번호.")]
        public int Port
        {
            get;
            set;
        } = 30251;

        public int Action()
        {
            SocketListener socketListener = new SocketListener(IPAddress.Parse("127.0.0.1"), Port);
            CancellationTokenSource tokenSource = new CancellationTokenSource();

            socketListener.StartAsync((writer, line) =>
            {
                Console.WriteLine(line);
            }, tokenSource.Token);

            return 0;
        }
    }
}
