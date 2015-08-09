using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public interface IカレンダーRepository
    {
        カレンダー Get(Tenants.テナント tenant, カレンダーId calendarId);
        カレンダーId GetNextIdentity();
        void Save(カレンダー calendar);
    }
}
