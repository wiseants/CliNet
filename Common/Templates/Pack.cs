using Prism.Events;
using Prism.Ioc;

namespace Common.Templates
{
    /// <summary>
    /// 컨테이너 패키지 추상 클래스.
    /// </summary>
    /// <typeparam name="C">컨테이너 타입.</typeparam>
    public abstract class Pack<C> : Singleton<C> where C : class
    {
        #region Public methods

        /// <summary>
        /// 초기화.
        /// </summary>
        /// <param name="container">유니티컨테이너 객체.</param>
        /// <param name="eventAggregator">이벤트 애그리게이터 객체.</param>
        public void Initialize(IContainerExtension container, IEventAggregator eventAggregator)
        {
            Bootstrapper(container, eventAggregator);
        }

        #endregion

        #region Abstract methods

        public abstract void Bootstrapper(IContainerExtension container, IEventAggregator eventAggregator);

        #endregion
    }
}
