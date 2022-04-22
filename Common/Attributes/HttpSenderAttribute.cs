using System;

namespace Common.Attributes
{
    /// <summary>
    /// http 요청 보내기 컨테이너 이름 애트리뷰트.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HttpSenderAttribute : Attribute
    {
        #region Fields

        public static readonly string DEFAULT = "GET";

        #endregion

        #region Constructors

        public HttpSenderAttribute()
        {
        }

        public HttpSenderAttribute(string sender)
        {
            Sender = sender;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 컨테이너 이름.
        /// </summary>
        public string Sender
        {
            get; private set;
        } = DEFAULT;

        #endregion
    }
}
