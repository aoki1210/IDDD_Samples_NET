﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class バックログアイテム計画除外時 : IDomainEvent
    {
        public バックログアイテム計画除外時(Tenants.テナントId tenantId, バックログアイテムId backlogItemId, Releases.リリースId unscheduledReleaseId)
        {
            this.TenantId = tenantId;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.UtcNow;
            this.BacklogItemId = backlogItemId;

            this.UnscheduledReleaseId = unscheduledReleaseId;
        }

        public Tenants.テナントId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
        public バックログアイテムId BacklogItemId { get; private set; }

        public Releases.リリースId UnscheduledReleaseId { get; private set; }
    }
}
