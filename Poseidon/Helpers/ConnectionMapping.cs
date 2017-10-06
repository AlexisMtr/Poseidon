using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poseidon.Helpers
{
    public class ConnectionMapping<T> : IConnectionMapper<T>
    {
        private readonly Dictionary<T, HashSet<string>> ConnectionsId;
        private static object Lock;

        public int Count { get => ConnectionsId.Count; }

        public void Add(T key, string connectionId)
        {
            lock (Lock)
            {
                if (!ConnectionsId.TryGetValue(key, out HashSet<string> connections))
                {
                    connections = new HashSet<string>();
                    ConnectionsId.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            if (ConnectionsId.TryGetValue(key, out HashSet<string> connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (Lock)
            {
                if (!ConnectionsId.TryGetValue(key, out HashSet<string> connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        ConnectionsId.Remove(key);
                    }
                }
            }
        }

    }
}
