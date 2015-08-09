using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダーエントリー詳細変更時 : IDomainEvent
    {
        public カレンダーエントリー詳細変更時(
            テナント tenant,
            カレンダーId calendarId,
            カレンダーエントリーId calendarEntryId,
            string description)
        {
            this.Tenant = tenant;
            this.CalendarId = calendarId;
            this.CalendarEntryId = calendarEntryId;
            this.Description = description;
        }

        public テナント Tenant { get; private set; }
        public カレンダーId CalendarId { get; private set; }
        public カレンダーエントリーId CalendarEntryId { get; private set; }
        public string Description { get; private set; }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}
