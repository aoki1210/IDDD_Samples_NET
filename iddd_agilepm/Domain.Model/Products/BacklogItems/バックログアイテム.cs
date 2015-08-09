// Copyright 2012,2013 Vaughn Vernon
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using SaaSOvation.Common.Domain.Model;    
    using SaaSOvation.AgilePM.Domain.Model.Tenants;
    using SaaSOvation.AgilePM.Domain.Model.Teams;
    using SaaSOvation.AgilePM.Domain.Model.Products.Sprints;
    using SaaSOvation.AgilePM.Domain.Model.Products.Releases;
    using SaaSOvation.AgilePM.Domain.Model.Discussions;

    public class バックログアイテム : EntityWithCompositeId
    {
        public バックログアイテム(
            テナントId tenantId,
            プロダクトId productId,
            バックログアイテムId backlogItemId,
            string summary,
            string category,
            バックログアイテムタイプ type,
            バックログアイテムステータス backlogItemStatus,
            ストーリポイント storyPoints)
        {
            this.BacklogItemId = backlogItemId;
            this.Category = category;
            this.ProductId = productId;
            this.Status = backlogItemStatus;
            this.StoryPoints = storyPoints;
            this.Summary = summary;
            this.TenantId = tenantId;
            this.Type = type;

            this.tasks = new List<タスク>();
        }

        readonly List<タスク> tasks;

        public テナントId TenantId { get; private set; }

        public プロダクトId ProductId { get; private set; }

        public バックログアイテムId BacklogItemId { get; private set; }

        string summary;

        public string Summary
        {
            get { return this.summary; }
            private set
            {
                AssertionConcern.AssertArgumentNotEmpty(value, "The summary must be provided.");
                AssertionConcern.AssertArgumentLength(value, 100, "The summary must be 100 characters or less.");
                this.summary = value;
            }
        }

        public string Category { get; private set; }

        public バックログアイテムタイプ Type { get; private set; }

        public バックログアイテムステータス Status { get; private set; }

        public bool IsDone
        {
            get { return this.Status == バックログアイテムステータス.Done; }
        }

        public bool IsPlanned
        {
            get { return this.Status == バックログアイテムステータス.Planned; }
        }

        public bool IsRemoved
        {
            get { return this.Status == バックログアイテムステータス.Removed; }
        }

        public ストーリポイント StoryPoints { get; private set; }

        public string AssociatedIssueId { get; private set; }

        public void AssociateWithIssue(string issueId)
        {
            if (this.AssociatedIssueId == null)
            {
                this.AssociatedIssueId = issueId;
            }
        }

        public ビジネス優先度 BusinessPriority { get; private set; }

        public bool HasBusinessPriority
        {
            get { return this.BusinessPriority != null; }
        }

        public void AssignBusinessPriority(ビジネス優先度 businessPriority)
        {
            this.BusinessPriority = businessPriority;
            DomainEventPublisher.Instance.Publish(
                new ビジネス優先度アサイン時(this.TenantId, this.BacklogItemId, businessPriority));
        }

        public void AssignStoryPoints(ストーリポイント storyPoints)
        {
            this.StoryPoints = storyPoints;
            DomainEventPublisher.Instance.Publish(
                new バックログアイテムストーリポイントアサイン時(this.TenantId, this.BacklogItemId, storyPoints));
        }

        public タスク GetTask(タスクId taskId)
        {
            return this.tasks.FirstOrDefault(x => x.TaskId.Equals(taskId));
        }

        タスク LoadTask(タスクId taskId)
        {
            var task = GetTask(taskId);
            if (task == null)
                throw new InvalidOperationException("Task does not exist.");
            return task;
        }

        public void AssignTaskVolunteer(タスクId taskId, チームメンバ volunteer)
        {
            var task = LoadTask(taskId);
            task.AssignVolunteer(volunteer);
        }

        public void ChangeCategory(string category)
        {
            this.Category = category;
            DomainEventPublisher.Instance.Publish(
                new バックログアイテムカテゴリ変更時(this.TenantId, this.BacklogItemId, category));
        }

        public void ChangeTaskStatus(タスクId taskId, タスクステータス status)
        {
            var task = LoadTask(taskId);
            task.ChangeStatus(status);
        }

        public void ChangeType(バックログアイテムタイプ type)
        {
            this.Type = type;
            DomainEventPublisher.Instance.Publish(
                new バックログアイテムタイプ変更時(this.TenantId, this.BacklogItemId, type));
        }

        public リリースId ReleaseId { get; private set; }

        public bool IsScheduledForRelease
        {
            get { return this.ReleaseId != null; }
        }

        public スプリントId SprintId { get; private set; }

        public bool IsCommittedToSprint
        {
            get { return this.SprintId != null; }
        }

        public void CommitTo(スプリント sprint)
        {
            AssertionConcern.AssertArgumentNotNull(sprint, "Sprint must not be null.");
            AssertionConcern.AssertArgumentEquals(sprint.TenantId, this.TenantId, "Sprint must be of same tenant.");
            AssertionConcern.AssertArgumentEquals(sprint.ProductId, this.ProductId, "Sprint must be of same product.");

            if (!this.IsScheduledForRelease)
                throw new InvalidOperationException("Must be scheduled for release to commit to sprint.");

            if (this.IsCommittedToSprint)
            {
                if (!sprint.SprintId.Equals(this.SprintId))
                {
                    UncommittFromSprint();
                }
            }

            ElevateStatusWith(バックログアイテムステータス.Committed);

            this.SprintId = sprint.SprintId;

            DomainEventPublisher.Instance.Publish(
                new バックログアイテムコミット時(this.TenantId, this.BacklogItemId, sprint.SprintId));
        }

        void ElevateStatusWith(バックログアイテムステータス status)
        {
            if (this.Status == バックログアイテムステータス.Scheduled)
            {
                this.Status = バックログアイテムステータス.Committed;
            }
        }

        public void UncommittFromSprint()
        {
            if (!this.IsCommittedToSprint)
                throw new InvalidOperationException("Not currently committed.");

            this.Status = バックログアイテムステータス.Scheduled;
            var uncommittedSprintId = this.SprintId;
            this.SprintId = null;

            DomainEventPublisher.Instance.Publish(
                new バックログアイテム未コミット時(this.TenantId, this.BacklogItemId, uncommittedSprintId));
        }

        public void DefineTask(チームメンバ volunteer, string name, string description, int hoursRemaining)
        {
            var task = new タスク(
                this.TenantId,
                this.BacklogItemId,
                new タスクId(),
                volunteer,
                name,
                description,
                hoursRemaining,
                タスクステータス.NotStarted);

            this.tasks.Add(task);

            DomainEventPublisher.Instance.Publish(
                new タスク定義時(this.TenantId, this.BacklogItemId, task.TaskId, volunteer.TeamMemberId.Id, name, description));
        }

        public void DescribeTask(タスクId taskId, string description)
        {
            var task = LoadTask(taskId);
            task.DescribeAs(description);
        }

        public バックログアイテムディスカッション Discussion { get; private set; }

        public void FailDiscussionInitiation()
        {
            if (this.Discussion.Availability == ディスカッションアベイラビリティ.Ready)
            {
                this.DiscussionInitiationId = null;
                this.Discussion = バックログアイテムディスカッション.FromAvailability(ディスカッションアベイラビリティ.Failed);
            }
        }

        string discussionInitiationId;

        public string DiscussionInitiationId
        {
            get { return this.discussionInitiationId; }
            private set
            {
                if (value != null)
                    AssertionConcern.AssertArgumentLength(value, 100, "Discussion initiation identity must be 100 characters or less.");
                this.discussionInitiationId = value;
            }
        }

        public void InitiateDiscussion(ディスカッション記述子 descriptor)
        {
            AssertionConcern.AssertArgumentNotNull(descriptor, "The descriptor must not be null.");
            if (this.Discussion.Availability == ディスカッションアベイラビリティ.Requested)
            {
                this.Discussion = this.Discussion.NowReady(descriptor);
                DomainEventPublisher.Instance.Publish(
                    new バックログアイテムディスカッション初期化時(this.TenantId, this.BacklogItemId, this.Discussion));
            }
        }

        public void InitiateDiscussion(バックログアイテムディスカッション discussion)
        {
            this.Discussion = discussion;
            DomainEventPublisher.Instance.Publish(
                new バックログアイテムディスカッション初期化時(this.TenantId, this.BacklogItemId, discussion));
        }

        public int TotalTaskHoursRemaining
        {
            get { return this.tasks.Select(x => x.HoursRemaining).Sum(); }
        }

        public bool AnyTaskHoursRemaining
        {
            get { return this.TotalTaskHoursRemaining > 0; }
        }

        public void EstimateTaskHoursRemaining(タスクId taskId, int hoursRemaining)
        {
            var task = LoadTask(taskId);
            task.EstimateHoursRemaining(hoursRemaining);

            var changedStatus = default(バックログアイテムステータス?);

            if (hoursRemaining == 0)
            {
                if (!this.AnyTaskHoursRemaining)
                {
                    changedStatus = バックログアイテムステータス.Done;
                }
            }
            else if (this.IsDone)
            {
                if (this.IsCommittedToSprint)
                {
                    changedStatus = バックログアイテムステータス.Committed;
                }
                else if (this.IsScheduledForRelease)
                {
                    changedStatus = バックログアイテムステータス.Scheduled;
                }
                else
                {
                    changedStatus = バックログアイテムステータス.Planned;
                }
            }

            if (changedStatus != null)
            {
                this.Status = changedStatus.Value;
                DomainEventPublisher.Instance.Publish(
                    new バックログアイテムステータス変更時(this.TenantId, this.BacklogItemId, changedStatus.Value));
            }
        }

        public void MarkAsRemoved()
        {
            if (this.IsRemoved)
                throw new InvalidOperationException("Already removed, not outstanding.");
            if (this.IsDone)
                throw new InvalidOperationException("Already done, not outstanding.");
            
            if (this.IsCommittedToSprint)
            {
                UncommittFromSprint();
            }

            if (this.IsScheduledForRelease)
            {
                UnscheduleFromRelease();
            }

            this.Status = バックログアイテムステータス.Removed;

            DomainEventPublisher.Instance.Publish(
                new バックログアイテム削除用マーク時(this.TenantId, this.BacklogItemId));
        }

        public void UnscheduleFromRelease()
        {
            if (this.IsCommittedToSprint)
                throw new InvalidOperationException("Must first uncommit.");
            if (!this.IsScheduledForRelease)
                throw new InvalidOperationException("Not scheduled for release.");

            this.Status = バックログアイテムステータス.Planned;
            var unscheduledReleaseId = this.ReleaseId;
            this.ReleaseId = null;

            DomainEventPublisher.Instance.Publish(
                new バックログアイテム計画除外時(this.TenantId, this.BacklogItemId, unscheduledReleaseId));
        }

        public void RemoveTask(タスクId taskId)
        {
            var task = LoadTask(taskId);

            if (!this.tasks.Remove(task))
                throw new InvalidOperationException("Task was not removed.");

            DomainEventPublisher.Instance.Publish(
                new タスク削除時(this.TenantId, this.BacklogItemId));
        }

        public void RenameTask(タスクId taskId, string name)
        {
            var task = LoadTask(taskId);
            task.Rename(name);
        }

        public void RequestDiscussion(ディスカッションアベイラビリティ availability)
        {
            if (this.Discussion.Availability != ディスカッションアベイラビリティ.Ready)
            {
                this.Discussion = バックログアイテムディスカッション.FromAvailability(availability);

                DomainEventPublisher.Instance.Publish(
                    new バックログアイテムディスカッションリクエスト時(
                        this.TenantId,
                        this.ProductId,
                        this.BacklogItemId,
                        availability == ディスカッションアベイラビリティ.Requested));

            }
        }

        public void ScheduleFor(リリース release)
        {
            AssertionConcern.AssertArgumentNotNull(release, "Release must not be null.");
            AssertionConcern.AssertArgumentEquals(this.TenantId, release.TenantId, "Release must be of same tenant.");
            AssertionConcern.AssertArgumentEquals(this.ProductId, release.ProductId, "Release must be of same product.");

            if (this.IsScheduledForRelease && !this.ReleaseId.Equals(release.ReleaseId))
            {
                UnscheduleFromRelease();
            }

            if (this.Status == バックログアイテムステータス.Planned)
            {
                this.Status = バックログアイテムステータス.Scheduled;
            }

            this.ReleaseId = release.ReleaseId;

            DomainEventPublisher.Instance.Publish(
                new バックログアイテム計画時(this.TenantId, this.BacklogItemId, release.ReleaseId));

        }

        public void StartDiscussionInitiation(string discussionInitiationId)
        {
            if (this.Discussion.Availability != ディスカッションアベイラビリティ.Ready)
            {
                this.DiscussionInitiationId = discussionInitiationId;
            }
        }

        public void Summarize(string summary)
        {
            this.Summary = summary;
            DomainEventPublisher.Instance.Publish(
                new バックログアイテム要約時(this.TenantId, this.BacklogItemId, summary));
        }

        public string Story { get; private set; }

        public void TellStory(string story)
        {
            if (story != null)
                AssertionConcern.AssertArgumentLength(story, 65000, "The story must be 65000 characters or less.");

            this.Story = story;

            DomainEventPublisher.Instance.Publish(
                new バックログアイテムストーリ説明時(this.TenantId, this.BacklogItemId, story));
        }

        protected override IEnumerable<object> GetIdentityComponents()
        {
            yield return this.TenantId;
            yield return this.ProductId;
            yield return this.BacklogItemId;
        }
    }
}
