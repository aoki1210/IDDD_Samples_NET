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

namespace SaaSOvation.AgilePM.Domain.Model.Products
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using SaaSOvation.AgilePM.Domain.Model.Discussions;
    using SaaSOvation.AgilePM.Domain.Model.Products.BacklogItems;
    using SaaSOvation.AgilePM.Domain.Model.Products.Releases;
    using SaaSOvation.AgilePM.Domain.Model.Products.Sprints;
    using SaaSOvation.AgilePM.Domain.Model.Teams;
    using SaaSOvation.AgilePM.Domain.Model.Tenants;
    using SaaSOvation.Common.Domain.Model;

    public class プロダクト : Entity, IEquatable<プロダクト>
    {
        public プロダクト(
                テナントId tenantId,
                プロダクトId productId,
                プロダクトオーナId productOwnerId,
                string name,
                string description,
                ディスカッションアベイラビリティ discussionAvailability)
        {
            this.TenantId = tenantId; // must precede productOwnerId for compare
            this.Description = description;
            this.Discussion = プロダクトディスカッション.FromAvailability(discussionAvailability);
            this.DiscussionInitiationId = null;
            this.Name = name;
            this.ProductId = productId;
            this.ProductOwnerId = productOwnerId; // TODO: validation currently missing

            DomainEventPublisher
                .Instance
                .Publish(new プロダクト作成時(
                        this.TenantId,
                        this.ProductId,
                        this.ProductOwnerId,
                        this.Name,
                        this.Description,
                        this.Discussion.Availability));
        }

        readonly ISet<プロダクトバックログアイテム> backlogItems;

        public テナントId TenantId { get; private set; } 
        
        public プロダクトId ProductId { get; private set; }

        public プロダクトオーナId ProductOwnerId { get; private set; }

        

        public string Description { get; private set; }

        public プロダクトディスカッション Discussion { get; private set; }

        public string DiscussionInitiationId { get; private set; }

        public string Name { get; private set; }               

        public ICollection<プロダクトバックログアイテム> AllBacklogItems()
        {
            return new ReadOnlyCollection<プロダクトバックログアイテム>(new List<プロダクトバックログアイテム>(this.backlogItems));
        }

        public void ChangeProductOwner(プロダクトオーナ productOwner)
        {
            if (!this.ProductOwnerId.Equals(productOwner.ProductOwnerId))
            {
                this.ProductOwnerId = productOwner.ProductOwnerId;

                // TODO: publish event
            }
        }

        public void FailDiscussionInitiation()
        {
            if (this.Discussion.Availability != ディスカッションアベイラビリティ.Ready)
            {
                this.DiscussionInitiationId = null;

                this.Discussion = プロダクトディスカッション.FromAvailability(ディスカッションアベイラビリティ.Failed);
            }
        }

        public void InitiateDiscussion(ディスカッション記述子 descriptor)
        {
            if (descriptor == null)
            {
                throw new InvalidOperationException("The descriptor must not be null.");
            }

            if (this.Discussion.Availability == ディスカッションアベイラビリティ.Requested)
            {
                this.Discussion = this.Discussion.NowReady(descriptor);

                DomainEventPublisher
                    .Instance
                    .Publish(new プロダクトディスカッション初期時(
                            this.TenantId,
                            this.ProductId,
                            this.Discussion));
            }
        }

        public BacklogItems.バックログアイテム PlanBacklogItem(
                バックログアイテムId newBacklogItemId,
                String summary,
                String category,
                バックログアイテムタイプ type,
                ストーリポイント storyPoints)
        {
            var backlogItem =
                new BacklogItems.バックログアイテム(
                        this.TenantId,
                        this.ProductId,
                        newBacklogItemId,
                        summary,
                        category,
                        type,
                        バックログアイテムステータス.Planned,
                        storyPoints);

            DomainEventPublisher
                .Instance
                .Publish(new プロダクトバックログアイテム計画時(
                        backlogItem.TenantId,
                        backlogItem.ProductId,
                        backlogItem.BacklogItemId,
                        backlogItem.Summary,
                        backlogItem.Category,
                        backlogItem.Type,
                        backlogItem.StoryPoints));

            return backlogItem;
        }

        public void PlannedProductBacklogItem(バックログアイテム backlogItem)
        {
            AssertionConcern.AssertArgumentEquals(this.TenantId, backlogItem.TenantId, "The product and backlog item must have same tenant.");
            AssertionConcern.AssertArgumentEquals(this.ProductId, backlogItem.ProductId, "The backlog item must belong to product.");

            int ordering = this.backlogItems.Count + 1;

            プロダクトバックログアイテム productBacklogItem =
                    new プロダクトバックログアイテム(
                            this.TenantId,
                            this.ProductId,
                            backlogItem.BacklogItemId,
                            ordering);

            this.backlogItems.Add(productBacklogItem);
        }

        public void ReorderFrom(バックログアイテムId id, int ordering)
        {
            foreach (var productBacklogItem in this.backlogItems)
            {
                productBacklogItem.ReorderFrom(id, ordering);
            }
        }

        public void RequestDiscussion(ディスカッションアベイラビリティ discussionAvailability)
        {
            if (this.Discussion.Availability != ディスカッションアベイラビリティ.Ready)
            {
                this.Discussion =
                        プロダクトディスカッション.FromAvailability(discussionAvailability);

                DomainEventPublisher
                    .Instance
                    .Publish(new プロダクトディスカッションリクエスト時(
                            this.TenantId,
                            this.ProductId,
                            this.ProductOwnerId,
                            this.Name,
                            this.Description,
                            this.Discussion.Availability == ディスカッションアベイラビリティ.Requested));
            }
        }

        public リリース ScheduleRelease(
                リリースId newReleaseId,
                String name,
                String description,
                DateTime begins,
                DateTime ends)
        {
            リリース release =
                new リリース(
                        this.TenantId,
                        this.ProductId,
                        newReleaseId,
                        name,
                        description,
                        begins,
                        ends);

            DomainEventPublisher
                .Instance
                .Publish(new プロダクトリリーススケジュール時(
                        release.TenantId,
                        release.ProductId,
                        release.ReleaseId,
                        release.Name,
                        release.Description,
                        release.Begins,
                        release.Ends));

            return release;
        }

        public スプリント ScheduleSprint(
                スプリントId newSprintId,
                String name,
                String goals,
                DateTime begins,
                DateTime ends)
        {
            スプリント sprint =
                new スプリント(
                        this.TenantId,
                        this.ProductId,
                        newSprintId,
                        name,
                        goals,
                        begins,
                        ends);

            DomainEventPublisher
                .Instance
                .Publish(new プロダクトスプリントスケジュール時(
                        sprint.TenantId,
                        sprint.ProductId,
                        sprint.SprintId,
                        sprint.Name,
                        sprint.Goals,
                        sprint.Begins,
                        sprint.Ends));

            return sprint;
        }

        public void StartDiscussionInitiation(String discussionInitiationId)
        {
            if (this.Discussion.Availability != ディスカッションアベイラビリティ.Ready)
            {
                this.DiscussionInitiationId = discussionInitiationId;
            }
        }

        public bool Equals(プロダクト other)
        {
            if (object.ReferenceEquals(this, other)) return true;
            if (object.ReferenceEquals(null, other)) return false;
            return this.TenantId.Equals(other.TenantId)
                && this.ProductId.Equals(other.ProductId);
        }

        public override bool Equals(object anotherObject)
        {
            return Equals(anotherObject as プロダクト);
        }

        public override int GetHashCode()
        {
            return 
                + (2335 * 3)
                + this.TenantId.GetHashCode()
                + this.ProductId.GetHashCode();
        }

        public override string ToString()
        {
            return "Product [tenantId=" + TenantId + ", productId=" + ProductId
                    + ", backlogItems=" + backlogItems + ", description="
                    + Description + ", discussion=" + Discussion
                    + ", discussionInitiationId=" + DiscussionInitiationId
                    + ", name=" + Name + ", productOwnerId=" + ProductOwnerId + "]";
        }
    }
}
