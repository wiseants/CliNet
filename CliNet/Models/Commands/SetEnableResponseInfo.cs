using Newtonsoft.Json;

namespace CliNet.Models.Commands
{
    /// <summary>
    /// 전원 응답 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SetEnableResponseInfo : PacketInfo
    {
        #region Constructors

        public SetEnableResponseInfo()
        {
            Type = 1;
            Name = "SetEnable";
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
