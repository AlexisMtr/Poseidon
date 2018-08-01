using Poseidon.Filters;
using Poseidon.Helpers;
using Poseidon.Models;
using Poseidon.Repositories;
using System;
using System.Collections.Generic;

namespace Poseidon.Services
{
    public class AlarmService
    {
        private readonly IAlarmRepository alarmRepository;

        public AlarmService(IAlarmRepository alarmRepository)
        {
            this.alarmRepository = alarmRepository;
        }

        public Alarm Get(int id)
        {
            return alarmRepository.GetById(id);
        }

        public PaginatedElement<Alarm> GetByPool(int id, IFilter<Alarm> filter, int rowsPerPage, int pageNumber)
        {
            IEnumerable<Alarm> alarms = alarmRepository.GetByPool(id, filter, rowsPerPage, pageNumber);
            int totalElementCount = alarmRepository.CountByPool(id, filter);

            return new PaginatedElement<Alarm>
            {
                TotalElementCount = totalElementCount,
                Elements = alarms,
                PageCount = RestApiHelper.GetPageCount(totalElementCount, rowsPerPage)
            };
        }

        public Alarm Ack(int id)
        {
            Alarm alarm = Get(id);
            alarm.Ack = true;
            alarm.AcknowledgmentDateTime = DateTime.UtcNow;
            alarmRepository.SaveChanges();

            return alarm;
        }
    }
}
