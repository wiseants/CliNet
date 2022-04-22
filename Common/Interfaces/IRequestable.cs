using Common;
using System.Net;

namespace Common.Interfaces
{
    /// <summary>
    /// http 요청을 보내기위한 정보.
    /// </summary>
    public interface IRequestable
    {
        #region Properties

        /// <summary>
        /// 메인 URL
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// 서브 URL
        /// </summary>
        string SubUrl { get; set; }

        /// <summary>
        /// 메소드 이름.
        /// ex ) MEHTOD, GET
        /// </summary>
        HttpMethodType Method { get; set; }

        /// <summary>
        /// 쿠키 정보.
        /// </summary>
        CookieContainer Cookies { get; set; }

        /// <summary>
        /// 바디 형식 타입.
        /// </summary>
        string ContentType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Accept { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string AcceptEncoding { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string AcceptLanguage { get; set; }

        #endregion
    }
}
