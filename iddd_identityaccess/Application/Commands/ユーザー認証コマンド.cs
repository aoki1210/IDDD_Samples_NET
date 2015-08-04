using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.IdentityAccess.Application.Commands
{
    public class ユーザー認証コマンド
    {
        public ユーザー認証コマンド()
        {
        }

        public ユーザー認証コマンド(string tenantId, string userName, string password)
        {
            this.TenantId = tenantId;
            this.Username = userName;
            this.Password = password;
        }

        public string TenantId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
