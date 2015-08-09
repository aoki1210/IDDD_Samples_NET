using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダーエントリー参加者招待時 : IDomainEvent
    {
        public カレンダーエントリー参加者招待時(
            テナント tenant,
            カレンダーId calendarId,
            カレンダーエントリーId calendarEntryId,
            参加者 participant)
        {
            this.Tenant = tenant;
            this.CalendarId = calendarId;
            this.CalendarEntryId = calendarEntryId;
            this.Participant = participant;
        }

        public テナント Tenant { get; private set; }
        public カレンダーId CalendarId { get; private set; }
        public カレンダーエントリーId CalendarEntryId { get; private set; }
        public 参加者 Participant { get; private set; }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}
