﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class バックログアイテム削除用マーク時 : IDomainEvent
    {
        public バックログアイテム削除用マーク時(Tenants.テナントId tenantId, バックログアイテムId backlogItemId)
        {
            this.TenantId = tenantId;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.UtcNow;
            this.BacklogItemId = backlogItemId;
        }

        public Tenants.テナントId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
        public バックログアイテムId BacklogItemId { get; private set; }
    }
}
