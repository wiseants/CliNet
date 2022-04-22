// https://stackoverflow.com/questions/5852863/fixed-size-queue-which-automatically-dequeues-old-values-upon-new-enques
using System;
using System.Collections.Concurrent;

namespace Common.Extensions
{
    /// <summary>
    /// 동기화를 위한 확장형 큐.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentQueueEx<T> : ConcurrentQueue<T>
    {
        #region Fields

        private static readonly object _lockObject = new object();

        #endregion

        #region Properties

        /// <summary>
        /// 최대 개수.
        /// </summary>
        public int Limit
        {
            get; set;
        } = 100;

        #endregion

        #region Public methods

        /// <summary>
        /// IsEmptyWithIfAction 동기화를 위한 재작성 메소드.
        /// </summary>
        /// <param name="item"></param>
        public new void Enqueue(T item)
        {
            base.Enqueue(item);

            lock (_lockObject)
            {
                while (Count > Limit && TryDequeue(out _))
                {
                }
            }
        }

        /// <summary>
        /// 비었음을 확인하며 액션을 수행.
        /// </summary>
        /// <param name="ifProc"></param>
        /// <returns></returns>
        public bool IsEmptyWithIfAction(Action ifProc)
        {
            lock(_lockObject)
            {
                bool isEmpty = IsEmpty;
                if (isEmpty == true)
                {
                    ifProc();
                }

                return isEmpty;
            }
        }

        #endregion
    }
}
