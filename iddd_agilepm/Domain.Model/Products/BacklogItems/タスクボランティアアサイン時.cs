using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class タスクボランティアアサイン時 : IDomainEvent
    {
        public タスクボランティアアサイン時(Tenants.TenantId tenantId, BacklogItemId backlogItemId, タスクId taskId, string volunteerMemberId)
        {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.TaskId = taskId;
            this.VolunteerMemberId = volunteerMemberId;
        }

        public Tenants.TenantId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }

        public BacklogItemId BacklogItemId { get; private set; }
        public タスクId TaskId { get; private set; }
        public string VolunteerMemberId { get; private set; }
    }
}
