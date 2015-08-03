using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.AgilePM.Domain.Model.Tenants;
using SaaSOvation.AgilePM.Domain.Model.Products.Sprints;
using SaaSOvation.AgilePM.Domain.Model.Products.Releases;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public interface IバックログアイテムRepository
    {
        ICollection<バックログアイテム> GetAllComittedTo(TenantId tenantId, スプリントId sprintId);

        ICollection<バックログアイテム> GetAllScheduledFor(TenantId tenantId, リリースId releaseId);

        ICollection<バックログアイテム> GetAllOutstanding(TenantId tenantId, プロダクトId productId);

        ICollection<バックログアイテム> GetAll(TenantId tenantId, プロダクトId productId);

        バックログアイテム Get(TenantId tenantId, BacklogItemId backlogItemId);

        BacklogItemId GetNextIdentity();

        void Remove(バックログアイテム backlogItem);

        void RemoveAll(IEnumerable<バックログアイテム> backlogItems);

        void Save(バックログアイテム backlogItem);

        void SaveAll(IEnumerable<バックログアイテム> backlogItems);
    }
}
