using CliNet.Interfaces;
using CommandLine;
using Common.Tools;
using System;
using System.Net;

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
            new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port).Command<int>("Incr", TargetNumber).ContinueWith(x =>
            {
                Console.WriteLine(string.Format("{0}", x.Result));
            });

            return 0;
        }
    }
}
