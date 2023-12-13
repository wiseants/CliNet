using Newtonsoft.Json;

namespace CliNet.Models.Commands
{
    /// <summary>
    /// 설정 요청 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class SetConfigRequestInfo : PacketInfo
    {
        #region Constructors

        public SetConfigRequestInfo()
        {
            Type = 0;
            Name = "SetConfig";
        }

        #endregion

        #region Properties

        /// <summary>
        /// 영상 스트림 받기 타입
        /// 0:유니캐스트, 1:멀티캐스트
        /// </summary>
        public int ListenType
        {
            get;
            set;
        }

        /// <summary>
        /// 영상 스트림 받기 포트 번호.
        /// </summary>
        public int ListenPortNo
        {
            get;
            set;
        }

        /// <summary>
        /// 가공된 영상 스트림 보내기 타입
        /// 0:유니캐스트, 1:멀티캐스트
        /// </summary>
        public int SendType
        {
            get;
            set;
        }


        /// <summary>
        /// 가공된 영상 스트림 보내기 IP 주소.
        /// </summary>
        public string SendIpAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 가공된 영상 스트림 보내기 포트 번호.
        /// </summary>
        public int SendPortNo
        {
            get;
            set;
        }

        #endregion
    }
}
