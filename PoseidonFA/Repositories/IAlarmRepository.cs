using PoseidonFA.Models;

namespace PoseidonFA.Repositories
{
    public interface IAlarmRepository
    {
        void Add(Alarm alarm);
        void Remove(Alarm alarm);
        void SaveChanges();
    }
}
