using System.ComponentModel;

namespace Common
{
    /// <summary>
    /// http 메소드 타입.
    /// </summary>
	public enum HttpMethodType
    {
        [Description("CONNECT")]
        CONNECT,

        [Description("GET")]
        GET,

        [Description("HEAD")]
        HEAD,

        [Description("MKCOL")]
        MKCOL,

        [Description("POST")]
        POST,

        [Description("PUT")]
        PUT
    }

    /// <summary>
    /// 프로토콜 타입.
    /// </summary>
    public enum ProtocolType
    {
        UDP,
        COM,
    }

    /// <summary>
    /// GCS 상태 타입.
    /// </summary>
    public enum GcsState
    {
        Idle = 0,
        Initialized = 1,
        Connecting = 2,
        Disarm = 3,
        Arm = 4
    }
}
