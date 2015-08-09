using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Domain.Model.Forums
{
    public interface IディスカッションRepository
    {
        ディスカッション Get(Tenants.テナント tenantId, ディスカッションId discussionId);

        ディスカッションId GetNextIdentity();

        void Save(ディスカッション discussion);
    }
}
