﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class バックログアイテム未コミット時 : IDomainEvent
    {
        public バックログアイテム未コミット時(Tenants.TenantId tenantId, BacklogItemId backlogItemId, Sprints.スプリントId sprintId)
        {
            this.TenantId = tenantId;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.UtcNow;
            this.BacklogItemId = backlogItemId;

            this.SprintId = sprintId;
        }

        public Tenants.TenantId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
        public BacklogItemId BacklogItemId { get; private set; }

        public Sprints.スプリントId SprintId { get; private set; }
    }
}
