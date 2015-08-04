using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.IdentityAccess.Domain.Model.Identity;

namespace SaaSOvation.IdentityAccess.Domain.Model.Access
{
    public class ロールプロビジョン時 : SaaSOvation.Common.Domain.Model.IDomainEvent
    {
        public ロールプロビジョン時(テナントId tenantId, string name)
        {
            this.EventVersion = 1;
            this.Name = name;
            this.OccurredOn = DateTime.Now;
            this.TenantId = tenantId.Id;
        }

        public int EventVersion { get; set; }

        public string Name { get; private set; }

        public DateTime OccurredOn { get; set; }

        public string TenantId { get; private set; }
    }
}
