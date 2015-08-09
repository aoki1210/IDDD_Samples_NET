using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Domain.Model.Forums
{
    public interface IポストRepository
    {
        ポスト Get(Tenants.テナント tenantId, ポストId postId);

        ポストId GetNextIdentity();

        void Save(ポスト post);
    }
}
