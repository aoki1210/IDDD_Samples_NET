using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.IdentityAccess.Domain.Model.Identity
{
    public class グループグループ削除時 : SaaSOvation.Common.Domain.Model.IDomainEvent
    {
        public グループグループ削除時(テナントId tenantId, string groupName, string nestedGroupName)
        {
            this.EventVersion = 1;
            this.GroupName = groupName;
            this.NestedGroupName = nestedGroupName;
            this.OccurredOn = DateTime.Now;
            this.TenantId = tenantId.Id;
        }

        public int EventVersion { get; set; }

        public string GroupName { get; private set; }

        public string NestedGroupName { get; private set; }

        public DateTime OccurredOn { get; set; }

        public string TenantId { get; private set; }
    }

}
