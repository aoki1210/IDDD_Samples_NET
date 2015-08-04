using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class バックログアイテムディスカッションリクエスト時 : IDomainEvent
    {
        public バックログアイテムディスカッションリクエスト時(Tenants.テナントId tenantId, プロダクトId productId, バックログアイテムId backlogItemId, bool isRequested)
        {
            this.TenantId = tenantId;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.UtcNow;
            this.ProductId = productId;
            this.BacklogItemId = backlogItemId;
            this.IsRequested = isRequested;
        }

        public Tenants.テナントId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
        public プロダクトId ProductId { get; private set; }
        public バックログアイテムId BacklogItemId { get; private set; }

        public bool IsRequested { get; private set; }
    }
}
