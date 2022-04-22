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
            }

            thread.Start();
            if (_threadMap.TryAdd(key, thread))
            {
                Console.WriteLine("Start to listen.");
                LogManager.GetCurrentClassLogger().Info("Start to listen.");
            }
        }

        public void Remove(string key)
        {
            if (_threadMap.ContainsKey(key))
            {
                if (_threadMap.TryRemove(key, out IThreadable beforeThread))
                {
                    beforeThread.Stop();

                    Console.WriteLine("Stop to listen.");
                    LogManager.GetCurrentClassLogger().Info("Stop to listen.");
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
