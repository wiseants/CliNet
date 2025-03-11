using Common.Models;
using Newtonsoft.Json;
using System;

namespace CliNet.Models.Commands
{
    /// <summary>
    /// 패킷 기본 정보.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class PacketInfo
    {
        #region Properties

        /// <summary>
        /// 명령 이름.
        /// 명령에 대한 식별자로 사용.
        /// </summary>
        public string Name
        { 
            get; 
            set; 
        }

        #endregion

        #region Override methods

        public override bool Equals(object obj)
        {
            return (obj is PacketInfo packet) && Name.Equals(packet.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #endregion
    }
}
