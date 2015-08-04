using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaaSOvation.Common.Domain.Model;
using SaaSOvation.AgilePM.Domain.Model.Tenants;
using SaaSOvation.AgilePM.Domain.Model.Teams;

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    public class タスク : EntityWithCompositeId
    {
        public タスク(
            テナントId tenantId, 
            バックログアイテムId backlogItemId, 
            タスクId taskId, 
            チームメンバ teamMember, 
            string name, 
            string description, 
            int hoursRemaining, 
            タスクステータス status)
        {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.TaskId = taskId;
            this.Volunteer = teamMember.TeamMemberId;
            this.Name = name;
            this.Description = description;
            this.HoursRemaining = hoursRemaining;
            this.Status = status;
            this.estimationLog = new List<見積りログエントリー>();
        }

        チームメンバId volunteer;
        List<見積りログエントリー> estimationLog;
        string name;
        string description;

        public テナントId TenantId { get; private set; }

        internal バックログアイテムId BacklogItemId { get; private set; }

        internal タスクId TaskId { get; private set; }

        public string Name
        {
            get { return this.name; }
            private set
            {
                AssertionConcern.AssertArgumentNotEmpty(value, "Name is required.");
                AssertionConcern.AssertArgumentLength(value, 100, "Name must be 100 characters or less.");
                this.name = value;
            }
        }

        public string Description
        {
            get { return this.description; }
            private set
            {
                AssertionConcern.AssertArgumentNotEmpty(value, "Description is required.");
                AssertionConcern.AssertArgumentLength(value, 65000, "Description must be 65000 characters or less.");
                this.description = value;
            }
        }

        public タスクステータス Status { get; private set; }

        public チームメンバId Volunteer
        {
            get { return this.volunteer; }
            private set
            {
                AssertionConcern.AssertArgumentNotNull(value, "The volunteer id must be provided.");
                AssertionConcern.AssertArgumentEquals(this.TenantId, value.TenantId, "The volunteer must be of the same tenant.");
                this.volunteer = value;
            }
        }
       
        internal int HoursRemaining { get; private set; }        

        internal void AssignVolunteer(チームメンバ teamMember)
        {
            AssertionConcern.AssertArgumentNotNull(teamMember, "A volunteer must be provided.");
            this.Volunteer = teamMember.TeamMemberId;
            DomainEventPublisher.Instance.Publish(
                new タスクボランティアアサイン時(this.TenantId, this.BacklogItemId, this.TaskId, teamMember.TeamMemberId.Id));
        }

        internal void ChangeStatus(タスクステータス status)
        {
            this.Status = status;
            DomainEventPublisher.Instance.Publish(
                new タスクステータス変更時(this.TenantId, this.BacklogItemId, this.TaskId, status));
        }

        internal void DescribeAs(string description)
        {
            this.Description = description;
            DomainEventPublisher.Instance.Publish(
                new タスク記述時(this.TenantId, this.BacklogItemId, this.TaskId, description));
        }

        internal void EstimateHoursRemaining(int hoursRemaining)
        {
            if (hoursRemaining < 0)
                throw new ArgumentOutOfRangeException("hoursRemaining");

            if (hoursRemaining != this.HoursRemaining)
            {
                this.HoursRemaining = hoursRemaining;
                DomainEventPublisher.Instance.Publish(
                    new タスク時間残見積り時(this.TenantId, this.BacklogItemId, this.TaskId, hoursRemaining));

                if (hoursRemaining == 0 && this.Status != タスクステータス.Done)
                {
                    ChangeStatus(タスクステータス.Done);
                }
                else if (hoursRemaining > 0 && this.Status != タスクステータス.InProgress)
                {
                    ChangeStatus(タスクステータス.InProgress);
                }

                LogEstimation(hoursRemaining);
            }
        }

        void LogEstimation(int hoursRemaining)
        {
            var today = 見積りログエントリー.CurrentLogDate;
            var updatedLogForToday = this.estimationLog.Any(entry => entry.UpdateHoursRemainingWhenDateMatches(hoursRemaining, today));
            if (updatedLogForToday)
            {
                this.estimationLog.Add(
                    new 見積りログエントリー(this.TenantId, this.TaskId, today, hoursRemaining));
            }
        }

        internal void Rename(string name)
        {
            this.Name = name;
            DomainEventPublisher.Instance.Publish(
                new タスクリネーム時(this.TenantId, this.BacklogItemId, this.TaskId, name));
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return this.TenantId;
            yield return this.BacklogItemId;
            yield return this.TaskId;
        }
    }
}
