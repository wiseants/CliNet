using Newtonsoft.Json;

namespace CliNet.Models.Commands.AiModule
{
    /// <summary>
    /// 나에게 보내라 요청 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SendToRequestInfo : PacketInfo
    {
        #region Constructors

        public SendToRequestInfo()
        {
            Type = 0;
            Name = "SendTo";
        }

        #endregion

        #region Properties

        /// <summary>
        /// 가공된 영상 스트림 보내기 IP 주소.
        /// </summary>
        public string TripIpAddress
        {
            get;
            set;
        }

        #endregion
    }
}
