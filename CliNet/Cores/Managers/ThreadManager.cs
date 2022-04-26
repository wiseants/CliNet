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

                Console.WriteLine("Remove a thread.");
                LogManager.GetCurrentClassLogger().Info("Remove a thread.");
            }

            thread.Start();
            if (_threadMap.TryAdd(key, thread))
            {
                LogManager.GetCurrentClassLogger().Info("Add a thread.");
            }
        }

        public void Remove(string key)
        {
            if (_threadMap.ContainsKey(key))
            {
                if (_threadMap.TryRemove(key, out IThreadable beforeThread))
                {
                    beforeThread.Stop();

                    Console.WriteLine("Remove a thread.");
                    LogManager.GetCurrentClassLogger().Info("Remove a thread.");
                }
            }
        }

        public void Release()
        {
            _threadMap.Keys.ToList().ForEach(x => Remove(x));
        }

        #endregion
    }
}
