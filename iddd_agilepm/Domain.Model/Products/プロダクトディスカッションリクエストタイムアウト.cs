using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model.LongRunningProcess;

namespace SaaSOvation.AgilePM.Domain.Model.Products
{
    public class プロダクトディスカッションリクエストタイムアウト : プロセスタイムアウト
    {
        public プロダクトディスカッションリクエストタイムアウト(string tenantId, プロセスId processId, int totalRetriedPermitted, int retryCount)
            : base(tenantId, processId, totalRetriedPermitted, retryCount)
        {
        }
    }
}
