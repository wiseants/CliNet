using Newtonsoft.Json;

namespace CliNet.Models.Commands.AiModule
{
    /// <summary>
    /// 서버 상태 요청 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class GetServerStateInfo : PacketInfo
    {
        #region Constructors

        public GetServerStateInfo()
        {
            Name = "GetServerState";
        }

        #endregion
    }
}
