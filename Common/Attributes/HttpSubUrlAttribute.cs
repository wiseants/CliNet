using System;

namespace Common.Attributes
{
    /// <summary>
    /// http 요청에 포함되는 서브 URL 애트리뷰트.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HttpSubUrlAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// 디폴트 서브 URL
        /// </summary>
        public static readonly string DEFAULT_SUB_URL = "/";

        #endregion

        #region Constructors

        public HttpSubUrlAttribute()
        {
        }

        public HttpSubUrlAttribute(string subUrl)
        {
            SubUrl = subUrl;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 서브 URL
        /// </summary>
        public string SubUrl
        {
            get; private set;
        } = DEFAULT_SUB_URL;

        #endregion
    }
}
