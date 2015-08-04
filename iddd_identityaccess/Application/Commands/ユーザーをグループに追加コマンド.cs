using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.IdentityAccess.Application.Commands
{
    public class ユーザーをグループに追加コマンド
    {
        public ユーザーをグループに追加コマンド()
        {
        }

        public ユーザーをグループに追加コマンド(string tenantId, string groupName, string userNmae)
        {
            this.TenantId = tenantId;
            this.GroupName = groupName;
            this.Username = userNmae;
        }

        public string TenantId { get; set; }
        public string GroupName { get; set; }
        public string Username { get; set; }
    }
}
