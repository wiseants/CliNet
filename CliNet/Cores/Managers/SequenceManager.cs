using Common.Templates;
using Common.Tools;
using System;

namespace CliNet.Cores.Managers
{
    /// <summary>
    /// 시퀀스 번호 관리자.
    /// </summary>
    public class SequenceManager : Singleton<SequenceManager>
    {
        private readonly string SEQ_KEY = "LastSeqNo";

        #region Properties

        /// <summary>
        /// 현재 시퀀스 번호.
        /// </summary>
        public int CurrentSeqNo
        {
            get => Convert.ToInt32(AppConfiguration.GetAppConfig(SEQ_KEY));
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 다음 시퀀스 번호.
        /// 자동으로 시퀀스 번호는 증가.
        /// </summary>
        /// <returns></returns>
        public int GetNext()
        {
            int nextSeqNo = CurrentSeqNo + 1;

            AppConfiguration.SetAppConfig(SEQ_KEY, nextSeqNo.ToString());

            return nextSeqNo;
        }

        /// <summary>
        /// 시퀀스 번호 초기화.
        /// </summary>
        public void Reset()
        {
            AppConfiguration.SetAppConfig(SEQ_KEY, 0.ToString());
        }

        #endregion
    }
}
