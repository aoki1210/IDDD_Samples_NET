using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Domain.Model.Calendars
{
    public class カレンダーId : SaaSOvation.Common.Domain.Model.Identity
    {
        public カレンダーId() { }

        public カレンダーId(string id)
            : base(id)
        {
        }
    }
}
