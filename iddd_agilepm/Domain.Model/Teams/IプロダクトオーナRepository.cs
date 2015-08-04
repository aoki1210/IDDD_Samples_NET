using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.AgilePM.Domain.Model.Tenants;

namespace SaaSOvation.AgilePM.Domain.Model.Teams
{
    public interface IプロダクトオーナRepository
    {
        ICollection<プロダクトオーナ> GetAllProductOwners(テナントId tenantId);

        プロダクトオーナ Get(テナントId tenantId, string userName);

        void Remove(プロダクトオーナ owner);

        void RemoveAll(IEnumerable<プロダクトオーナ> owners);

        void Save(プロダクトオーナ owner);

        void SaveAll(IEnumerable<プロダクトオーナ> owners);
    }
}
