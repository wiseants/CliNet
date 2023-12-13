using Newtonsoft.Json;

namespace CliNet.Models.Commands
{
    /// <summary>
    /// 패킷 기본 정보.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class PacketInfo
    {
        /// <summary>
        /// 명령 타입.
        /// 0:요청, 1:응답.
        /// </summary>
        public int Type
        {
            get;
            set;
        }

        /// <summary>
        /// 명령 이름.
        /// 명령에 대한 식별자로 사용.
        /// </summary>
        public string Name
        { 
            get; 
            set; 
        }

        /// <summary>
        /// 시퀀스 번호.
        /// 요청과 응답이 동일한 값을 사용.
        /// </summary>
        public int SeqNo
        {
            get;
            set;
        }
    }
}
