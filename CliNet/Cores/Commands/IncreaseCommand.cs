using CliNet.Interfaces;
using CommandLine;
using Common.Tools;
using System;

namespace CliNetCore.Cores.Commands
{
    [Verb("increase", HelpText = "Increase number")]
    public class IncreaseCommand : IAction
    {
        public bool IsValid => true;

        [Option('p', "port", Required = false, HelpText = "Service port number.")]
        public int Port
        {
            get;
            set;
        } = 8055;

        [Option('t', "target", Required = true, HelpText = "target number.")]
        public int TargetNumber
        {
            get;
            set;
        }

        public int Action()
        {
            // Incr RPC 메소드 호출.
            int incrResult = RpcTool.LocalCommand<int>(Port, "Incr", TargetNumber);

            Console.WriteLine(string.Format("{0}", incrResult));

            // Decr RPC 메소드 호출.
            int decrResult = RpcTool.LocalCommand<int>(Port, "Decr", TargetNumber);

            Console.WriteLine(string.Format("{0}", decrResult));

            return 0;
        }
    }
}
