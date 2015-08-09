using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class ビジネス優先度アサイン時 : IDomainEvent
    {
        public ビジネス優先度アサイン時(Tenants.テナントId tenantId, バックログアイテムId backlogItemId, ビジネス優先度 businessPriority)
        {
            this.TenantId = tenantId;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.UtcNow;

            this.BacklogItemId = backlogItemId;
            this.BusinessPriority = businessPriority;
        }

        public Tenants.テナントId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }

        public バックログアイテムId BacklogItemId { get; private set; }
        public ビジネス優先度 BusinessPriority { get; private set; }
    }
}
