using CliNet.Cores.Implementations;
using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;
using Common.Tools;

namespace CliNet.Cores.Commands
{
    [Verb("start.upload", HelpText = "업로드를 시작합니다.")]
    internal class StartUploadCommand : IAction
    {
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
        } = 30251;

        [Option('f', "file", Required = false, HelpText = "업로드할 파일 전체 경로.")]
        public string FileFullPath
        {
            get;
            set;
        } = @"D:\test_app_2.bin";

        [Option('b', "block", Required = false, HelpText = "업로드시 한번에 전송하는 블럭 사이즈.")]
        public int BlockSize
        {
            get;
            set;
        } = 1024;

        public int Action()
        {
            PrintServer server = new PrintServer
            {
                ServerIpAddress = ServerIpAddress,
                Port = Port,
                FileFullPath = FileFullPath,
                BlockSize = BlockSize
            };

            ThreadManager.Instance.Add("upload", server);

            return 0;
        }
    }
}
