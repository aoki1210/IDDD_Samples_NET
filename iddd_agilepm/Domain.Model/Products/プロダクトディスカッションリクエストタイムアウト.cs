using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model.LongRunningProcess;

namespace SaaSOvation.AgilePM.Domain.Model.Products
{
    public class プロダクトディスカッションリクエストタイムアウト : ProcessTimedOut
    {
        public プロダクトディスカッションリクエストタイムアウト(string tenantId, ProcessId processId, int totalRetriedPermitted, int retryCount)
            : base(tenantId, processId, totalRetriedPermitted, retryCount)
        {
        }
    }
}
