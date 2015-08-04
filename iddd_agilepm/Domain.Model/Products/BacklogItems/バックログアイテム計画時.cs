using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class バックログアイテム計画時 : IDomainEvent
    {
        public バックログアイテム計画時(Tenants.テナントId tenantId, バックログアイテムId backlogItemId, Releases.リリースId releaseId)
        {
            this.TenantId = tenantId;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.UtcNow;
            this.BacklogItemId = backlogItemId;

            this.ReleaseId = releaseId;
        }

        public Tenants.テナントId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
        public バックログアイテムId BacklogItemId { get; private set; }

        public Releases.リリースId ReleaseId { get; private set; }
    }
}
