using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.IdentityAccess.Domain.Model.Identity;

namespace SaaSOvation.IdentityAccess.Domain.Model.Access
{
    public class ユーザーロールアサイン解除時 : SaaSOvation.Common.Domain.Model.IDomainEvent
    {
        public ユーザーロールアサイン解除時(テナントId tenantId, string roleName, string username)
        {
            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
            this.RoleName = roleName;
            this.TenantId = tenantId;
            this.Username = username;
        }

        public int EventVersion { get; set; }

        public string Username { get; private set; }

        public DateTime OccurredOn { get; set; }

        public string RoleName { get; private set; }

        public テナントId TenantId { get; private set; }
    }
}
