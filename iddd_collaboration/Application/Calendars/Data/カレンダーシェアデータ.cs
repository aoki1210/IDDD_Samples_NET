﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Application.Calendars.Data
{
    public class カレンダーシェアデータ
    {
        public string CalendarId { get; set; }
        public string ParticipantEmailAddress { get; set; }
        public string ParticipantIdentity { get; set; }
        public string ParticipantName { get; set; }
        public string TenantId { get; set; }
    }
}
