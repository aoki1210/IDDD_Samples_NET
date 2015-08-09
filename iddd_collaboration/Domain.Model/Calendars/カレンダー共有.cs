using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダー共有 : ValueObject, IComparable<カレンダー共有>
    {
        public カレンダー共有(参加者 participant)
        {
            AssertionConcern.AssertArgumentNotNull(participant, "Participant must be provided.");
            this.participant = participant;
        }

        readonly 参加者 participant;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.participant;
        }

        public int CompareTo(カレンダー共有 other)
        {
            return this.participant.CompareTo(other.participant);
        }
    }
}
