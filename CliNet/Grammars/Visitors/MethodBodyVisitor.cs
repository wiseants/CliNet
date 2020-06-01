using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliNet.Grammars.Visitors
{
    public class MethodBodyVisitor : AbstractParseTreeVisitor<bool>
    {
        public MethodBodyVisitor()
        {
        }

        public override bool Visit([NotNull] IParseTree tree)
        {
            return true;
        }
    }
}
