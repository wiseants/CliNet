using Antlr4.Runtime;
using CliNet.CSharp;
using CliNet.Grammars.Listeners;
using CliNet.Interfaces;
using CommandLine;
using System;

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
            var target = new AntlrInputStream(@FileFullPath);
            var lexer = new CSharpLexer(target);
            var tokens = new CommonTokenStream(lexer);
            CSharpParser parser = new CSharpParser(tokens) { BuildParseTree = true };

            CSharpParserListener listener = new CSharpParserListener();

            parser.AddParseListener(listener);

            CSharpParser.LiteralContext result = parser.literal();

            Console.WriteLine(result.ToStringTree());

            return 1;
        }
    }
}
