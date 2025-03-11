using Newtonsoft.Json;

namespace CliNet.Models.Commands.AiModule
{
    /// <summary>
    /// 목표 위치 입력 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SetGoalInfo : PacketInfo
    {
        #region Constructors

        public SetGoalInfo()
        {
            Name = "SetGoal";
        }

        #endregion

        #region Properties

        /// <summary>
        /// 목표 위도.
        /// </summary>
        public double GoalLatitude
        {
            get;
            set;
        }

        /// <summary>
        /// 목표 경도.
        /// </summary>
        public double GoalLongitude
        {
            get;
            set;
        }

        #endregion
    }
}
