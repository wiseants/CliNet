using System.Net.Sockets;

namespace Common.Models
{
    /// <summary>
    /// 소켓 반환 상태 모델.
    /// </summary>
    public class SocketStateInfo
    {
        #region Fields

        /// <summary>
        /// 버퍼 사이즈.
        /// </summary>
        public static readonly int BufferSize = 256;

        #endregion

        #region Properties

        /// <summary>
        /// 소켓 객체.
        /// </summary>
        public Socket WorkSocket
        {
            get;
            set;
        }

        /// <summary>
        /// 받기 버퍼.
        /// </summary>
        public byte[] Buffer
        {
            get;
            set;
        } = new byte[BufferSize];

        #endregion
    }
}
