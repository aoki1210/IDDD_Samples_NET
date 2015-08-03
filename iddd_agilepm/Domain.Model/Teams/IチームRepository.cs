using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.AgilePM.Domain.Model.Teams
{
    public interface IチームRepository
    {
        ICollection<チーム> GetAllTeams(Tenants.TenantId tenantId);

        void Remove(チーム team);

        void RemoveAll(IEnumerable<チーム> teams);

        void Save(チーム team);

        void SaveAll(IEnumerable<チーム> teams);

        チーム GetByName(Tenants.TenantId tenantId, string name);
    }
}
