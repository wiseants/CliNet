using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;

namespace CliNet.Cores.Commands
{
    [Verb("stop", HelpText = "Stop to Multicast UPD Listen.")]
    public class ListenerStopCommand : IAction
    {
        #region Properties

        public bool IsValid => true;

        [Option('i', "identifier", Required = false, HelpText = "Listener Identifier")]
        public string Key
        {
            get;
            set;
        } = "default";

        #endregion

        #region Public methods

        public int Action()
        {
            ThreadManager.Instance.Remove(Key);

            return 0;
        }

        #endregion
    }
}
