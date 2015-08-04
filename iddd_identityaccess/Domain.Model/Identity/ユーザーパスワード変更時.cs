using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.IdentityAccess.Domain.Model.Identity
{
    public class ユーザーパスワード変更時 : SaaSOvation.Common.Domain.Model.IDomainEvent
    {
        public ユーザーパスワード変更時(
                テナントId tenantId,
                String username)
        {
            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
            this.TenantId = tenantId.Id;
            this.Username = username;
        }

        public int EventVersion { get; set; }

        public DateTime OccurredOn { get; set; }

        public string TenantId { get; private set; }

        public string Username { get; private set; }
    }
}
