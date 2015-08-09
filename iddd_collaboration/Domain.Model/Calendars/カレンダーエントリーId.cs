using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダーエントリーId : SaaSOvation.Common.Domain.Model.Identity
    {
        public カレンダーエントリーId()
        {
        }

        public カレンダーエントリーId(string id)
            : base(id)
        {
        }
    }
}
