using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class リピート : ValueObject
    {
        public static リピート DoesNotRepeat(DateTime ends)
        {
            return new リピート(リピートタイプ.DoesNotRepeat, ends);
        }

        public static リピート RepeatsIndefinitely(リピートタイプ repeatType)
        {
            return new リピート(repeatType, DateTime.MaxValue);
        }

        public リピート(リピートタイプ repeats, DateTime ends)
        {
            this.Repeats = repeats;
            this.Ends = ends;
        }

        public リピートタイプ Repeats { get; private set; }

        public DateTime Ends { get; private set; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Repeats;
            yield return this.Ends;
        }
    }
}
