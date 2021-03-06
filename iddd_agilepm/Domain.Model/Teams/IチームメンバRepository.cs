﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.AgilePM.Domain.Model.Teams
{
    public interface IチームメンバRepository
    {
        ICollection<チームメンバ> GetAllTeamMembers(Tenants.テナントId tenantId);

        void Remove(チームメンバ teamMember);

        void RemoveAll(IEnumerable<チームメンバ> teamMembers);

        void Save(チームメンバ teamMember);

        void SaveAll(IEnumerable<チームメンバ> teamMembers);

        チームメンバ Get(Tenants.テナントId tenantId, string userName);
    }
}
