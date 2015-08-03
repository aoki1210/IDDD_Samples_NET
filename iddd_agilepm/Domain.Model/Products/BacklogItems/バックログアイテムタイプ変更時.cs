﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class バックログアイテムタイプ変更時 : IDomainEvent
    {
        public バックログアイテムタイプ変更時(Tenants.TenantId tenantId, BacklogItemId backlogItemId, バックログアイテムタイプ type)
        {
            this.TenantId = tenantId;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.UtcNow;
            this.BacklogItemId = backlogItemId;

            this.BacklogItemType = type;
        }

        public Tenants.TenantId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
        public BacklogItemId BacklogItemId { get; private set; }

        public バックログアイテムタイプ BacklogItemType { get; private set; }
    }
}
