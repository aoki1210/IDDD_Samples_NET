using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.Common.Domain.Model.LongRunningProcess
{
    public interface I時間制約プロセストラッカーRepository
    {
        void Add(時間制約プロセストラッカー processTracker);

        ICollection<時間制約プロセストラッカー> GetAllTimedOut();

        ICollection<時間制約プロセストラッカー> GetAllTimedOutOf(string tenantId);

        ICollection<時間制約プロセストラッカー> GetAll(string tenantId);

        void Save(時間制約プロセストラッカー processTracker);

        時間制約プロセストラッカー Get(string tenantId, プロセスId processId); 
    }
}
