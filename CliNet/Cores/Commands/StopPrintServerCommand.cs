using CliNet.Cores.Managers;
using CommandLine;

namespace CliNet.Cores.Commands
{
    [Verb("stop.print", HelpText = "테스트를 위한 JSON 프린트 서버 시작.")]
    internal class StopPrintServerCommand : Interfaces.IAction
    {
        #region Constructors

        public StopPrintServerCommand() 
        {
        }

        #endregion

        #region Properties

        public bool IsValid => true;

        #endregion

        #region Public methods

        public int Action()
        {
            ThreadManager.Instance.Remove("print");

            return 0;
        }

        #endregion
    }
}
