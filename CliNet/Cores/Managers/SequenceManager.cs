using Common.Templates;
using System;
using System.Configuration;

namespace CliNet.Cores.Managers
{
    public class SequenceManager : Singleton<SequenceManager>
    {
        #region Properties

        public int CurrentSeqNo
        {
            get => Convert.ToInt32(ConfigurationManager.AppSettings["LastSeqNo"]);
        }

        #endregion

        #region Public methods

        public int GetNext()
        {
            int nextSeqNo = CurrentSeqNo + 1;

            ConfigurationManager.AppSettings["LastSeqNo"] = nextSeqNo.ToString();

            return nextSeqNo;
        }

        public void Reset()
        {
            ConfigurationManager.AppSettings["LastSeqNo"] = "0";
        }

        #endregion
    }
}
