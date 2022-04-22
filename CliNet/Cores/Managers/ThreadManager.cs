using Common.Interfaces;
using Common.Templates;
using System.Linq;
using System.Collections.Concurrent;

namespace CliNet.Cores.Managers
{
    public class ThreadManager : Singleton<ThreadManager>
    {
        #region Fields

        private ConcurrentDictionary<string, IThreadable> _threadMap = new ConcurrentDictionary<string, IThreadable>();

        #endregion

        #region Public methods

        public void Add(string key, IThreadable thread)
        {
            if (_threadMap.TryGetValue(key, out IThreadable beforeThread))
            {
                beforeThread.Stop();
            }

            thread.Start();
            _ = _threadMap.TryAdd(key, thread);
        }

        public void Remove(string key)
        {
            if (_threadMap.ContainsKey(key))
            {
                if (_threadMap.TryRemove(key, out IThreadable beforeThread))
                {
                    beforeThread.Stop();
                }
            }
        }

        public void Release()
        {
            _threadMap.ToList().ForEach(x =>
            {
                x.Value.Stop();
            });
        }

        #endregion
    }
}
