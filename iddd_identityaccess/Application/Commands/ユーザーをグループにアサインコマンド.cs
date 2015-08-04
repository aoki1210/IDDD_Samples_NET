using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.IdentityAccess.Application.Commands
{
    public class ユーザーをグループにアサインコマンド
    {
        public ユーザーをグループにアサインコマンド()
        {
        }

        public ユーザーをグループにアサインコマンド(string tenantId, string groupName, string userName)
        {
            this.TenantId = tenantId;
            this.GroupName = groupName;
            this.Username = userName;
        }

        public string TenantId { get; set; }
        public string GroupName { get; set; }
        public string Username { get; set; }
    }
}
