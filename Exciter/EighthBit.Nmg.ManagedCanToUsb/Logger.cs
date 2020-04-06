using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EighthBit.Nmg.ManagedCanToUsb
{
    public static class Logger
    {
        private static readonly List<ILogger> _loggers = new List<ILogger>();

        public static void Register(ILogger log)
        {
            _loggers.Add(log);
        }

        public static void Unregister(ILogger log)
        {
            _loggers.Remove(log);
        }

        public static void UnregisterAll()
        {
            _loggers.Clear();
        }

        internal static void Log(string message)
        {
            foreach (var log in _loggers)
                log.Log(message);
        }

    }
}
