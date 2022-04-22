using CliNet.Interfaces;
using CommandLine;
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
            // (1) UdpClient 객체 성성
            UdpClient udp = new UdpClient();

            // (2) UDP 로컬 IP/포트에 바인딩            
            // udp.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, SenderPortNo);
            udp.Client.Bind(localEP);

            // (3) Multicast 그룹에 Join
            IPAddress multicastIP = IPAddress.Parse(SenderIpAddress);
            udp.JoinMulticastGroup(multicastIP);

            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

            while (!Console.KeyAvailable)
            {
                // (4) Multicast 수신
                byte[] buff = udp.Receive(ref remoteEP);

                string data = Encoding.UTF8.GetString(buff, 0, buff.Length);
                Console.WriteLine(data);
            }

            return 0;
        }
    }
}
