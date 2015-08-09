using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダーエントリー再配置時 : IDomainEvent
    {
        public カレンダーエントリー再配置時(
            テナント tenant,
            カレンダーId calendarId,
            カレンダーエントリーId calendarEntryId,
            string location)
        {
            this.Tenant = tenant;
            this.CalendarId = calendarId;
            this.CalendarEntryId = calendarEntryId;
            this.Location = location;
        }

        public テナント Tenant { get; private set; }
        public カレンダーId CalendarId { get; private set; }
        public カレンダーエントリーId CalendarEntryId { get; private set; }
        public string Location { get; private set; }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}
