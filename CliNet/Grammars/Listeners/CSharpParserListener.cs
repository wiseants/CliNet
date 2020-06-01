using Antlr4.Runtime.Misc;
using CliNet.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliNet.Grammars.Listeners
{
    public class CSharpParserListener : CSharpParserBaseListener
    {
        public CSharpParserListener()
        {
        }

        public override void EnterLiteralAccessExpression([NotNull] CSharpParser.LiteralAccessExpressionContext context)
        {
        }

    }
}
