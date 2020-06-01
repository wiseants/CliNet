using Antlr4.Runtime;
using CliNet.CSharp;
using CliNet.CPP14;
using CliNet.Grammars.Listeners;
using CliNet.Grammars.Visitors;
using CliNet.Interfaces;
using CommandLine;
using System;
using System.IO;
using CliNet.Expr;

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
        public string @FileFullPath
        {
            get;
            set;
        }

        public int Action()
        {
            var target = new AntlrInputStream(File.ReadAllText(@FileFullPath));
            var lexer = new ExprLexer(target);
            var tokens = new CommonTokenStream(lexer);

            ExprParser parser = new ExprParser(tokens) { BuildParseTree = true };
            parser.AddErrorListener(new ParseErrorListener());

            MethodBodyVisitor visitor = new MethodBodyVisitor();
            visitor.Visit(parser.expr());


            Console.WriteLine(parser.prog().ToStringTree());

            return 1;
        }
    }
}
