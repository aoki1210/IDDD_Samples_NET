using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.IdentityAccess.Domain.Model.Identity
{
    public class ユーザー有効化変更時 : SaaSOvation.Common.Domain.Model.IDomainEvent
    {
        public ユーザー有効化変更時(
                テナントId tenantId,
                String username,
                有効化 enablement)
        {
            this.Enablement = enablement;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
            this.TenantId = tenantId.Id;
            this.Username = username;
        }

        public 有効化 Enablement { get; private set; }

        public int EventVersion { get; set; }

        public DateTime OccurredOn { get; set; }

        public string TenantId { get; private set; }

        public string Username { get; private set; }
    }
}
