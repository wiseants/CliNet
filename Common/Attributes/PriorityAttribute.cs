using System;

namespace Common.Attributes
{
    /// <summary>
    /// 프로퍼트 출력 우선순위 애트리뷰트.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class PriorityAttribute : Attribute
    {
        #region Constructors

        public PriorityAttribute()
        {
        }

        public PriorityAttribute(int priority)
        {
            Priority = priority;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 우선순위 작은수일수록 우선순위가 높음.
        /// </summary>
        public int Priority
        {
            get; private set;
        } = Int32.MaxValue;

        #endregion
    }
}
