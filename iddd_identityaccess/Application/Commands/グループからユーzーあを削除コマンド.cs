using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.IdentityAccess.Application.Commands
{
    public class グループからユーzーあを削除コマンド
    {
        public グループからユーzーあを削除コマンド()
        {
        }

        public グループからユーzーあを削除コマンド(String tenantId, String groupName, String username)
        {
            this.TenantId = tenantId;
            this.GroupName = groupName;
            this.Username = username;
        }

        public string TenantId { get; set; }
        public string GroupName { get; set; }
        public string Username { get; set; }
    }
}
