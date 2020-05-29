using Antlr4.Runtime;
using CliNet.Interfaces;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CliNet.CSharpLexer;

namespace CliNet.Cores.Commands
{
    [Verb("parse", HelpText = "Parse to C# code.")]
    public class ParseCommand : IAction
    {
        public bool IsValid => true;

        /// <summary>
        /// 필수 옵션.
        /// </summary>
        [Option('f', "file", Required = true, HelpText = "file-name.")]
        public string FileFullPath
        {
            get;
            set;
        }

        public int Action()
        {
            var target = new AntlrInputStream(FileFullPath);
            var lexer = new CSharpLexer.CSharpLexer(target);
            var tokens = new CommonTokenStream(lexer);
            CSharpParser.CSharpParser parser = new CSharpParser.CSharpParser(tokens) { BuildParseTree = true };

            CSharpParser.CSharpParser.LiteralContext result = parser.literal();

            Console.WriteLine(result.ToStringTree());

            return 1;
        }
    }
}
