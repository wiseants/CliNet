// https://stackoverflow.com/questions/19404199/how-to-to-make-udpclient-receiveasync-cancelable

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Tools
{
    /// <summary>
    /// 비동기 액션 도구.
    /// </summary>
    public static class AsyncTools
    {
        /// <summary>
        /// 태스크 취소 가능하게 하는 도구.
        /// </summary>
        /// <typeparam name="T">타입.</typeparam>
        /// <param name="task">취소할 태스크.</param>
        /// <param name="cancellationToken">캔슬 토큰.</param>
        /// <returns>결과 태스크.</returns>
        public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task))
                {
                    throw new OperationCanceledException(cancellationToken);
                }
            }

            return task.Result;
        }
    }
}
