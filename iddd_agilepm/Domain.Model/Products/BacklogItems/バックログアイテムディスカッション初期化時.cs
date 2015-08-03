using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class バックログアイテムディスカッション初期化時 : IDomainEvent
    {
        public バックログアイテムディスカッション初期化時(Tenants.TenantId tenantId, BacklogItemId backlogItemId, バックログアイテムディスカッション discussion)
        {
            this.TenantId = tenantId;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.UtcNow;
            this.BacklogItemId = backlogItemId;

            this.Discussion = discussion;
        }

        public Tenants.TenantId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
        public BacklogItemId BacklogItemId { get; private set; }

        public バックログアイテムディスカッション Discussion { get; private set; }
    }
}
