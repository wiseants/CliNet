using CliNet.Cores.Implementations;
using CliNet.Cores.Managers;
using CommandLine;
using System;

namespace CliNet.Cores.Commands
{
    [Verb("start.print", HelpText = "테스트를 위한 JSON 프린트 서버 시작.")]
    internal class StartPrintServerCommand : Interfaces.IAction
    {
        #region Constructors

        public StartPrintServerCommand() 
        {
        }

        #endregion

        #region Properties

        public bool IsValid => true;

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
            if (ThreadManager.Instance.IsExist("print"))
            {
                Console.WriteLine("서버가 이미 동작중입니다.");
                return 0;
            }

            AiModuleServer server = new AiModuleServer
            {
                Port = Port,
            };

            ThreadManager.Instance.Add("print", server);

            return 0;
        }

        #endregion
    }
}
