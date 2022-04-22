using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;

namespace CliNet.Cores.Commands
{
    [Verb("stop", HelpText = "Stop to Multicast UPD Listen.")]
    public class ListenerStopCommand : IAction
    {
        public bool IsValid => true;

        [Option('i', "identifier", Required = false, HelpText = "Listener Identifier")]
        public string Key
        {
            get;
            set;
        } = "default";

        public int Action()
        {
            ThreadManager.Instance.Remove(Key);

            return 0;
        }
    }
}
