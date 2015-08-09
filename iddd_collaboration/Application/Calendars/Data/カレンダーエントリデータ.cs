using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Application.Calendars.Data
{
    public class カレンダーエントリデータ
    {
        public int AlarmAlarmUnits { get; set; }
        public string AlarmAlarmUnitsType { get; set; }
        public string CalendarEntryId { get; set; }
        public string CalendarId { get; set; }
        public string Description { get; set; }
        public ISet<カレンダーエントリー招待された人データ> Invitees { get; set; }
        public string Location { get; set; }
        public string OwnerEmailAddress { get; set; }
        public string OwnerIdentity { get; set; }
        public string OwnerName { get; set; }
        public DateTime RepetitionEnds { get; set; }
        public string RepetitionType { get; set; }
        public string TenantId { get; set; }
        public DateTime TimeSpanBegins { get; set; }
        public DateTime TimeSpanEnds { get; set; }
    }
}
