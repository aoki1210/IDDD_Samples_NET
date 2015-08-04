using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.IdentityAccess.Domain.Model.Identity
{
    public class ユーザー登録時 : SaaSOvation.Common.Domain.Model.IDomainEvent
    {
        public ユーザー登録時(
                テナントId tenantId,
                String username,
                フルネーム name,
                Emailアドレス emailAddress)
        {
            this.EmailAddress = emailAddress;
            this.EventVersion = 1;
            this.Name = name;
            this.OccurredOn = DateTime.Now;
            this.TenantId = tenantId.Id;
            this.Username = username;
        }

        public Emailアドレス EmailAddress { get; private set; }

        public int EventVersion { get; set; }

        public フルネーム Name { get; private set; }

        public DateTime OccurredOn { get; set; }

        public string TenantId { get; private set; }

        public string Username { get; private set; }
    }
}
