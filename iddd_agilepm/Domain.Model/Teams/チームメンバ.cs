using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.AgilePM.Domain.Model.Tenants;

namespace SaaSOvation.AgilePM.Domain.Model.Teams
{
    public class チームメンバ : メンバ
    {
        public チームメンバ(TenantId tenantId, string userName, string firstName, string lastName, string emailAddress, DateTime initializedOn)
            : base(tenantId, userName, firstName, lastName, emailAddress, initializedOn)
        {
        }

        public チームメンバId TeamMemberId
        {
            get 
            {
                // TODO: consider length restrictions on TeamMemberId.Id
                return new チームメンバId(this.TenantId, this.Username); 
            }
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return this.TenantId;
            yield return this.Username;
        }
    }
}
