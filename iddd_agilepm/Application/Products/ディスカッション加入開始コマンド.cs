using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.AgilePM.Application.Products
{
    public class ディスカッション加入開始コマンド
    {
        public ディスカッション加入開始コマンド()
        {
        }

        public ディスカッション加入開始コマンド(string tenantId, string productId)
        {
            this.TenantId = tenantId;
            this.ProductId = productId;
        }

        public string TenantId { get; set; }

        public string ProductId { get; set; }
    }
}
