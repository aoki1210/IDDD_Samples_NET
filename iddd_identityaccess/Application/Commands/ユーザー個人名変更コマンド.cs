using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.IdentityAccess.Application.Commands
{
    public class ユーザー個人名変更コマンド
    {
        public ユーザー個人名変更コマンド()
        {
        }

        public ユーザー個人名変更コマンド(string tenantId, string userName, string firstName, string lastName)
        {
            this.TenantId = tenantId;
            this.Username = userName;
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public string TenantId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
