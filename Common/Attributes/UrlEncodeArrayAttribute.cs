using System;

namespace Common.Attributes
{
    /// <summary>
    /// URL 엔코딩 이름 컬렉션 애트리뷰트.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class UrlEncodeArrayAttribute : Attribute
    {
        #region Constructors

        public UrlEncodeArrayAttribute(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 컬렉션 이름.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        #endregion
    }
}
