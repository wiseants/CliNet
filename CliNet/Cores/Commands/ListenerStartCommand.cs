using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;
using Common.Tools;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CliNet.Cores.Commands
{
    [Verb("start", HelpText = "Start to Multicast UPD Listen.")]
    public class ListenerStartCommand : IAction
    {
        #region Properties

        public bool IsValid => true;

        [Option('i', "identifier", Required = false, HelpText = "Listener Identifier")]
        public string Key
        {
            get;
            set;
        } = "default";

        [Option('a', "address", Required = false, HelpText = "Source IP address.")]
        public string SenderIpAddress
        {
            get;
            set;
        } = "224.11.11.11";

        [Option('p', "port", Required = false, HelpText = "Source Port number.")]
        public int SenderPortNo
        {
            get;
            set;
        } = 1097;

        #endregion

        #region Public methods

        public int Action()
        {
            UdpListener lister = new UdpListener()
            {
                IpAddress = SenderIpAddress,
                PortNo = SenderPortNo
            };
            lister.Received += (sender, buffer) =>
            {
                string message = string.Join("|", new List<byte>(buffer).Select(x => string.Format("{0}", x.ToString("X"))));
                if (string.IsNullOrEmpty(message) == false)
                {
                    LogManager.GetCurrentClassLogger().Info(message);
                }
            };

            ThreadManager.Instance.Add(Key, lister);

            return 0;
        }

        #endregion
    }
}
