﻿using CliNet.Cores.Managers;
using CliNet.Interfaces;
using CommandLine;
using NLog;
using NLog.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CliNet
{
    public class Program
    {
        #region Fields

        public static readonly string LOG_LAYOUT = "${longdate} | ${uppercase:${level}} | ${logger} | ${message}";
        public static readonly string TRACE_LAYOUT = "${uppercase:${level}} | ${message} | ${logger}";
        public static readonly char[] DELIMITER_CHARS = { ' ' };

        #endregion

        public static void Main(string[] args)
        {
            ConfigurationForLog();

            // IAction 구현 클래스를 전부 사용.
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(IAction).IsAssignableFrom(p) && p.IsAbstract == false);

            // 파라미터가 있는 경우 1회 실행 후 종료하는 모드.
            // 파라미터가 없는 경우 지속적인 명령어 입력이 가능한 모드. 종료 명령으로 종료.
            bool isContinuous = args.Length <= 0;

            do
            {
                IEnumerable<string> currentArgs = args.OfType<string>().ToList();
                if (currentArgs.Count() <= 0)
                {
                    // 새로운 파라미터 입력.
                    currentArgs = Console.ReadLine().Split(DELIMITER_CHARS);
                }

                int commandResult = Parser.Default.ParseArguments(currentArgs, types.ToArray())
                    .MapResult((IAction opts) =>
                    {
                        if (opts.IsValid == false)
                        {
                            Console.WriteLine("Invalid command.");
                            return 0;
                        }

                        return opts.Action();
                    }, errs => 0);

                isContinuous &= commandResult >= 0;
                if (isContinuous == true)
                {
                    // 지속적으로 사용하는 경우 결과값 출력.
                    Console.WriteLine(commandResult < 0 ? "Error" : "Ok");
                }
            } while (isContinuous == true);

            ThreadManager.Instance.Release();
        }

        #region Private methods

        /// <summary>
        /// 로그 셋팅.
        /// </summary>
        private static void ConfigurationForLog()
        {
            var config = new LoggingConfiguration();

            // 콘솔 로그 룰.     
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, new NLog.Targets.DebuggerTarget()
            {
                Layout = TRACE_LAYOUT
            });

            // 파일 로그 룰.
            config.AddRule(LogLevel.Info, LogLevel.Fatal, new NLog.Targets.FileTarget()
            {
                FileName = string.Format(@"{0}${{shortdate}}.log", Constant.LOG_FOLDER_PATH),
                Layout = LOG_LAYOUT
            });

            LogManager.Configuration = config;
        }

        #endregion
    }
}
