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
    public class カレンダーエントリーアプリケーションサービス
    {
        public カレンダーエントリーアプリケーションサービス(IカレンダーエントリーRepository calendarEntryRepository, Iコラボレータサービス collaboratorService)
        {
            this.calendarEntryRepository = calendarEntryRepository;
            this.collaboratorService = collaboratorService;
        }

        readonly IカレンダーエントリーRepository calendarEntryRepository;
        readonly Iコラボレータサービス collaboratorService;

        public void ChangeCalendarEntryDescription(string tenantId, string calendarEntryId, string description)
        {
            var calendarEntry = this.calendarEntryRepository.Get(new テナント(tenantId), new カレンダーエントリーId(calendarEntryId));

            calendarEntry.ChangeDescription(description);

            this.calendarEntryRepository.Save(calendarEntry);
        }

        public void InviteCalendarEntryParticipant(string tenantId, string calendarEntryId, ISet<string> participantsToInvite)
        {
            var tenant = new テナント(tenantId);
            var calendarEntry = this.calendarEntryRepository.Get(tenant, new カレンダーエントリーId(calendarEntryId));

            foreach (var participant in GetInviteesFrom(tenant, participantsToInvite))
            {
                calendarEntry.Invite(participant);
            }

            this.calendarEntryRepository.Save(calendarEntry);
        }

        public void RelocateCalendarEntry(string tenantId, string calendarEntryId, string location)
        {
            var calendarEntry = this.calendarEntryRepository.Get(new テナント(tenantId), new カレンダーエントリーId(calendarEntryId));

            calendarEntry.Relocate(location);

            this.calendarEntryRepository.Save(calendarEntry);
        }

        public void RescheduleCalendarEntry(string tenantId, string calendarEntryId, string description, string location, DateTime timeSpanBegins, DateTime timeSpanEnds,
            string repeatType, DateTime repeatEndsOn, string alarmType, int alarmUnits)
        {
            var calendarEntry = this.calendarEntryRepository.Get(new テナント(tenantId), new カレンダーエントリーId(calendarEntryId));

            calendarEntry.Reschedule(
                description, 
                location, 
                new データレンジ(timeSpanBegins, timeSpanEnds), 
                new リピート((リピートタイプ)Enum.Parse(typeof(リピートタイプ), repeatType), repeatEndsOn),
                new アラーム((アラームユニットタイプ)Enum.Parse(typeof(アラームユニットタイプ), alarmType), alarmUnits));

            this.calendarEntryRepository.Save(calendarEntry);
        }

        public void UninviteCalendarEntryParticipant(string tenantId, string calendarEntryId, ISet<string> participantsToUninvite)
        {
            var tenant = new テナント(tenantId);
            var calendarEntry = this.calendarEntryRepository.Get(tenant, new カレンダーエントリーId(calendarEntryId));

            foreach (var participant in GetInviteesFrom(tenant, participantsToUninvite))
            {
                calendarEntry.Uninvite(participant);
            }

            this.calendarEntryRepository.Save(calendarEntry);
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
    }
}
