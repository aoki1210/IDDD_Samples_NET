using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダー非共有時 : IDomainEvent
    {
        public カレンダー非共有時(テナント tenant, カレンダーId calendarId, string name, カレンダー共有 unsharedWith)
        {
            this.Tenant = tenant;
            this.CalendarId = calendarId;
            this.Name = name;
            this.UnsharedWith = unsharedWith;
        }

        public テナント Tenant { get; private set; }
        public カレンダーId CalendarId { get; private set; }
        public string Name { get; private set; }
        public カレンダー共有 UnsharedWith { get; private set; }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}
