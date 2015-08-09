using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaaSOvation.AgilePM.Application.Teams
{
    public class プロダクトオーナー有効化コマンド : メンバー有効化コマンド
    {
        public プロダクトオーナー有効化コマンド()
        {
        }

        public プロダクトオーナー有効化コマンド(string tenantId, string username, string firstName, string lastName, string emailAddress, DateTime occurredOn)
            : base(tenantId, username, firstName, lastName, emailAddress, occurredOn)
        {
        }
    }
}
