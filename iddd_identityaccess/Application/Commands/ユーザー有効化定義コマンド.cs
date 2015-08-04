using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.IdentityAccess.Application.Commands
{
    public class ユーザー有効化定義コマンド
    {
        public ユーザー有効化定義コマンド()
        {
        }

        public ユーザー有効化定義コマンド(string tenantId, string userName, string enabled, DateTime startDate, DateTime endDate)
        {
            this.TenantId = tenantId;
            this.Username = userName;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        public string TenantId { get; set; }
        public string Username { get; set; }
        public bool Enabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
