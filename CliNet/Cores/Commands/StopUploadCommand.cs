using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;

namespace CliNet.Cores.Commands
{
    [Verb("stop.upload", HelpText = "업로드를 중지합니다.")]
    internal class StopUploadCommand : IAction
    {
        public bool IsValid => true;

        public int Action()
        {
            ThreadManager.Instance.Remove("upload");

            return 0;
        }
    }
}
