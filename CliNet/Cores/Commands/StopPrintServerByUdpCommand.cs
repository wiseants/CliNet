using CliNet.Cores.Managers;
using CommandLine;

namespace CliNet.Cores.Commands
{
    [Verb("stop.print.udp", HelpText = "JSON 프린트 서버 종료.")]
    internal class StopPrintByServerCommand : Interfaces.IAction
    {
        #region Constructors

        public StopPrintByServerCommand() 
        {
        }

        #endregion

        #region Properties

        public bool IsValid => true;

        #endregion

        #region Public methods

        public int Action()
        {
            ThreadManager.Instance.Remove("print.udp");

            return 0;
        }

        #endregion
    }
}
