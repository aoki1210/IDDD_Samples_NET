using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダーエントリー : EventSourcedRootEntity
    {
        public カレンダーエントリー(
            テナント tenant,
            カレンダーId calendarId,
            カレンダーエントリーId calendarEntryId,
            string description,
            string location,
            オーナ owner,
            データレンジ timeSpan,
            リピート repetition,
            アラーム alarm,
            IEnumerable<参加者> invitees = null)
        {
            AssertionConcern.AssertArgumentNotNull(tenant, "The tenant must be provided.");
            AssertionConcern.AssertArgumentNotNull(calendarId, "The calendar id must be provided.");
            AssertionConcern.AssertArgumentNotNull(calendarEntryId, "The calendar entry id must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(description, "The description must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(location, "The location must be provided.");
            AssertionConcern.AssertArgumentNotNull(owner, "The owner must be provided.");
            AssertionConcern.AssertArgumentNotNull(timeSpan, "The time span must be provided.");
            AssertionConcern.AssertArgumentNotNull(repetition, "The repetition must be provided.");
            AssertionConcern.AssertArgumentNotNull(alarm, "The alarm must be provided.");

            if (repetition.Repeats == リピートタイプ.DoesNotRepeat)
                repetition = リピート.DoesNotRepeat(timeSpan.Ends);

            AssertTimeSpans(repetition, timeSpan);

            Apply(new カレンダーエントリースケジュール時(tenant, calendarId, calendarEntryId, description, location, owner, timeSpan, repetition, alarm, invitees));
        }        

        テナント tenant;
        カレンダーId calendarId;
        カレンダーエントリーId calendarEntryId;
        string description;
        string location;
        オーナ owner;
        データレンジ timeSpan;
        リピート repetition;
        アラーム alarm;
        HashSet<参加者> invitees;

        public カレンダーエントリーId CalendarEntryId
        {
            get { return this.CalendarEntryId; }
        }

        void AssertTimeSpans(リピート repetition, データレンジ timeSpan)
        {
            if (repetition.Repeats == リピートタイプ.DoesNotRepeat)
            {
                AssertionConcern.AssertArgumentEquals(repetition.Ends, timeSpan.Ends, "Non-repeating entry must end with time span end.");
            }
            else
            {
                AssertionConcern.AssertArgumentFalse(timeSpan.Ends > repetition.Ends, "Time span must end when or before repetition ends.");
            }
        }

        void When(カレンダーエントリースケジュール時 e)
        {
            this.tenant = e.Tenant;
            this.calendarId = e.CalendarId;
            this.calendarEntryId = e.CalendarEntryId;
            this.description = e.Description;
            this.location = e.Location;
            this.owner = e.Owner;
            this.timeSpan = e.TimeSpan;
            this.repetition = e.Repetition;
            this.alarm = e.Alarm;
            this.invitees = new HashSet<参加者>(e.Invitees ?? Enumerable.Empty<参加者>());
        }


        public void ChangeDescription(string description)
        {
            if (description == null)
            {
                // TODO: consider
            }

            description = description.Trim();

            if (!string.IsNullOrEmpty(description) && !this.description.Equals(description))
            {
                Apply(new カレンダーエントリー詳細変更時(this.tenant, this.calendarId, this.calendarEntryId, description));
            }
        }

        void When(カレンダーエントリー詳細変更時 e)
        {
            this.description = e.Description;
        }
        

        public void Invite(参加者 participant)
        {
            AssertionConcern.AssertArgumentNotNull(participant, "The participant must be provided.");
            if (!this.invitees.Contains(participant))
            {
                Apply(new カレンダーエントリー参加者招待時(this.tenant, this.calendarId, this.calendarEntryId, participant));
            }
        }

        void When(カレンダーエントリー参加者招待時 e)
        {
            this.invitees.Add(e.Participant);
        }


        public void Relocate(string location)
        {
            if (location == null)
            {
                // TODO: consider
            }

            location = location.Trim();
            if (!string.IsNullOrEmpty(location) && !this.location.Equals(location))
            {
                Apply(new カレンダーエントリー再配置時(this.tenant, this.calendarId, this.calendarEntryId, location));
            }
        }

        void When(カレンダーエントリー再配置時 e)
        {
            this.location = e.Location;
        }


        public void Reschedule(string description, string location, データレンジ timeSpan, リピート repetition, アラーム alarm)
        {
            AssertionConcern.AssertArgumentNotNull(timeSpan, "The time span must be provided.");
            AssertionConcern.AssertArgumentNotNull(repetition, "The repetition must be provided.");
            AssertionConcern.AssertArgumentNotNull(alarm, "The alarm must be provided.");

            if (repetition.Repeats == リピートタイプ.DoesNotRepeat)
                repetition = リピート.DoesNotRepeat(timeSpan.Ends);

            AssertTimeSpans(repetition, timeSpan);

            ChangeDescription(description);
            Relocate(location);

            Apply(new カレンダーエントリー再スケジュール時(this.tenant, this.calendarId, this.calendarEntryId, timeSpan, repetition, alarm));
        }

        void When(カレンダーエントリー再スケジュール時 e)
        {
            this.timeSpan = e.TimeSpan;
            this.repetition = e.Repetition;
            this.alarm = e.Alarm;
        }


        public void Uninvite(参加者 participant)
        {
            AssertionConcern.AssertArgumentNotNull(participant, "The participant must be provided.");

            if (this.invitees.Contains(participant))
            {
                Apply(new カレンダーエントリー参加者非招待時(this.tenant, this.calendarId, this.calendarEntryId, participant));
            }
        }

        void When(カレンダーエントリー参加者非招待時 e)
        {
            this.invitees.Remove(e.Participant);
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return this.tenant;
            yield return this.calendarId;
            yield return this.calendarEntryId;
        }
    }
}
