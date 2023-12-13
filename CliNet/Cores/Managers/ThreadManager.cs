using Common.Interfaces;
using Common.Templates;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Linq;

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

                LogManager.GetCurrentClassLogger().Info($"{key} 스레드를 제거합니다.");
            }

            thread.Finished += r =>
            {
                Remove(key);
            };
            thread.Start();

            if (_threadMap.TryAdd(key, thread))
            {
                LogManager.GetCurrentClassLogger().Info($"{key} 스레드를 추가합니다.");
            }
        }

        public void Remove(string key)
        {
            if (_threadMap.ContainsKey(key))
            {
                if (_threadMap.TryRemove(key, out IThreadable beforeThread))
                {
                    beforeThread.Stop();

                    LogManager.GetCurrentClassLogger().Info($"{key} 스레드를 제거합니다.");
                }
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
        }

        #endregion
    }
}
