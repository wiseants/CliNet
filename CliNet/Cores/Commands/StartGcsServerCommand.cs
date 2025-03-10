using CliNet.Cores.Implementations;
using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;
using System;

namespace CliNet.Cores.Commands
{
    [Verb("start.gcs.server", HelpText = "노바코스 인터페이스 GCS 통신 서버 시작.")]
    internal class StartGcsServerCommand : IAction
    {
        #region Fields

        public static readonly string SERVER_NAME = "GCS 데이터서버";

        #endregion

        #region Constructors

        public StartGcsServerCommand() 
        {
        }

        #endregion

        #region Properties

        public bool IsValid => true;

        [Option('i', "ip", Required = false, HelpText = "로컬 서버 IP 주소.")]
        public string IpAddress
        {
            get;
            set;
        } = "127.0.0.1";

        [Option('p', "port", Required = false, HelpText = "서버 포트 번호.")]
        public int Port
        {
            get;
            set;
        } = 15300;

        #endregion

        #region Public methods

        public int Action()
        {
            if (ThreadManager.Instance.IsExist(SERVER_NAME))
            {
                Console.WriteLine($"[{SERVER_NAME}] 서버가 이미 동작중입니다.");
                return 0;
            }

            GcsServer server = new GcsServer
            {
                IpAddress = IpAddress,
                Port = Port,
            };
            server.ReceivedData += (x) => Console.WriteLine(x);

            ThreadManager.Instance.Add(SERVER_NAME, server);

            return 0;
        }

        #endregion
    }
}
