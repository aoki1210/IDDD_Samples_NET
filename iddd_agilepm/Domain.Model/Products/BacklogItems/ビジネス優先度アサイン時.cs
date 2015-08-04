using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class �r�W�l�X�D��x�A�T�C���� : IDomainEvent
    {
        public �r�W�l�X�D��x�A�T�C����(Tenants.�e�i���gId tenantId, �o�b�N���O�A�C�e��Id backlogItemId, �r�W�l�X�D��x businessPriority)
        {
            this.TenantId = tenantId;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.UtcNow;

            this.BacklogItemId = backlogItemId;
            this.BusinessPriority = businessPriority;
        }

        public Tenants.�e�i���gId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }

        public �o�b�N���O�A�C�e��Id BacklogItemId { get; private set; }
        public �r�W�l�X�D��x BusinessPriority { get; private set; }
    }
}
