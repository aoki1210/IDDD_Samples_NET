using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class タスクステータス変更時 : IDomainEvent
    {
        public タスクステータス変更時(Tenants.TenantId tenantId, BacklogItemId backlogItemId, タスクId taskId, タスクステータス status)
        {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.TaskId = taskId;
            this.Status = status;
        }

        public Tenants.TenantId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }

        public BacklogItemId BacklogItemId { get; private set; }
        public タスクId TaskId { get; private set; }
        public タスクステータス Status { get; private set; }
    }
}
