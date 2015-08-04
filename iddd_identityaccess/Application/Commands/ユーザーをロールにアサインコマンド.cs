using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.IdentityAccess.Application.Commands
{
    public class ユーザーをロールにアサインコマンド
    {
        public ユーザーをロールにアサインコマンド()
        {
        }

        public ユーザーをロールにアサインコマンド(string tenantId, string userName, string roleName)
        {
            this.TenantId = tenantId;
            this.Username = userName;
            this.RoleName = roleName;
        }

        public string TenantId { get; set; }
        public string Username { get; set; }
        public string RoleName { get; set; }
    }
}
