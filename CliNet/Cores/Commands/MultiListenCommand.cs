using CliNet.Interfaces;
using CommandLine;
using Common.Tools;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CliNet.Cores.Commands
{
    [Verb("listen", HelpText = "Listen to Multicast UPD.")]
    public class MultiListenCommand : IAction
    {
        public bool IsValid => true;

        [Option('i', "ip", Required = false, HelpText = "Source IP address.")]
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

            lister.Start();

            return 0;
        }
    }
}
