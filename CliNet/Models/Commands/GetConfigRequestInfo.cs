using Newtonsoft.Json;

namespace CliNet.Models.Commands
{
    /// <summary>
    /// 설정 요청 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class GetConfigRequestInfo : PacketInfo
    {
        #region Constructors

        public GetConfigRequestInfo()
        {
            Type = 0;
            Name = "GetConfig";
        }

        #endregion
    }
}
