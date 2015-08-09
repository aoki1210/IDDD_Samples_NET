using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.Collaboration.Domain.Model.Tenants;
using SaaSOvation.Collaboration.Domain.Model.Collaborators;

namespace SaaSOvation.Collaboration.Domain.Model.Forums
{
    public class フォーラムモデレータ変更時 : IDomainEvent
    {
        public フォーラムモデレータ変更時(テナント tenantId, フォーラムId forumId, モデレータ moderator,string exclusiveOwner)
        {
            this.TenantId = tenantId;
            this.ForumId = forumId;
            this.Moderator = moderator;
            this.ExclusiveOwner = exclusiveOwner;
        }

        public テナント TenantId { get; private set; }
        public フォーラムId ForumId { get; private set; }
        public モデレータ Moderator { get; private set; }
        public string ExclusiveOwner { get; private set; }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}
