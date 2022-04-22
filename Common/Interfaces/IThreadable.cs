namespace Common.Interfaces
{
    /// <summary>
    /// 스레드 동작 인터페이스.
    /// </summary>
    public interface IThreadable
    {
        /// <summary>
        /// 스레드 시작.
        /// </summary>
        void Start();

        /// <summary>
        /// 스레드 중지.
        /// </summary>
        void Stop();
    }
}
