// https://stackoverflow.com/questions/36201456/c-sharp-is-there-a-method-to-serialize-to-urlencoded

using System;

namespace Common.Attributes
{
    /// <summary>
    /// URL 엔코딩 이름 애트리뷰트.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class UrlEncodeAttribute : Attribute
    {
        #region Constructors

        public UrlEncodeAttribute(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 이름.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        #endregion
    }
}
