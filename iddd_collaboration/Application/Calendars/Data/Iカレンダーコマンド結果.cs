using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Application.Calendars.Data
{
    public interface Iカレンダーコマンド結果
    {
        void SetResultingCalendarId(string calendarId);

        void SetResultingCalendarEntryId(string calendarEntryId);
    }
}
