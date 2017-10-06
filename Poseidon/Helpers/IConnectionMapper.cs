using System.Collections.Generic;

namespace Poseidon.Helpers
{
    public interface IConnectionMapper<T>
    {
        int Count { get; }
        void Add(T key, string connectionId);
        IEnumerable<string> GetConnections(T key);
        void Remove(T key, string connectionId);
    }
}