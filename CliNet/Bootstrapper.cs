using Antlr4.Runtime;
using CliNet.Antlr.Grammars.CPP14;
using CliNet.Antlr.Grammars.CSharp;
using System;
using Unity;
using Unity.Resolution;

namespace CliNet
{
    public class Bootstrapper
    {
        #region Fileds

        private static Bootstrapper instance = null;
        private IUnityContainer container = new UnityContainer();

        #endregion

        #region Constructors

        private Bootstrapper()
        {
            BuildContainer();
        }

        #endregion

        #region Properties

        public static Bootstrapper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Bootstrapper();
                }

                return instance;
            }
        }

        #endregion

        #region Public methods

        public T CreateContainer<T>(params ResolverOverride[] overrides)
        {
            T result = default(T);

            try
            {
                result = container.Resolve<T>(overrides);
            }
            catch (Exception)
            {

            }

            return result;
        }

        public T CreateContainer<T>(string name, params ResolverOverride[] overrides)
        {
            T result = default(T);

            try
            {
                result = container.Resolve<T>(name, overrides);
            }
            catch(Exception)
            {

            }

            return result;
        }

        #endregion

        #region Private methods

        private void BuildContainer()
        {
            //container.RegisterType<Lexer, ExprLexer>("Expr");
            //container.RegisterType<Lexer, CPP14Lexer>("CPP14");
            container.RegisterType<Lexer, CSharpLexer>("CSharp");
            //container.RegisterType<Lexer, CPP14Lexer>();

            //container.RegisterType<Parser, ExprParser>("Expr");
            //container.RegisterType<Parser, CPP14Parser>("CPP14");
            container.RegisterType<Parser, CSharpParser>("CSharp");
            //container.RegisterType<Parser, CPP14Parser>();
        }

        #endregion
    }
}
