using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.AgilePM.Application.Teams
{
    public class チームメンバー無効化コマンド : メンバー無効化コマンド
    {
        public チームメンバー無効化コマンド()
        {
        }

        public チームメンバー無効化コマンド(string tenantId, string username, DateTime occurredOn)
            : base(tenantId, username, occurredOn)
        {
        }
    }
}
