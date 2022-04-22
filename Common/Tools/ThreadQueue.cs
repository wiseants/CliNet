using Common.Extensions;
using NLog;
using System;
using System.Threading;

namespace Common.Tools
{
    /// <summary>
    /// 1개 스레드만 동작하는 스레드 큐 클래스.
    /// 작업을 입력해도 바로 처리하는게 아니라 1개 스레드로 작업을 순차적으로 처리합니다.
    /// </summary>
    public class ThreadQueue : IDisposable
    {
        #region Fields

        private Thread _thread;
        private ConcurrentQueueEx<dynamic> _queue = new ConcurrentQueueEx<dynamic>();
        private object _lockObject = new object();

        #endregion

        #region Events

        /// <summary>
        /// 작업 시작전 이벤트.
        /// </summary>
        public event Action<object> StartingProc;

        /// <summary>
        /// 작업 종료후 이벤트.
        /// </summary>
        public event Action<object> FinishedProc;

        #endregion

        #region Properties

        /// <summary>
        /// 아이템 개수.
        /// </summary>
        public int Count
        {
            get => _queue.Count;
        }

        /// <summary>
        /// 비었는지 여부.
        /// </summary>
        public bool IsEmpty
        {
            get => _queue.IsEmpty;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// 작업을 큐에 입력합니다.
        /// </summary>
        /// <param name="proc">작업 프로세스.</param>
        /// <param name="param">작업 프로세스에 전달할 파라미터.</param>
        /// <param name="token">캔슬 토큰.</param>
        public void Enqueue(Action<object, CancellationTokenSource> proc, object param, CancellationTokenSource token)
        {
            _queue.Enqueue(new { Id = Guid.NewGuid(), Proc = proc, Param = param, TokenSource = token });

            lock(_lockObject)
            {
                if (_thread == null)
                {
                    _thread = new Thread(ThreadProc);
                    _thread.Start();
                }
            }
        }

        /// <summary>
        /// 작업을 완료한 경우 호출합니다.
        /// </summary>
        public void Finish()
        {
            _ = _queue.TryDequeue(out _);
        }

        /// <summary>
        /// 작업을 중지하고 큐를 클리어합니다.
        /// </summary>
        public void Abort()
        {
            if (_thread != null)
            {
                _thread.Abort();
                _thread = null;
            }

            while (_queue.TryDequeue(out _))
            {
            }
        }

        #endregion

        #region Private methods

        private void ThreadProc(object param)
        {
            while (_queue.IsEmptyWithIfAction(() => _thread = null) == false)
            {
                if (_queue.TryPeek(out dynamic result))
                {
                    StartingProc?.Invoke(result.Param);

                    try
                    {
                        result.Proc(result.Param, result.TokenSource);
                    }
                    catch(Exception ex)
                    {
                        LogManager.GetCurrentClassLogger().Error("예외 발생. 메시지( {0} )", ex.Message);
                    }
                    finally
                    {
                        // 큐잉된 프로세스안에서 프로세스를 제거해야 합니다.
                        while (_queue.TryPeek(out dynamic first) && first.Id == result.Id)
                        {
                            Thread.Sleep(20);
                        }
                    }

                    FinishedProc?.Invoke(result.Param);
                }
            }
        }

        #endregion

        #region IDisposable implementations

        public void Dispose()
        {
            Abort();
        }

        #endregion
    }
}
