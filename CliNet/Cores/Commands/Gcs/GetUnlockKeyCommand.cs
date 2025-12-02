using CommandLine;
using Common.Tools;
using System;

namespace CliNet.Cores.Commands.Gcs
{
    [Verb("get.unlock", HelpText = "GCS 상태 정보 요청.")]
    internal class GetUnlockKeyCommand : Interfaces.IAction
    {
        #region Fields

        private readonly int BUFFER_SIZE = 1024;

        #endregion

        #region Constructors

        public GetUnlockKeyCommand() 
        {
        }

        #endregion

        #region Properties

        public bool IsValid => true;

        [Option('k', "key", Required = true, HelpText = "입력 초기화 키.")]
        public string ResetKey
        {
            get;
            set;
        }

        #endregion

        #region Public methods

        public int Action()
        {
            bool isSuccess = false;
            ushort unlockKey = default;

            try
            {
                unlockKey = KeyTool.GetSimpleHash(Convert.ToUInt16(ResetKey, 16));

                isSuccess = true;
            }
            catch { }

            Console.WriteLine(isSuccess ? $"해제 키: {unlockKey:X4}" : "생성 실패");

            return 0;
        }

        #endregion
    }
}
