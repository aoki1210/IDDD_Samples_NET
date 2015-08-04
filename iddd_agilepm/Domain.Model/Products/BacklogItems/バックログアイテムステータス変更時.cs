using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class バックログアイテムステータス変更時 : IDomainEvent
    {
        public バックログアイテムステータス変更時(Tenants.テナントId tenantId, バックログアイテムId backlogItemId, バックログアイテムステータス status)
        {
            this.TenantId = tenantId;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.UtcNow;
            this.BacklogItemId = backlogItemId;

            this.Status = status;
        }

        public Tenants.テナントId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
        public バックログアイテムId BacklogItemId { get; private set; }

        public バックログアイテムステータス Status { get; private set; }
    }
}
