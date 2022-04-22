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

    public enum ProtocolType
    {
        UDP,
        COM,
    }
}
