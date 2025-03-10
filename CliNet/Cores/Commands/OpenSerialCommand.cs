using CliNet.Interfaces;
using CommandLine;
using System;

namespace CliNet.Cores.Commands
{
    [Verb("open", HelpText = "Convert file.")]
    public class OpenSerialCommand// : IAction
    {
        public bool IsValid => false;

        public int Action()
        {
            throw new NotImplementedException();
        }
    }
}
