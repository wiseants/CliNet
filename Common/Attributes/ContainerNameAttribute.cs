using System;

namespace Common.Attributes
{
    /// <summary>
    /// 컨테이너 이름 애트리뷰트.
    /// 부트스트래퍼가 이 애트리뷰트로 컨테이너를 등록합니다.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ContainerNameAttribute : Attribute
    {
        #region Constructors

        public ContainerNameAttribute()
        {
        }

        public ContainerNameAttribute(string name)
        {
            Name = name;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 컨테이너 이름.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        #endregion
    }
}
