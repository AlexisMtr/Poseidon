using Poseidon.Exceptions;
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

        public Alarm Get(int id, User user)
        {
            IFilter<Alarm> filter = new AlarmFilter();
            if(user != null)
            {
                filter = new IdentityAlarmFilter(filter, user);
            }
            Alarm alarm = alarmRepository.GetById(id, filter);
            if (alarm == null) throw new NotFoundException(typeof(Alarm));

            return alarm;
        }

        public PaginatedElement<Alarm> GetByPool(int id, IFilter<Alarm> filter, int rowsPerPage, int pageNumber, User user = null)
        {
            if(user != null)
            {
                filter = new IdentityAlarmFilter(filter, user);
            }
            IEnumerable<Alarm> alarms = alarmRepository.GetByPool(id, filter, rowsPerPage, pageNumber);
            int totalElementCount = alarmRepository.CountByPool(id, filter);

            return new PaginatedElement<Alarm>
            {
                TotalElementCount = totalElementCount,
                Elements = alarms,
                PageCount = RestApiHelper.GetPageCount(totalElementCount, rowsPerPage)
            };
        }

        public Alarm Ack(int id, User user)
        {
            Alarm alarm = Get(id, user);
            alarm.Ack = true;
            alarm.AcknowledgmentDateTime = DateTime.UtcNow;
            alarmRepository.SaveChanges();

            return alarm;
        }
    }
}
