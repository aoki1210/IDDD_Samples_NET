using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class 見積りログエントリー : EntityWithCompositeId
    {
        public 見積りログエントリー(Tenants.TenantId tenantId, タスクId taskId, DateTime date, int hoursRemaining)
        {
            AssertionConcern.AssertArgumentNotNull(tenantId, "The tenant id must be provided.");
            AssertionConcern.AssertArgumentNotNull(taskId, "The task id must be provided.");

            this.TenantId = tenantId;
            this.TaskId = taskId;
            this.Date = date.Date;
            this.HoursRemaining = hoursRemaining;
        }

        public static DateTime CurrentLogDate
        {
            get { return DateTime.UtcNow.Date; }
        }

        public Tenants.TenantId TenantId { get; set; }
        public タスクId TaskId { get; private set; }

        public DateTime Date { get; private set; }
        public int HoursRemaining { get; private set; }

        internal bool IsMatching(DateTime date)
        {
            return this.Date.Equals(date);
        }

        internal bool UpdateHoursRemainingWhenDateMatches(int hoursRemaining, DateTime date)
        {
            if (IsMatching(date))
            {
                this.HoursRemaining = hoursRemaining;
                return true;
            }
            return false;
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return this.TenantId;
            yield return this.TaskId;
            yield return this.Date;
            yield return this.HoursRemaining;
        }        
    }
}
    