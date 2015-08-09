using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.AgilePM.Application.Products
{
    public class リクエストプロダクトディスカッションコマンド
    {
        public リクエストプロダクトディスカッションコマンド()
        {
        }

        public リクエストプロダクトディスカッションコマンド(string tenantId, string productId)
        {
            this.TenantId = tenantId;
            this.ProductId = productId;
        }

        public string TenantId { get; set; }

        public string ProductId { get; set; }
    }
}
