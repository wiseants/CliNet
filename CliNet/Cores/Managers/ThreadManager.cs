using Common.Interfaces;
using Common.Templates;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace CliNet.Cores.Managers
{
    public class ThreadManager : Singleton<ThreadManager>
    {
        #region Fields

        private readonly ConcurrentDictionary<string, IThreadable> _threadMap = new ConcurrentDictionary<string, IThreadable>();

        #endregion

        #region Public methods

        public void Add(string key, IThreadable thread)
        {
            if (_threadMap.TryRemove(key, out IThreadable beforeThread))
            {
                beforeThread.Stop();
            }

            thread.Finished += r =>
            {
                _ = _threadMap.TryRemove(key, out _);

                Console.WriteLine($"[{key}] 스레드를 제거합니다.");
            };
            thread.Start();

            if (_threadMap.TryAdd(key, thread))
            {
                Console.WriteLine($"[{key}] 스레드를 시작합니다.");
            }
        }

        public void Remove(string key)
        {
            if (_threadMap.TryGetValue(key, out IThreadable thread))
            {
                thread.Stop();
            }
            else
            {
                Console.WriteLine("서버가 존재하지 않습니다.");
            }
        }

        public bool IsExist(string key)
        {
            return _threadMap.ContainsKey(key);
        }

        public void Release()
        {
            _threadMap.Keys.ToList().ForEach(x => Remove(x));

            Thread.Sleep(1000);
        }

        #endregion
    }
}
