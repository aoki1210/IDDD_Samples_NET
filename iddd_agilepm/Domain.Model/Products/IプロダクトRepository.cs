using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.AgilePM.Domain.Model.Products
{
    public interface IプロダクトRepository
    {
        ICollection<プロダクト> GetAllByTenant(Tenants.テナントId tenantId);

        プロダクトId GetNextIdentity();

        プロダクト GetByDiscussionInitiationId(Tenants.テナントId tenantId, string discussionInitiationId);

        プロダクト Get(Tenants.テナントId tenantId, Products.プロダクトId productId);

        void Remove(プロダクト product);

        void RemoveAll(IEnumerable<プロダクト> products);

        void Save(プロダクト product);

        void SaveAll(IEnumerable<プロダクト> products);
    }
}
