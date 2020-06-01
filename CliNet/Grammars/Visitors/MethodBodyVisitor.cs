using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using CliNet.Expr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CliNet.Expr.ExprParser;

namespace CliNet.Grammars.Visitors
{
    public class MethodBodyVisitor : AbstractParseTreeVisitor<bool>
    {
        public MethodBodyVisitor()
        {
        }

        public override bool Visit([NotNull] IParseTree tree)
        {
            if (tree is ExprContext expr == false)
            {
                return false;
            }

            return true;
        }
    }
}
