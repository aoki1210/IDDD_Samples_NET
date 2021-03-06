﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class タスク時間残見積り時 : IDomainEvent
    {
        public タスク時間残見積り時(Tenants.テナントId tenantId, バックログアイテムId backlogItemId, タスクId taskId, int hoursRemaining)
        {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.TaskId = taskId;
            this.HoursRemaining = hoursRemaining;
        }

        public Tenants.テナントId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }

        public バックログアイテムId BacklogItemId { get; private set; }
        public タスクId TaskId { get; private set; }
        public int HoursRemaining { get; private set; }
    }
}
