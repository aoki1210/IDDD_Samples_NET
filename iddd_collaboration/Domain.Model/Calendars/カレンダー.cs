using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダー : EventSourcedRootEntity
    {
        public カレンダー(テナント tenant, カレンダーId calendarId, string name, string description, オーナ owner, IEnumerable<カレンダー共有> sharedWith = null)
        {
            AssertionConcern.AssertArgumentNotNull(tenant, "The tenant must be provided.");
            AssertionConcern.AssertArgumentNotNull(calendarId, "The calendar id must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(name, "The name must be provided.");
            AssertionConcern.AssertArgumentNotEmpty(description, "The description must be provided.");
            AssertionConcern.AssertArgumentNotNull(owner, "The owner must be provided.");
            Apply(new カレンダー作成時(tenant, calendarId, name, description, owner, sharedWith));
        }

        void When(カレンダー作成時 e)
        {
            this.tenant = e.Tenant;
            this.calendarId = e.CalendarId;
            this.name = e.Name;
            this.description = e.Description;
            this.sharedWith = new HashSet<カレンダー共有>(e.SharedWith ?? Enumerable.Empty<カレンダー共有>());
        }

        public カレンダー(IEnumerable<IDomainEvent> eventStream, int streamVersion)
            : base(eventStream, streamVersion)
        {
        }

        テナント tenant;
        カレンダーId calendarId;
        string name;
        string description;
        HashSet<カレンダー共有> sharedWith;

        public カレンダーId CalendarId
        {
            get { return this.calendarId; }
        }

        public ReadOnlyCollection<カレンダー共有> AllSharedWith
        {
            get { return new ReadOnlyCollection<カレンダー共有>(this.sharedWith.ToArray()); }
        }

        public void ChangeDescription(string description)
        {
            AssertionConcern.AssertArgumentNotEmpty(description, "The description must be provided.");
            Apply(new カレンダー詳細変更時(this.tenant, this.calendarId, this.name, description));
        }

        void When(カレンダー詳細変更時 e)
        {
            this.description = e.Description;
        }

        public void Rename(string name)
        {
            AssertionConcern.AssertArgumentNotEmpty(name, "The name must be provided.");
            Apply(new カレンダー名変更時(this.tenant, this.calendarId, name, this.description));
        }

        void When(カレンダー名変更時 e)
        {
            this.name = e.Name;
        }


        public カレンダーエントリー ScheduleCalendarEntry(
            カレンダー識別サービス calendarIdService,
            string description,
            string location,
            オーナ owner,
            データレンジ timeSpan,
            リピート repetition,
            アラーム alarm,
            IEnumerable<参加者> invitees = null)
        {
            return new カレンダーエントリー(
                this.tenant,
                this.calendarId,
                calendarIdService.GetNextCalendarEntryId(),
                description,
                location,
                owner,
                timeSpan,
                repetition,
                alarm,
                invitees);
        }

        public void ShareCalendarWith(カレンダー共有 calendarSharer)
        {
            AssertionConcern.AssertArgumentNotNull(calendarSharer, "The calendar sharer must be provided.");
            if (!this.sharedWith.Contains(calendarSharer))
            {
                Apply(new カレンダー共有時(this.tenant, this.calendarId, this.name, calendarSharer));
            }
        }

        void When(カレンダー共有時 e)
        {
            this.sharedWith.Add(e.SharedWith);
        }


        public void UnshareCalendarWith(カレンダー共有 calendarSharer)
        {
            AssertionConcern.AssertArgumentNotNull(calendarSharer, "The calendar sharer must be provided.");
            if (this.sharedWith.Contains(calendarSharer))
            {
                Apply(new カレンダー非共有時(this.tenant, this.calendarId, this.name, calendarSharer));
            }
        }

        void When(カレンダー非共有時 e)
        {
            this.sharedWith.Remove(e.UnsharedWith);
        }


        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return this.tenant;
            yield return this.calendarId;
        }
    }
}
