using Newtonsoft.Json;

namespace CliNet.Models.Commands
{
    /// <summary>
    /// 전원 요청 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SetEnableRequestInfo : PacketInfo
    {
        #region Constructors

        public SetEnableRequestInfo()
        {
            Type = 0;
            Name = "SetEnable";
        }

        #endregion

        #region Properties

        /// <summary>
        /// 동작 켜기/끄기
        /// true:켜기, false:끄기.
        /// </summary>
        public bool IsEnable
        {
            get;
            set;
        }

        #endregion
    }
}
