using Newtonsoft.Json;

namespace CliNet.Models.Commands.AiModule
{
    /// <summary>
    /// 나에게 보내라 응답 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SendToResponseInfo : PacketInfo
    {
        #region Constructors

        public SendToResponseInfo()
        {
            Type = 1;
            Name = "SendTo";
        }

        #endregion

        #region Properties

        /// <summary>
        /// 반환 코드.
        /// 0:실패, 1:성공, 2이상:에러 코드.
        /// </summary>
        public int ReturnCode
        {
            get;
            set;
        } = 0;

        #endregion
    }
}
