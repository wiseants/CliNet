using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CliNet.Models.Commands;
using CommandLine;
using Common.Tools;
using Newtonsoft.Json;
using NLog.Targets;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime;
using System.Text;

namespace CliNet.Cores.Commands
{
    [Verb("set.enable", HelpText = "활성화/비활성화 명령을 보냅니다.")]
    internal class SetEnableCommand : IAction
    {
        #region Properties

        public bool IsValid => true;

        [Option('a', "address", Required = false, HelpText = "서버 IP 주소.")]
        public string ServerIpAddress
        {
            get;
            set;
        } = IPAddressTool.LocalIpAddress;

        [Option('p', "port", Required = false, HelpText = "서버 포트 번호.")]
        public int Port
        {
            get;
            set;
        } = 15300;

        [Option('e', "enable", Required = true, HelpText = "0:비활성화, 1:활성화")]
        public int IsEnable
        {
            get;
            set;
        }

        #endregion

        #region Public methods

        public int Action()
        {
            string message;

            try
            {
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ServerIpAddress), Port);
                    sock.Connect(endPoint);

                    Console.WriteLine(string.Format("{0} 명령을 전송합니다.", IsEnable == 0 ? "비활성화" : "활성화"));

                    SetEnableRequestInfo requestInfo = new SetEnableRequestInfo()
                    {
                        SeqNo = SequenceManager.Instance.GetNext(),
                        IsEnable = IsEnable == 1
                    };

                    string writeString = JsonConvert.SerializeObject(requestInfo);
                    Console.WriteLine(writeString);

                    sock.Send(Encoding.ASCII.GetBytes(writeString), SocketFlags.None);

                    //byte[] receiverBuff = new byte[128];
                    //int receivedLength = sock.Receive(receiverBuff);

                    //string result = Encoding.Default.GetString(receiverBuff, 0, receivedLength);
                    //Console.WriteLine($"<< {result}");

                    // 소켓 닫기.
                    sock.Close();
                }
            }
            catch (Exception ex) 
            {
                message = $"예외 발생: {ex.Message}";
            }

            return 0;
        }

        #endregion
    }
}
