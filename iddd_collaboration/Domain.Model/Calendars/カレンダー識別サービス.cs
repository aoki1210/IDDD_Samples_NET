using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダー識別サービス
    {
        public カレンダー識別サービス(IカレンダーRepository calendarRepository, IカレンダーエントリーRepository calendarEntryRepository)
        {
            this.calendarRepository = calendarRepository;
            this.calendarEntryRepository = calendarEntryRepository;
        }

        readonly IカレンダーRepository calendarRepository;
        readonly IカレンダーエントリーRepository calendarEntryRepository;

        public カレンダーId GetNextCalendarId()
        {
            return this.calendarRepository.GetNextIdentity();
        }

        public カレンダーエントリーId GetNextCalendarEntryId()
        {
            return this.calendarEntryRepository.GetNextIdentity();
        }
    }
}
