using PoseidonFA.Models;
using PoseidonFA.Repositories;

namespace PoseidonFA.Services
{
    public class AlarmService
    {
        private readonly IAlarmRepository alarmRepository;

        public AlarmService(IAlarmRepository alarmRepository)
        {
            this.alarmRepository = alarmRepository;
        }

        public Alarm Add(Alarm alarm)
        {
            alarmRepository.Add(alarm);
            alarmRepository.SaveChanges();

            return alarm;
        }
    }
}
