using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;
using Common.Tools;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace CliNet.Cores.Commands
{
    [Verb("udpbypass", HelpText = "Start to UPD bypass.")]
    public class UdpBypassCommand : IAction
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

        [Option('d', "destination port", Required = false, HelpText = "Destination Port number.")]
        public int DestinationPortNo
        {
            get;
            set;
        } = 1098;

        [Option("stop", Required = false, HelpText = "Stop UDP listener.")]
        public bool IsStop
        {
            get;
            set;
        } = false;

        #endregion

        #region Public methods

        public int Action()
        {
            if (IsStop)
            {
                ThreadManager.Instance.Remove(Key);

                Console.WriteLine("Stop a UDP-Bypass.");

                return 0;
            }

            UdpListener listner = new UdpListener()
            {
                IpAddress = SenderIpAddress,
                PortNo = SenderPortNo
            };
            listner.Received += (sender, buffer) =>
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), DestinationPortNo);
                    socket.Connect(endPoint);

                    bool isResult = socket.SendTo(buffer, endPoint) > 0;

                    socket.Close();
                }

                string message = string.Join("|", new List<byte>(buffer).Select(x => string.Format("{0}", x.ToString("X2"))));
                if (string.IsNullOrEmpty(message) == false)
                {
                    LogManager.GetCurrentClassLogger().Info("[{0}]{1}", Key, message);
                }
            };

            ThreadManager.Instance.Add(Key, listner);

            Console.WriteLine("Start a UDP-Bypass.");

            return 0;
        }

        #endregion
    }
}
