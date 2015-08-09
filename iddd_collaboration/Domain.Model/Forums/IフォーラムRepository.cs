using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Collaboration.Domain.Model.Forums
{
    public interface IフォーラムRepository
    {
        フォーラム Get(Tenants.テナント tenantId, フォーラムId forumId);
        
        フォーラムId GetNextIdentity();

        void Save(フォーラム forum);
    }
}
