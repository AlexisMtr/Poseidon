using PoseidonFA.Configuration;
using PoseidonFA.Models;

namespace PoseidonFA.Repositories.SQL
{
    public class AlarmRepository : IAlarmRepository
    {
        private readonly PoseidonContext context;

        public AlarmRepository(PoseidonContext context)
        {
            this.context = context;
        }

        public void Add(Alarm alarm)
        {
            context.Alarms.Add(alarm);
        }

        public void Remove(Alarm alarm)
        {
            context.Alarms.Remove(alarm);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
