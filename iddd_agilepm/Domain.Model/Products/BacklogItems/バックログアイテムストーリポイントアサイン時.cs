using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class �o�b�N���O�A�C�e���X�g�[���|�C���g�A�T�C���� : IDomainEvent
    {
        public �o�b�N���O�A�C�e���X�g�[���|�C���g�A�T�C����(Tenants.�e�i���gId tenantId, �o�b�N���O�A�C�e��Id backlogItemId, �X�g�[���|�C���g storyPoints)
        {
            this.TenantId = tenantId;
            this.EventVersion = 1;
            this.OccurredOn = DateTime.UtcNow;

            this.BacklogItemId = backlogItemId;
            this.StoryPoints = storyPoints;
        }

        public Tenants.�e�i���gId TenantId { get; private set; }
        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }

        public �o�b�N���O�A�C�e��Id BacklogItemId { get; private set; }
        public �X�g�[���|�C���g StoryPoints { get; private set; }
    }
}
