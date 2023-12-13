using Newtonsoft.Json;

namespace CliNet.Models.Commands
{
    /// <summary>
    /// 전원 응답 모델.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class GetConfigResponseInfo : PacketInfo
    {
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

        /// <summary>
        /// 반환 코드.
        /// 0:실패, 1:성공, 2이상:에러 코드.
        /// </summary>
        public int ReturnCode
        {
            get;
            set;
        }

        #endregion
    }
}
