using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;

namespace CliNet.Cores.Commands.Gcs
{
    [Verb("stop.gcs.server", HelpText = "노바코스 인터페이스 GCS 통신 서버 종료.")]
    internal class StopGcsServerCommand : IAction
    {
        #region Constructors

        public StopGcsServerCommand() 
        {
        }

        #endregion

        #region Properties

        public bool IsValid => true;

        #endregion

        #region Public methods

        public int Action()
        {
            ThreadManager.Instance.Remove(StartGcsServerCommand.SERVER_NAME);

            return 0;
        }

        #endregion
    }
}
