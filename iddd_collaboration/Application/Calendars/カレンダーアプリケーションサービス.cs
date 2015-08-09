using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Collaboration.Domain.Model.Calendars;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;
using SaaSOvation.Collaboration.Domain.Model.Tenants;

using SaaSOvation.Collaboration.Application.Calendars.Data;

namespace SaaSOvation.Collaboration.Application.Calendars
{
    public class カレンダーアプリケーションサービス
    {
        public カレンダーアプリケーションサービス(IカレンダーRepository calendarRepository, IカレンダーエントリーRepository calendarEntryRepository, カレンダー識別サービス calendarIdentityService, Iコラボレータサービス collaboratorService)
        {
            this.calendarRepository = calendarRepository;
            this.calendarEntryRepository = calendarEntryRepository;
            this.calendarIdentityService = calendarIdentityService;
            this.collaboratorService = collaboratorService;
        }

        readonly IカレンダーRepository calendarRepository;
        readonly IカレンダーエントリーRepository calendarEntryRepository;
        readonly カレンダー識別サービス calendarIdentityService;
        readonly Iコラボレータサービス collaboratorService;

        public void ChangeCalendarDescription(string tenantId, string calendarId, string description)
        {
            var calendar = this.calendarRepository.Get(new テナント(tenantId), new カレンダーId(calendarId));

            calendar.ChangeDescription(description);

            this.calendarRepository.Save(calendar);
        }

        public void CreateCalendar(string tenantId, string name, string description, string ownerId, ISet<string> participantsToShareWith, Iカレンダーコマンド結果 calendarCommandResult)
        {
            var tenant = new テナント(tenantId);
            var owner = this.collaboratorService.GetOwnerFrom(tenant, ownerId);
            var sharers = GetSharersFrom(tenant, participantsToShareWith);

            var calendar = new カレンダー(tenant, this.calendarRepository.GetNextIdentity(), name, description, owner, sharers);

            this.calendarRepository.Save(calendar);

            calendarCommandResult.SetResultingCalendarId(calendar.CalendarId.Id);
        }

        public void RenameCalendar(string tenantId, string calendarId, string name)
        {
            var calendar = this.calendarRepository.Get(new テナント(tenantId), new カレンダーId(calendarId));

            calendar.Rename(name);

            this.calendarRepository.Save(calendar);
        }

        public void ScheduleCalendarEntry(string tenantId, string calendarId, string description, string location, string ownerId, DateTime timeSpanBegins, DateTime timeSpanEnds,
            string repeatType, DateTime repeatEndsOn, string alarmType, int alarmUnits, ISet<string> participantsToInvite, Iカレンダーコマンド結果 calendarCommandResult)
        {
            var tenant = new テナント(tenantId);

            var calendar = this.calendarRepository.Get(tenant, new カレンダーId(calendarId));

            var calendarEntry = calendar.ScheduleCalendarEntry(
                this.calendarIdentityService,
                description,
                location,
                this.collaboratorService.GetOwnerFrom(tenant, ownerId),
                new データレンジ(timeSpanBegins, timeSpanEnds),
                new リピート((リピートタイプ)Enum.Parse(typeof(リピートタイプ), repeatType), repeatEndsOn),
                new アラーム((アラームユニットタイプ)Enum.Parse(typeof(アラームユニットタイプ), alarmType), alarmUnits),
                GetInviteesFrom(tenant, participantsToInvite));

            this.calendarEntryRepository.Save(calendarEntry);

            calendarCommandResult.SetResultingCalendarId(calendar.CalendarId.Id);
            calendarCommandResult.SetResultingCalendarEntryId(calendarEntry.CalendarEntryId.Id);
        }

        public void ShareCalendarWith(string tenantId, string calendarId, ISet<string> participantsToShareWith)
        {
            var tenant = new テナント(tenantId);
            var calendar = this.calendarRepository.Get(tenant, new カレンダーId(calendarId));

            foreach (var sharer in GetSharersFrom(tenant, participantsToShareWith))
            {
                calendar.ShareCalendarWith(sharer);
            }

            this.calendarRepository.Save(calendar);
        }

        public void UnshareCalendarWith(string tenantId, string calendarId, ISet<string> participantsToShareWith)
        {
            var tenant = new テナント(tenantId);
            var calendar = this.calendarRepository.Get(tenant, new カレンダーId(calendarId));

            foreach (var sharer in GetSharersFrom(tenant, participantsToShareWith))
            {
                calendar.UnshareCalendarWith(sharer);
            }

            this.calendarRepository.Save(calendar);
        }

        ISet<参加者> GetInviteesFrom(テナント tenant, ISet<string> participantsToInvite)
        {
            var invitees = new HashSet<参加者>();
            foreach (string participatnId in participantsToInvite)
            {
                var participant = this.collaboratorService.GetParticipantFrom(tenant, participatnId);
                invitees.Add(participant);
            }
            return invitees;
        }

        ISet<カレンダー共有> GetSharersFrom(テナント tenant, ISet<string> participantsToShareWith)
        {
            var sharers = new HashSet<カレンダー共有>();
            foreach (var participatnId in participantsToShareWith)
            {
                var participant = this.collaboratorService.GetParticipantFrom(tenant, participatnId);
                sharers.Add(new カレンダー共有(participant));
            }
            return sharers;
        }
    }
}
