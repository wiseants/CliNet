using Common;
using Newtonsoft.Json;

namespace CliNet.Models.Commands.AiModule
{
    /// <summary>
    /// 서버 상태 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ServerStateInfo : ResponsePacketInfo
    {
        #region Constructors

        public ServerStateInfo()
        {
            Name = "ServerState";
        }

        #endregion

        #region Properties

        /// <summary>
        /// GCS 상태.
        /// </summary>
        public GcsState State
        {
            get;
            set;
        }

        #endregion
    }
}
