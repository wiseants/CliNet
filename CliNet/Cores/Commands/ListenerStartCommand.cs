using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;
using Common.Tools;
using System;
using System.Text;

namespace CliNet.Cores.Commands
{
    [Verb("start", HelpText = "Start to Multicast UPD Listen.")]
    public class ListenerStartCommand : IAction
    {
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

        public int Action()
        {
            UdpListener lister = new UdpListener()
            {
                IpAddress = SenderIpAddress,
                PortNo = SenderPortNo
            };
            lister.Received += (sender, buffer) =>
            {
                string data = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                Console.WriteLine(data);
            };

            ThreadManager.Instance.Add(Key, lister);

            return 0;
        }
    }
}
