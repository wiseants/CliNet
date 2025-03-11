using Common;
using Newtonsoft.Json;

namespace CliNet.Models.Commands
{
    /// <summary>
    /// 패킷 기본 정보.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ResponsePacketInfo : PacketInfo
    {
        #region Constructors

        public ResponsePacketInfo() : base()
        {
            Name = "Response";
        }

        #endregion

        #region Properties

        /// <summary>
        /// 요청 결과.
        /// </summary>
        public RequestResult ResultCode
        { 
            get; 
            set; 
        }

        #endregion
    }
}
