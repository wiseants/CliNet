using Common.Models;
using Newtonsoft.Json;
using System;

namespace CliNet.Models.Commands
{
    /// <summary>
    /// 패킷 기본 정보.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class ResponsePacketInfo : PacketInfo
    {
        #region Properties

        /// <summary>
        /// 요청 결과.
        /// </summary>
        public int ResultCode
        { 
            get; 
            set; 
        }

        #endregion
    }
}
