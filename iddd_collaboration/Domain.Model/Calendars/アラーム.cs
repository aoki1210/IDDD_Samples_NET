using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class アラーム : ValueObject
    {
        public アラーム(アラームユニットタイプ alarmUnitsType, int alarmUnits)
        {
            this.alarmUnitsType = alarmUnitsType;
            this.alarmUnits = alarmUnits;
        }

        readonly int alarmUnits;
        readonly アラームユニットタイプ alarmUnitsType;

        public アラームユニットタイプ AlarmUnitsType
        {
            get { return this.alarmUnitsType; }
        }

        public int AlarmUnits
        {
            get { return this.alarmUnits; }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.alarmUnits;
            yield return this.alarmUnitsType;
        }
    }
}
