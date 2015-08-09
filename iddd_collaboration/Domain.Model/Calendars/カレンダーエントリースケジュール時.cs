using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダーエントリースケジュール時 : IDomainEvent
    {
        public カレンダーエントリースケジュール時(
            テナント tenant, 
            カレンダーId calendarId, 
            カレンダーエントリーId calendarEntryId, 
            string description, 
            string location, 
            オーナ owner, 
            データレンジ timeSpan, 
            リピート repetition,
            アラーム alarm,
            IEnumerable<参加者> invitees)
        {
            this.Tenant = tenant;
            this.CalendarId = calendarId;
            this.CalendarEntryId = calendarEntryId;
            this.Description = description;
            this.Location = location;
            this.Owner = owner;
            this.TimeSpan = timeSpan;
            this.Repetition = repetition;
            this.Alarm = alarm;
            this.Invitees = invitees;
        }

        public テナント Tenant { get; private set; }
        public カレンダーId CalendarId { get; private set; }
        public カレンダーエントリーId CalendarEntryId { get; private set; }
        public string Description { get; private set; }
        public string Location { get; private set; }
        public オーナ Owner { get; private set; }
        public データレンジ TimeSpan { get; private set; }
        public リピート Repetition { get; private set; }
        public アラーム Alarm { get; private set; }
        public IEnumerable<参加者> Invitees { get; private set; }


        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}
