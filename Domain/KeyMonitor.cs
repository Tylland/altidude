using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Altidude.Domain
{
    public static class KeyMonitor
    {
        public class MonitorObject
        {
            public int Counter { get; set; }
        }

        private static readonly ConcurrentDictionary<string, MonitorObject> MonitorObjects = new ConcurrentDictionary<string, MonitorObject>();

        public static void Enter<T>(T key)
        {
            var monitorObject = MonitorObjects.GetOrAdd(key.ToString(), x => new MonitorObject());

            monitorObject.Counter++;

            Monitor.Enter(monitorObject);
        }

        public static void Exit<T>(T key)
        {
            MonitorObject monitorObject;

            if(MonitorObjects.TryGetValue(key.ToString(), out monitorObject))
            {
                Monitor.Exit(monitorObject);
                monitorObject.Counter--;

                if(monitorObject.Counter == 0)
                    MonitorObjects.TryRemove(key.ToString(), out monitorObject);
            }
        }
    }
}
