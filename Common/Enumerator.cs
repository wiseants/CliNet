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
        // 미가동.
        Idle = 0,
        // 초기화 완료.
        Initialized = 1,
        // 연결 중.
        Connecting = 2,
        // 연결 완료. 비행 준비 중.
        Disarm = 3,
        // 비행 중.
        Arm = 4
    }

    /// <summary>
    /// GCS 요청 결과.
    /// </summary>
    public enum RequestResult
    {
        // 실패.
        Fail = 0,
        // 성공.
        Success = 1,
        // 패킷 분석 오류.
        ParsingError = 2,
        // 유효하지 않은 시점의 요청.
        InvalidRequest = 3,
    }
}
