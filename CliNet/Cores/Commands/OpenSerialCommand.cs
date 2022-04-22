using CliNet.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
