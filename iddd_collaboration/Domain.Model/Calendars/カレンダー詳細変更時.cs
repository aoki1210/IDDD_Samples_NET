﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダー詳細変更時 : IDomainEvent
    {
        public カレンダー詳細変更時(テナント tenant, カレンダーId calendarId, string name, string description)
        {
            this.Tenant = tenant;
            this.CalendarId = calendarId;
            this.Name = name;
            this.Description = description;
        }

        public テナント Tenant { get; private set; }

        public カレンダーId CalendarId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }


        public int EventVersion { get; set; }

        public DateTime OccurredOn { get; set; }
    }
}
