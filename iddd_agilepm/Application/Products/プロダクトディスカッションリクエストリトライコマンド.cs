using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.AgilePM.Application.Products
{
    public class プロダクトディスカッションリクエストリトライコマンド
    {
        public プロダクトディスカッションリクエストリトライコマンド()
        {
        }

        public プロダクトディスカッションリクエストリトライコマンド(string tenantId, string processId)
        {
            this.TenantId = tenantId;
            this.ProcessId = processId;
        }

        public string TenantId { get; set; }

        public string ProcessId { get; set; }
    }
}
