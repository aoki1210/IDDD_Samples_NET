using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.AgilePM.Application.Teams
{
    public class プロダクトオーナーコマンド無効化 : メンバー無効化コマンド
    {
        public プロダクトオーナーコマンド無効化()
        {
        }

        public プロダクトオーナーコマンド無効化(string tenantId, string username, DateTime occurredOn)
            : base(tenantId, username, occurredOn)
        {
        }
    }
}
