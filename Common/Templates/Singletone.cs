namespace Common.Templates
{
    /// <summary>
    /// 싱글톤 추상 클래스.
    /// </summary>
    /// <typeparam name="T">싱글톤으로 생성할 타입.</typeparam>
    public abstract class Singleton<T> where T : new()
    {
        private static T _instance;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// 싱글톤 객체.
        /// </summary>
        public static T Instance
        {
            get
            {
                lock(_lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new T();
                    }
                }

                return _instance;
            }
        }
    }
}