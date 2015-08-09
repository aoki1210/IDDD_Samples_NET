using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems;
using SaaSOvation.AgilePM.Domain.Model.Products.Sprints;
using SaaSOvation.AgilePM.Domain.Model.Tenants;

namespace SaaSOvation.AgilePM.Application.Sprints
{
    public class スプリントアプリケーションサービス
    {
        public スプリントアプリケーションサービス(IスプリントRepository sprintRepository, IバックログアイテムRepository backlogItemRepository)
        {
            this.sprintRepository = sprintRepository;
            this.backlogItemRepository = backlogItemRepository;
        }

        readonly IスプリントRepository sprintRepository;
        readonly IバックログアイテムRepository backlogItemRepository;

        public void CommitBacklogItemToSprint(バックログアイテムをスプリントにコミットするコマンド command)
        {
            var tenantId = new テナントId(command.TenantId);
            var sprint = this.sprintRepository.Get(tenantId, new スプリントId(command.SprintId));
            var backlogItem = this.backlogItemRepository.Get(tenantId, new バックログアイテムId(command.BacklogItemId));

            sprint.Commit(backlogItem);

            this.sprintRepository.Save(sprint);
        }

    }
}
