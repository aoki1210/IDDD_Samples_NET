using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダーエントリー再スケジュール時 : IDomainEvent
    {
        public カレンダーエントリー再スケジュール時(
            テナント tenant,
            カレンダーId calendarId,
            カレンダーエントリーId calendarEntryId,
            データレンジ timeSpan,
            リピート repetition,
            アラーム alarm)
        {
            this.Tenant = tenant;
            this.CalendarId = calendarId;
            this.CalendarEntryId = calendarEntryId;
            this.TimeSpan = timeSpan;
            this.Repetition = repetition;
            this.Alarm = alarm;
        }

        public テナント Tenant { get; private set; }
        public カレンダーId CalendarId { get; private set; }
        public カレンダーエントリーId CalendarEntryId { get; private set; }
        public データレンジ TimeSpan { get; private set; }
        public リピート Repetition { get; private set; }
        public アラーム Alarm { get; private set; }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}
