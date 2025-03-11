using Newtonsoft.Json;

namespace CliNet.Models.Commands.AiModule
{
    /// <summary>
    /// 기체 스테이터스 요청 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class GetVehicleStatusInfo : PacketInfo
    {
        #region Constructors

        public GetVehicleStatusInfo()
        {
            Name = "GetVehicleStatus";
        }

        #endregion
    }
}
