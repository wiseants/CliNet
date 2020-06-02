using Antlr4.Runtime;
using CliNet.Grammars.Listeners;
using CliNet.Grammars.Visitors;
using CliNet.Interfaces;
using CommandLine;
using System;
using System.IO;
using Unity.Resolution;

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
            var targetStream = new AntlrInputStream(File.ReadAllText(@FileFullPath));
            var lexer = Bootstrapper.Instance.CreateContainer<Lexer>(new ResolverOverride[]
            {
                new ParameterOverride("input", targetStream)
            });

            var tokens = new CommonTokenStream(lexer);
            var parser = Bootstrapper.Instance.CreateContainer<Antlr4.Runtime.Parser>(new ResolverOverride[]
            {
                new ParameterOverride("input", tokens)
            });
            parser.BuildParseTree = true;
            parser.AddErrorListener(new ParseErrorListener());

            MethodBodyVisitor visitor = new MethodBodyVisitor();
            visitor.Visit(parser.Context);

            Console.WriteLine(parser.Context.ToStringTree());

            return 1;
        }
    }
}
