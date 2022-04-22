using Common.Templates;
using System.Collections.Generic;
using System.Diagnostics;

namespace Common.Tools
{
    /// <summary>
    /// 시간 간격 체크용 도구 클래스.
    /// </summary>
    public class PeriodChecker : Singleton<PeriodChecker>
    {
        #region Fields

        private readonly Dictionary<string, Stopwatch> _watchMap = new Dictionary<string, Stopwatch>();

        #endregion

        #region Constructors

        private PeriodChecker()
        {

        }

        #endregion

        #region Public methods

        /// <summary>
        /// 이전 체크 메소드 호출시부터 현재 체크 메소드 호출까지의 시간을 밀리초(ms)로 출력.
        /// </summary>
        /// <param name="keyword">식별용 키워드.</param>
        /// <returns>호출간 시간 간격 밀리초(ms).</returns>
        public long Check(string keyword)
        {
            long period = 0;

            if (_watchMap.TryGetValue(keyword, out Stopwatch watch))
            {
                period = watch.ElapsedMilliseconds;
                watch.Restart();
            }
            else
            {
                Stopwatch newWatch = new Stopwatch();
                newWatch.Start();
                _watchMap.Add(keyword, newWatch);
            }

            return period;
        }

        /// <summary>
        /// 해당 키워드 체크 제거.
        /// </summary>
        /// <param name="keyword"></param>
        public void Remove(string keyword)
        {
            _watchMap.Remove(keyword);
        }

        #endregion
    }
}
