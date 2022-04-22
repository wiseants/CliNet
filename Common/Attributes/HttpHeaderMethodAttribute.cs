using System;

namespace Common.Attributes
{
    /// <summary>
    /// http 헤더에 포함되는 메소드 파라미터 애트리뷰트.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HttpHeaderMethodAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// 디폴트 메소드 이름.
        /// </summary>
        public static readonly HttpMethodType DEFAULT_METHOD = HttpMethodType.CONNECT;

        #endregion

        #region Constructors

        public HttpHeaderMethodAttribute()
        {
        }

        public HttpHeaderMethodAttribute(HttpMethodType method)
        {
            Method = method;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 메소드 이름.
        /// ex ) MEHTOD, GET
        /// </summary>
        public HttpMethodType Method
        {
            get; private set;
        } = DEFAULT_METHOD;

        #endregion
    }
}
