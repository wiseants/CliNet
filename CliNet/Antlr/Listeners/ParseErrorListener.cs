using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliNet.Antrl.Listeners.Listeners
{
    public class ParseErrorListener : IAntlrErrorListener<object>
    {
        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] object offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            Console.WriteLine(msg);
        }
    }
}
