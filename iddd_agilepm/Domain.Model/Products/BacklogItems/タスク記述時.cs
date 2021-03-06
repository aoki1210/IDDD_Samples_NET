﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class タスク記述時 : IDomainEvent
    {
        public タスク記述時(Tenants.テナントId tenantId, バックログアイテムId backlogItemId, タスクId taskId, string description)
        {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.TaskId = taskId;
            this.Description = description;
        }

        public Tenants.テナントId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }

        public バックログアイテムId BacklogItemId { get; private set; }
        public タスクId TaskId { get; private set; }
        public string Description { get; private set; }
    }
}
