using Poseidon.Models;
using System.Collections.Generic;
using System.Linq;

namespace Poseidon.Repositories
{
    public interface IAlarmsRepository<T> : IRepository<T> where T : Alarm
    {
        void Ack(string id);
        IEnumerable<Alarm> GetByPoolId(string poolId, AlarmState filter);
        IQueryable<Alarm> Get(AlarmState state);
    }
}
