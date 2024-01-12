using CliNet.Interfaces;
using CommandLine;
using System;

namespace CliNet.Cores.Commands
{
    [Verb("open", HelpText = "Convert file.")]
    public class OpenSerialCommand : IAction
    {
        public bool IsValid => throw new NotImplementedException();

        public int Action()
        {
            throw new NotImplementedException();
        }
    }
}
