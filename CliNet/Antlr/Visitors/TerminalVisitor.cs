using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliNet.Antrl.Visitors
{
    public class TerminalVisitor : AbstractParseTreeVisitor<string>
    {
        public override string VisitTerminal([NotNull] ITerminalNode node)
        {
            return node.GetText();
        }
    }
}
