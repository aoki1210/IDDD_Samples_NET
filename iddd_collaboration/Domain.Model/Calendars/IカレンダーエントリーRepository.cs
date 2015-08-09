using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public interface IカレンダーエントリーRepository
    {
        カレンダーエントリー Get(Tenants.テナント tenant, カレンダーエントリーId calendarId);
        カレンダーエントリーId GetNextIdentity();
        void Save(カレンダーエントリー calendarEntry);
    }
}
