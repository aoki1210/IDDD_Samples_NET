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
        ICollection<バックログアイテム> GetAllComittedTo(テナントId tenantId, スプリントId sprintId);

        ICollection<バックログアイテム> GetAllScheduledFor(テナントId tenantId, リリースId releaseId);

        ICollection<バックログアイテム> GetAllOutstanding(テナントId tenantId, プロダクトId productId);

        ICollection<バックログアイテム> GetAll(テナントId tenantId, プロダクトId productId);

        バックログアイテム Get(テナントId tenantId, バックログアイテムId backlogItemId);

        バックログアイテムId GetNextIdentity();

        void Remove(バックログアイテム backlogItem);

        void RemoveAll(IEnumerable<バックログアイテム> backlogItems);

        void Save(バックログアイテム backlogItem);

        void SaveAll(IEnumerable<バックログアイテム> backlogItems);
    }
}
