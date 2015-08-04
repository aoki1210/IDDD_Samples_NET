using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.IdentityAccess.Application.Commands
{
    public class テナント有効化コマンド
    {
        public テナント有効化コマンド() { }

        public テナント有効化コマンド(string tenantId)
        {
            this.TenantId = tenantId;
        }

        public string TenantId { get; set; }
    }
}
