using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.AgilePM.Domain.Model.Teams
{
    public interface IチームRepository
    {
        ICollection<チーム> GetAllTeams(Tenants.テナントId tenantId);

        void Remove(チーム team);

        void RemoveAll(IEnumerable<チーム> teams);

        void Save(チーム team);

        void SaveAll(IEnumerable<チーム> teams);

        チーム GetByName(Tenants.テナントId tenantId, string name);
    }
}
